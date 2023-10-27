using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StudentPropertiesControl : MonoBehaviour
{
    [SerializeField] public StudentUnit studentUnit;
    [Header("初始选择")] [SerializeField] private Toggle toggle;
    [Header("心情、信任条绑定")] [SerializeField] private MoodControl moodControl;
    [SerializeField] private TrustControl trustControl;
    [SerializeField] private List<Text> basicList;
    [SerializeField] private GameObject gaoKaoObj;
    [Header("预制体")] [SerializeField] private GameObject scorePrefab;

    [Header("主学科页面")] [SerializeField] private List<ScoreEntryControl> mainScores;
    [Header("兴趣学科页面")] private List<ScoreEntryControl> interestGrades;
    [SerializeField] private Transform scoreParent;
    [SerializeField] private GameObject NotInterested;

    [Header("基本属性页面")] [SerializeField] [FormerlySerializedAs("propertys")]
    private List<Text> properties;

    /// <summary>
    /// 肖像的父物体
    /// </summary>
    [SerializeField] private RectTransform portrayalParent;

    /// <summary>
    /// 肖像
    /// </summary>
    [SerializeField] private List<GameObject> studentsPortrayals;

    /// <summary>
    /// 善恶指针
    /// </summary>
    [SerializeField] private RectTransform goodAndEvilPointer;

    public event UnityAction UIUpdateEvent;
    [Header("人际关系页面")] [SerializeField] private Transform friendListParent;
    [SerializeField] private GameObject friendPrefab;
    [SerializeField] private GameObject nptFriends;
    private List<FriendEntryControl> friendControls;

    private void Awake()
    {
        friendControls = new List<FriendEntryControl>();
        interestGrades = new List<ScoreEntryControl>();
    }

    public void IntoPanel(StudentUnit unit)
    {
        studentUnit = unit;
        toggle.isOn = true;
        UIUpdate();
    }


    /// <summary>
    /// 刷新界面
    /// </summary>
    public void UIUpdate()
    {
        gameObject.SetActive(true);
        moodControl.DoubleSet(studentUnit.Mood, studentUnit.Mood);
        trustControl.InitTrust(studentUnit);
        SetBasic();
        SetMainScore();
        SetInterestGrade();
        SetProperty();
        SetPortrayal();
        SetFriendList();
        UIUpdateEvent?.Invoke();
    }

    /// <summary>
    /// 设置肖像
    /// </summary>
    private void SetPortrayal()
    {
        foreach (GameObject obj in studentsPortrayals) obj.SetActive(obj.CompareTag(studentUnit.id));
    }


    /// <summary>
    /// 设置基本信息
    /// </summary>
    private void SetBasic()
    {
        basicList[0].text = studentUnit.fullName;
        basicList[1].text = studentUnit.gender switch
        {
            Gender.None => "未设置",
            Gender.Man => "男",
            Gender.Woman => "女",
            _ => throw new ArgumentOutOfRangeException()
        };
        basicList[2].text = (GameManager.Instance.saveObject.SaveData.gameDate - studentUnit.birthday).year + "岁";
        basicList[3].text = studentUnit.birthday.ToString();
        basicList[4].text = studentUnit.school + "  " + DatetimeManager.Instance.Grade + "年级";
        basicList[5].text = studentUnit.personalData;
        if (GameManager.Instance.saveObject.SaveData.gameDate.year - DatetimeManager.Instance.InitYear == 0)
        {
            gaoKaoObj.SetActive(false);
        }
        else
        {
            basicList[6].text = "";
            foreach (Grade grade in studentUnit.NCEESelect) basicList[6].text += grade.name;

            gaoKaoObj.SetActive(true);
        }
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    private void SetProperty()
    {
        for (var i = 0; i < properties.Count; i++) properties[i].text = studentUnit.properties[i].score.ToString();

        goodAndEvilPointer.anchoredPosition =
            new Vector2(studentUnit.properties.Find(x => x.gradeID == "goodAndEvil").score / 2f, 0);
    }

    /// <summary>
    /// 设置主要学科分数
    /// </summary>
    private void SetMainScore()
    {
        for (var i = 0; i < studentUnit.mainGrade.Count; i++) mainScores[i].UIUpdate(studentUnit.mainGrade[i]);
    }

    /// <summary>
    /// 设置兴趣学科分数
    /// </summary>
    private void SetInterestGrade()
    {
        var isNotInterested = false;
        foreach (Grade grade in studentUnit.interestGrade)
        {
            ScoreEntryControl entryControl = interestGrades.Find(x => x.grade.gradeID == grade.gradeID);
            if (entryControl == null)
            {
                entryControl = Instantiate(scorePrefab, scoreParent).GetComponent<ScoreEntryControl>();
                interestGrades.Add(entryControl);
            }

            var b = entryControl.UIUpdate(grade);
            if (b) isNotInterested = true;
        }

        //设置无兴趣提示的显示
        //Debug.Log(!isNotInterested);
        NotInterested.SetActive(!isNotInterested);
    }

    /// <summary>
    /// 设置好友列表
    /// </summary>
    private void SetFriendList()
    {
        foreach (FriendEntryControl control in friendControls) control.gameObject.SetActive(false);

        while (friendControls.Count < studentUnit.interpersonalRelationship.Count)
        {
            var control = Instantiate(friendPrefab, friendListParent).GetComponent<FriendEntryControl>();
            friendControls.Add(control);
        }

        for (var i = 0; i < studentUnit.interpersonalRelationship.Count; i++)
            friendControls[i].UpdateUI(studentUnit.interpersonalRelationship[i]);

        //设置无朋友的提示
        //Debug.Log(friendListParent.childCount);
        if (friendControls.Count == 0)
            nptFriends.SetActive(true);
        else
            nptFriends.SetActive(friendControls.All(control => !control.gameObject.activeSelf));
    }

    public void OnExit()
    {
        GetComponent<Animator>().Play("StudentPropertiesExitPanel");
    }

    public void Exit()
    {
        portrayalParent.anchoredPosition = new Vector2(portrayalParent.anchoredPosition.x, 0);
        GetComponentInChildren<CanvasGroup>().alpha = 1;
        gameObject.SetActive(false);
    }
}