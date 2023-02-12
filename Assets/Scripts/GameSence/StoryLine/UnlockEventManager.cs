using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 随时间解锁的事件
/// </summary>
public class UnlockEventManager : MonoInstance<UnlockEventManager>
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DatetimeManager datetimeManager;
    [SerializeField] private StoryLineManager storyLineManager;
    [SerializeField] private GameObject goToWorldGameObject;
    [SerializeField] private GameObject divideIntoClassesButton;
    [SerializeField] private GameObject overPanel;
    [SerializeField] private DivideIntoClassesControl divideIntoClassesControl;
    [Header("打印成绩单")] [SerializeField] private GameObject cjdButton;

    private SaveData saveData;
    private int firstMood40 = 0;
    private int firstMood20 = 0;

    public override void Awake()
    {
        datetimeManager.AddDateEvent += GoToWorld;
        datetimeManager.AddDateEvent += Mood20;
        datetimeManager.AddDateEvent += Mood40;
        datetimeManager.AddDateEvent += TermEnd;
        datetimeManager.AddDateEvent += Midterm;
        datetimeManager.AddDateEvent += AddPlayerCourse;
        datetimeManager.AddDateEvent += OverPanelEvent;
        datetimeManager.AddDateEvent += DivideIntoClasses;
    }

    private void OnEnable()
    {
        saveData = gameManager.saveObject.SaveData;
    }

    /// <summary>
    /// 解锁去世界功能
    /// </summary>
    private void GoToWorld()
    {
        if (!saveData.isWorld)
        {
            if (datetimeManager.DateTime > new Date(datetimeManager.InitYear, 0, 2, 0))
            {
                saveData.isWorld = true;
                goToWorldGameObject.SetActive(true);
                HintManager.Instance.AddHint(new Hint("新解锁", "外出功能已解锁"));
            }
        }
        else
        {
            goToWorldGameObject.SetActive(true);
        }
    }

    private void Mood40()
    {
        bool any = GameManager.Instance.saveObject.SaveData.studentUnits.Any(x => x.Mood <= 40 && x.Mood > 20);
        if (any)
        {
            if (firstMood40 == 0)
            {
                HintManager.Instance.AddHint(new Hint("心情过低", "心情小于40，学习效率降低TAT。请注意让学生休息哟~"));
            }

            firstMood40 += 1;
            firstMood40 %= 3;
        }
    }

    private void Mood20()
    {
        bool any = GameManager.Instance.saveObject.SaveData.studentUnits.Any(x => x.Mood <= 20);
        if (any)
        {
            if (firstMood20 == 0)
            {
                HintManager.Instance.AddHint(new Hint("心情过低", "心情小于20，学习效率大幅度降低TAT。请注意让学生休息哟~/n心情可以让学生去“睡觉”快速恢复"));
            }

            firstMood20 += 1;
            firstMood20 %= 2;
        }
    }

    /// <summary>
    /// 期中考试
    /// </summary>
    private void Midterm()
    {
        if (saveData.gameDate.Week == saveData.midtermDate.Week || saveData.gameDate.WhatDay != 2)
        {
            return;
        }

        if (saveData.gameDate.Week == Date.MaxWeek || saveData.gameDate.Week == Date.MaxWeek / 2) //进入期中/期末考试
        {
            saveData.midtermDate.Week = saveData.gameDate.Week; //保存考试的周数

            if (saveData.gameDate.year - saveData.InitYear == 2 && saveData.gameDate.Week == Date.MaxWeek && saveData.gameDate.Semester == 1) //高考
            {
                HintManager.Instance.AddHint(new Hint("高考进行中", "您的学生正在参加高考，结局达成！"));
            }
            else
            {
                HintManager.Instance.AddHint(new Hint(saveData.gameDate.Week == Date.MaxWeek ? "期末考试" : "期中考试", "今日学校组织进行考试。学生们主学科的分数获得提升。分数越高，提升越多。"));

                float efficiency = saveData.midtermEfficiency;
                foreach (var unit in saveData.studentUnits)
                {
                    string text = "";
                    foreach (var grade in unit.mainGrade)
                    {
                        text += $"{grade.name}提升{(int)(grade.score * efficiency)}\r\n";
                        grade.score += (int)(efficiency * grade.score);
                    }

                    HintManager.Instance.AddHint(new Hint(unit.fullName + "-考试提升", text));
                }
            }
        }
    }

    /// <summary>
    /// 期末成绩单
    /// </summary>
    private void TermEnd()
    {
        if (saveData.gameDate.Week == 8 && saveData.gameDate.WhatDay == 5 || saveData.isTranscript == true)
        {
            HintManager.Instance.AddHint(new Hint("成绩单", "<color=red>期末考试成绩单已接收，请查阅并编写评语</color>"));
            saveData.isTranscript = true;
            cjdButton.SetActive(true);
        }
    }

    /// <summary>
    /// 根据配置表条件，添加玩家技能
    /// </summary>
    private void AddPlayerCourse()
    {
        if (saveData.playerUnit.playerCourses.All(x => x.id != "1"))
        {
            if (saveData.studentUnits.Any(y => y.mainGrade.Find(x => x.gradeID == "0").score >= 380))
            {
                saveData.playerUnit.playerCourses.Add(new PlayerCourse() { empiricalValue = 0, id = "1", isHave = true, level = 0, name = "文学进修" });
                HintManager.Instance.AddHint(new Hint("教研技能解锁", "文学进修已解锁，积极参与教研可以大幅增加教学效果"));
            }
        }

        if (saveData.playerUnit.playerCourses.All(x => x.id != "20"))
        {
            if (saveData.studentUnits.Any(y => y.interestGrade.Find(x => x.gradeID == "21").score >= 150))
            {
                saveData.playerUnit.playerCourses.Add(new PlayerCourse() { empiricalValue = 0, id = "20", isHave = true, level = 0, name = "蝶泳特训" });
                HintManager.Instance.AddHint(new Hint("教研技能解锁", "蝶泳特训已解锁，积极参与教研可以大幅增加教学效果"));
            }
        }

        if (saveData.playerUnit.playerCourses.All(x => x.id != "21"))
        {
            if (saveData.studentUnits.Any(y => y.mainGrade.Find(x => x.gradeID == "1").score >= 380))
            {
                saveData.playerUnit.playerCourses.Add(new PlayerCourse() { empiricalValue = 0, id = "21", isHave = true, level = 0, name = "奥数进阶" });
                HintManager.Instance.AddHint(new Hint("教研技能解锁", "奥数进阶已解锁，积极参与教研可以大幅增加教学效果"));
            }
        }
    }

    /// <summary>
    /// 结局了
    /// </summary>
    private void OverPanelEvent()
    {
        if (saveData.gameDate.year == saveData.initYear + 2 && saveData.gameDate.Week == 8 && saveData.gameDate.WhatDay == 3 && saveData.gameDate.Semester == 1)
        {
            storyLineManager.isEnd = true;
            overPanel.SetActive(true);
        }
    }

    /// <summary>
    /// 分班按钮的显示与否
    /// </summary>
    private void DivideIntoClasses()
    {
        if (saveData.isDivideIntoClass)
        {
            divideIntoClassesButton.SetActive(true);
        }

        if (saveData.gameDate.Semester != 0)
        {
            return;
        }

        if (divideIntoClassesButton.activeSelf) //如果是这个功能是激活的
        {
            if (saveData.gameDate.Week == DivideIntoClassesControl.DivideIntoClassesWeek && saveData.gameDate.WhatDay == 3)
            {
                if (!divideIntoClassesControl.isDivideIntoClasses)
                {
                    HintManager.Instance.AddHint(new Hint("大意了", "粗心的老师没有给学生进行分班预选，现在的预选可能很糟糕"));
                }
                divideIntoClassesControl.DivideIntoClasses();
                divideIntoClassesButton.SetActive(false);
                saveData.isDivideIntoClass = false;
                divideIntoClassesControl.gameObject.SetActive(false);
                HintManager.Instance.AddHint(new Hint("分班完成", "学生们在学校内完成了高考分班的选择，现在可以在学生属性中查看分班情况了。"));
            }
        }
        else
        {
            if (saveData.gameDate.year - saveData.InitYear == 1 && saveData.gameDate.Week == 1)
            {
                divideIntoClassesButton.SetActive(true);
                saveData.isDivideIntoClass = true;
            }
        }
    }
}