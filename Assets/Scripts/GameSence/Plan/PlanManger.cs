using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlanManger : MonoInstance<PlanManger>
{
    /// <summary>
    /// 技能的初始数据
    /// </summary>
    private PlayerCourseList playerCourseList;
    /// <summary>
    /// 工作的初始数据
    /// </summary>
    private PlayerWorkList playerWorkList;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform playerLearnParent;
    [SerializeField] private Transform playerWorkParent;
    [SerializeField] private GameObject playerLearnGameObject;
    [SerializeField] private GameObject playerWorkGameObject;
    [SerializeField] private Text remainingDaysText;
    [SerializeField] private Text usedDaysText;
    [SerializeField] private AudioSource audioSource;
    public event UnityAction UIUpdateEvent;
    public event UnityAction ClearPlanNumberEvent;

    public string[] playerPlan;

    /// <summary>
    /// 剩余可分配的天数
    /// </summary>
    public int RemainingDays
    {
        get
        {
            int day=0;
            foreach (string i in playerPlan)
            {
                if (i=="0")
                {
                    day++;
                }
            }

            return day;
        }
    }

    private PlayerCourseLevelList playerCourseLevelList;
    /// <summary>
    /// 玩家所有学习条目，包括已解锁和未解锁
    /// </summary>
    private List<GameObject> playerLearnGameObjects;
    /// <summary>
    /// 玩家所有工作条目
    /// </summary>
    private List<GameObject> playerWorkGameObjects;
    /// <summary>
    /// 玩家当前所拥有的技能
    /// </summary>
    private List<PlayerCourse> playerCourses;

    public override void Awake()
    {
        base.Awake();
        gameManager.InitGameEvent += Init;
    }

    private void Init()
    {
        playerPlan = gameManager.saveObject.SaveData.playerPlan;
        playerCourses = gameManager.saveObject.SaveData.playerUnit.playerCourses;
        playerCourseList = gameManager.PlayerCourseList;
        playerCourseLevelList = gameManager.PlayerCourseLevelList;
        playerWorkList = gameManager.PlayerWorkList;
        playerLearnGameObjects = new List<GameObject>();
        playerWorkGameObjects = new List<GameObject>();
        //初始化、生成教研词条
        for (int i = 0; i < playerCourseList.Count(); i++)
        {
            string id = playerCourseList[i].Id;
            GameObject obj = Instantiate(playerLearnGameObject, playerLearnParent);
            playerLearnGameObjects.Add(obj);
            PlayerCourseList.Row row = playerCourseList[i];
            PlayerCourse playerCourse = playerCourses.Find(x => x.id == id);
            if (playerCourse==null)// 如果存档中没有指定ID的教研，就添加一个，并且设置为未拥有
            {
                playerCourse = new PlayerCourse
                {
                    empiricalValue = 0,
                    id = playerCourseList[i].Id,
                    level = 0,
                    name = playerCourseList[i].Name,
                    isHave = false
                };
                playerCourses.Add(playerCourse);
            }
            obj.GetComponent<PlayerLearnControl>().Init(row, playerCourse , playerCourseLevelList);
        }
        //初始化、生成工作词条
        for (int i = 0; i < playerWorkList.Count(); i++)
        {
            string id = playerWorkList[i].id;
            GameObject obj = Instantiate(playerWorkGameObject, playerWorkParent);
            playerWorkGameObjects.Add(obj);
            PlayerWorkList.Row row = playerWorkList[i];
            PlayerCourse playerCourse =playerCourses.Find(x => x.id == id);
            if (playerCourse==null)// 如果存档中没有指定ID的打工，就直接添加一个
            {
                playerCourse = new PlayerCourse
                {
                    empiricalValue = 0,
                    id = playerWorkList[i].id,
                    level = 0,
                    name = playerWorkList[i].name,
                    isHave = true
                };
                playerCourses.Add(playerCourse);
            }
            obj.GetComponent<PlayerWorkControl>().Init(row,playerCourse);
        }
        UIUpdateEvent += UpdateUI;
        UIUpdateEvent?.Invoke();
    }

    void UpdateUI()
    {
        remainingDaysText.text = RemainingDays.ToString();
        usedDaysText.text = (5 - RemainingDays).ToString();
    }
    public void OnAdd(string id)
    {
        for (int index = 0; index < playerPlan.Length; index++)
        {
            if (playerPlan[index] == "0")
            {
                playerPlan[index] = id;
                audioSource.Play();
                UIUpdateEvent?.Invoke();
                return;
            }
        }

        Debug.Log("不能分配");
    }

    public void OnSub(string id)
    {
        for (int index = 0; index < playerPlan.Length; index++)
        {
            if (playerPlan[index] == id)
            {
                playerPlan[index] = "0";
                audioSource.Play();
                UIUpdateEvent?.Invoke();
                return;
            }
        }
        Debug.Log("不能分配");
    }

    /// <summary>
    /// 清空分配的计划点数
    /// </summary>
    public void ClearPlanNumber()
    {
        for (int i = 0; i < playerPlan.Length; i++)
        {
            playerPlan[i] = "0";
        }

        ClearPlanNumberEvent?.Invoke();
        UIUpdateEvent?.Invoke();
    }

    public void UIUpdateAll()
    {
        UIUpdateEvent?.Invoke();
    }
}