using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameOverControl : MonoBehaviour
{
    [ContextMenuItem("end","End")]
    [SerializeField] private GameManager gameManager;
    [Header("高考结果")] [SerializeField] private Text studentName;
    [SerializeField] private Text schoolName;
    [SerializeField] private Text schoolDescription;
    [SerializeField] private Text interestDescription;
    [Header("职业状态")] [SerializeField] private Text longName;

    [SerializeField] private Text zhiYe;
    [SerializeField] private Text zhuangTai;
    [SerializeField] private Text longOverDescription;
    [SerializeField] private Text levelText;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject conclusionPanel;
    [SerializeField] private GameObject studentOver;
    [SerializeField] private GameObject longTimePanel;
    private SaveData saveData;
    private GameOverList gameOverList;
    private EnterHigherSchoolList enterHigherSchoolList;
    private SchoolWorkList schoolWorkList;
    private int studentNumber = 0;
    private int longStudentNumber = 0;
    private static readonly int Load = Animator.StringToHash("Load");
    private static readonly int Next = Animator.StringToHash("Next");
    private static readonly int LongTime = Animator.StringToHash("LongTime");
    private static readonly int NextLong = Animator.StringToHash("NextLong");

    void OnEnable()
    {
        gameOverList ??= gameManager.GameOverList;
        enterHigherSchoolList ??= gameManager.EnterHigherSchoolList;
        schoolWorkList ??= gameManager.SchoolWorkList;
        saveData ??= gameManager.saveObject.SaveData;
        //LoadStudent(saveData.studentUnits[0]);
        gameManager.bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType.GameOver);
    }

    private void LoadStudent(StudentUnit unit)
    {
        studentName.text = unit.fullName;
        //总成绩
        int totalPoints = 0;
        for (int i = 0; i < 3; i++) //三大主科
        {
            totalPoints += unit.mainGrade[i].GradeScore;
        }

        foreach (var grade in unit.mainGrade) //三大选科
        {
            if (unit.NceeSelect.Any(gr => grade.gradeID == gr.gradeID))
            {
                totalPoints += grade.GradeScore;
            }
        }


        //Debug.Log(totalPoints);
        List<EnterHigherSchoolList.Row> schoolRowList = enterHigherSchoolList.GetRowList().FindAll(row =>
        {
            if (int.TryParse(row.TotalPoints, out int thisTotalPoints))
            {
                return thisTotalPoints < totalPoints;
            }

            return false;
        });
        EnterHigherSchoolList.Row enterHigherSchoolRow = null;
        foreach (var row in schoolRowList)
        {
            //判断该学生是否可以使用这个结局
            string[] ids = row.Student.Split(',');
            bool isThisStudentOver = false;
            foreach (var str in ids)
            {
                if (str == unit.id)
                {
                    isThisStudentOver = true;
                }
            }

            if (row.Student != "all" && !isThisStudentOver) continue;
            //判断是不是高分区间
            if (enterHigherSchoolRow==null)
            {
                enterHigherSchoolRow = row;
            }else if (int.Parse(enterHigherSchoolRow.TotalPoints) < int.Parse(row.TotalPoints))
            {
                enterHigherSchoolRow = row;
            }
        } //得到正确的schoolRow

        schoolName.text = enterHigherSchoolRow.SchoolName;
        schoolDescription.text = enterHigherSchoolRow.Description;
        schoolDescription.text = schoolDescription.text.Replace("name", unit.fullName);

        Grade maxGrade = unit.interestGrade[0]; //找出分最高的一个兴趣
        foreach (Grade grade in unit.interestGrade.Where(grade => grade.score > maxGrade.score)) maxGrade = grade;

        List<SchoolWorkList.Row> schoolWorkRows = schoolWorkList.FindAll_TotalPoints(enterHigherSchoolRow.TotalPoints);
        SchoolWorkList.Row schoolWorkRow = null; //学生在大学内的兴趣培养情况
        foreach (var row in schoolWorkRows)
        {
            if (maxGrade.score <= int.Parse(row.LeastValue)) continue; //学生的兴趣等级不够
            if (schoolWorkRow == null) schoolWorkRow = row;
            else if (int.Parse(row.LeastValue) > int.Parse(schoolWorkRow.LeastValue)) //挑一个最大值
                schoolWorkRow = row;
        }

        if (schoolWorkRow != null)
            interestDescription.text = schoolWorkRow.Description;
        else
            interestDescription.text = "name的兴趣爱好有点不太对劲";
        interestDescription.text = interestDescription.text.Replace("name", unit.fullName);
        interestDescription.text = interestDescription.text.Replace("interest", maxGrade.name);
    }

    /// <summary>
    /// 点击第一个继续按钮，开始显示学生升学情况
    /// </summary>
    public void OnInitButton()
    {
        conclusionPanel.SetActive(false);
        studentOver.SetActive(true);
        animator.SetBool(Load, true);
    }

    public void OnLoadButton()
    {
        if (++studentNumber < saveData.studentUnits.Count)
            animator.SetTrigger(Next);
        else
            animator.SetTrigger(LongTime);
    }

    public void OnLoadLongButton()
    {
        if (++longStudentNumber < saveData.studentUnits.Count)
            animator.SetTrigger(NextLong);
        else
            animator.SetTrigger(Next);
    }

    /// <summary>
    /// 动画开始事件，以刷新UI
    /// </summary>
    public void LoadNextStudent()
    {
        LoadStudent(saveData.studentUnits[studentNumber]);
    }

    public void LoadLongOverNextStudent()
    {
        LoadLongTimeStudent(saveData.studentUnits[longStudentNumber]);
    }

    /// <summary>
    /// 装在一个学生的职业状况
    /// </summary>
    /// <param name="unit"></param>
    private void LoadLongTimeStudent(StudentUnit unit)
    {
        longName.text = unit.fullName;
        List<GameOverList.Row> finds = new List<GameOverList.Row>();
        foreach (var row in gameOverList.GetRowList())
        {
            if (row.EndID == "") continue;
            bool isEligibility = false;

            //判断该学生是否可以使用这个结局
            //判断学生ID要求
            string[] ids = row.StudentID.Split(',');
            if (ids.Any(str => str == unit.id))
            {
                isEligibility = true;
            }

            if (row.StudentID != "all" && !isEligibility) continue;

            //基本属性要求
            isEligibility = true;
            for (int i = 0; i < unit.properties.Count; i++)
            {
                if (unit.properties[i].name == "善恶") continue;//去除善恶
                if (unit.properties[i].score<row.Properties[i] ) isEligibility = false;//学生属性不够就会continue
            }
            switch (row.GESymbol)
            {
                case "<":
                    if (unit.GoodAndEvil.score > (int.TryParse(row.善恶, out int value1) ? value1 : 0)) isEligibility = false;
                    break;
                case ">":
                    if (unit.GoodAndEvil.score < (int.TryParse(row.善恶, out int value2) ? value2 : 0)) isEligibility = false;
                    break;
            }
            if (!isEligibility) continue;
            //主学科要求
            if (unit.mainGrade.Where((t, i) => t.score < row.MainGrade[i]).Any()) continue;
            //兴趣要求
            if (unit.interestGrade.Where((t, i) => t.score < row.InterestGrade[i]).Any()) continue;
            finds.Add(row);
        }

        List<GameOverList.Row> rows0 = new List<GameOverList.Row>();
        List<GameOverList.Row> rows1 = new List<GameOverList.Row>();
        List<GameOverList.Row> rows2 = new List<GameOverList.Row>();
        List<GameOverList.Row> rows3 = new List<GameOverList.Row>();
        List<GameOverList.Row> rows4 = new List<GameOverList.Row>();
        List<GameOverList.Row> rows5 = new List<GameOverList.Row>();
        foreach (var row in finds)
        {
            switch (int.Parse(row.Level))
            {
                case 0:
                    rows0.Add(row);
                    break;
                case 1:
                    rows1.Add(row);
                    break;
                case 2:
                    rows2.Add(row);
                    break;
                case 3:
                    rows3.Add(row);
                    break;
                case 4:
                    rows4.Add(row);
                    break;
                case 5:
                    rows5.Add(row);
                    break;
            }
        }

        GameOverList.Row target = finds[finds.Count-1];
        if (rows5.Any())
        {
            target = rows5[Random.Range(0, rows5.Count)];
            zhuangTai.text = $"{unit.fullName}拥有着很高的社会地位，生活非常美满，成为了极富裕人士";
            levelText.text = "S";
        }else if (rows4.Any())
        {
            target = rows4[Random.Range(0, rows4.Count)];
            zhuangTai.text = $"{unit.fullName}拥有着很高的社会地位，生活非常美满，顺利成为了富裕人士";
            levelText.text = "A";

        }else if (rows3.Any())
        {
            target = rows3[Random.Range(0, rows3.Count)];
            zhuangTai.text = $"{unit.fullName}非常满意现在的生活，时常感慨自己曾经所做的付出,现已加入高薪行列";
            levelText.text = "B+";

        }else if (rows2.Any())
        {
            target = rows2[Random.Range(0, rows2.Count)];
            zhuangTai.text = $"{unit.fullName}与大多数人一样，普普通通，家庭收入始终在拖国家后腿。是组成社会大家庭的中坚力量";
            levelText.text = "B";

        }else if (rows1.Any())
        {
            target = rows1[Random.Range(0, rows1.Count)];
            zhuangTai.text = $"{unit.fullName}与别人不一样，{unit.GetGenderString(true)}陷入了自我贫困，对曾经的全托班老师十分憎恨";
            levelText.text = "C";

        }else if (rows0.Any())
        {
            target = rows0[Random.Range(0, rows0.Count)];
            zhuangTai.text = $"{unit.fullName}在一次意外中没有坚持下去，昏迷了过去。";
            levelText.text = "X";

        }
        zhiYe.text = target.EndName;
        longOverDescription.text = target.Description.Replace("name",unit.fullName);
        longOverDescription.text = longOverDescription.text.Replace("TA",unit.GetGenderString());
        if (gameManager.saveObject.OverSaveData.overUnits.All(un=>un.overId!=target.EndID))
        {
            gameManager.saveObject.OverSaveData.overUnits.Add(new OverUnit(unit.fullName,saveData.playerUnit.fullName,DateTime.Now,target.EndID ));
        }
    }

    public void LongTimeEvent()
    {
        studentOver.SetActive(false);
        conclusionPanel.SetActive(false);
    }

    public void End()
    {
        SaveManager.SaveOverGame(gameManager.saveObject.OverSaveData);
        SaveManager.ResetSaveFile(0);
        SceneManager.LoadScene(0);
    }
}