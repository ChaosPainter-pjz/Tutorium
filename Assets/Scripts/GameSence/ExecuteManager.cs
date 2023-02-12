using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 执行功能管理器
/// </summary>
public class ExecuteManager : MonoInstance<ExecuteManager>
{
    [SerializeField] private GameManager gameManager;
    private StoryLineManager storyLineManager;
    /// <summary>
    /// 学生属性更新面板的控制器
    /// </summary>
    [SerializeField] private ExecuteControl executeControl;

    /// <summary>
    /// 学生属性变化词条
    /// </summary>
    [SerializeField] private GameObject propertyUpdateGlossary;
    [SerializeField] private GameObject classIsOverScene;
    [SerializeField] private Image classIsOverImage;

    /// <summary>
    /// 工作日过场面板
    /// </summary>
    [Header("过场面板")] [SerializeField] private DayAnimationControl dayAnimationControl;
    [SerializeField] private GameObject schoolIsOverScene;
    [Header("工作日结算面板")] [SerializeField] private PlayerPropertyChangeControl playerPropertyChangeControl; //面板本体
    [Header("日期动画")] [SerializeField]private Animator dateAnimator;
    [Header("成绩单面板")] [SerializeField]private TimerStreamControl timeStream;
    private List<StudentUnit> studentUnits;
    private PlayerCourseLevelList playerCourseLevelList;
    private PlayerWorkList playerWorkList;
    private StudentWeeklyPropertyList studentWeeklyPropertyList;
    private Date date;
    private string[] playerPlan;
    private List<PlayerCourse> playerCourses;

    public override void Awake()
    {
        base.Awake();
        gameManager.InitGameEvent += Init;
    }

    private void Init()
    {
        storyLineManager = StoryLineManager.Instance;
        studentUnits = gameManager.saveObject.SaveData.studentUnits;
        date = gameManager.saveObject.SaveData.gameDate;
        playerCourseLevelList = gameManager.PlayerCourseLevelList;
        playerWorkList = gameManager.PlayerWorkList;
        playerPlan = gameManager.saveObject.SaveData.playerPlan;
        playerCourses = gameManager.saveObject.SaveData.playerUnit.playerCourses;
        studentWeeklyPropertyList = gameManager.StudentWeeklyPropertyList;
        executeControl.Init();
        playerPropertyChangeControl.Init();
    }

    /// <summary>
    /// 单击执行按钮
    /// </summary>
    public void OnClick()
    {
        if (date.WhatDay == 5) //今天是周五
        {
            SaveManager.SaveGame(gameManager.saveObject.SaveData,0);
            // SaveSchedule();
            //开始放动画片
            StopAllCoroutines();
            StartCoroutine(OffDayAnimator());
            //刷新世界小游戏
            WorldGameManager.Instance.UpdateWorldGame();
        }
        else if (date.WhatDay == 0) //今天是周日
        {
            SaveManager.SaveGame(gameManager.saveObject.SaveData,0);
            StopAllCoroutines();
            StartCoroutine(WorkDayAnimator());
            PlayerAttributeUpdate();
            StudentsIndicateUpdate();
            StudentWorkUpdate();
            AddStudentTrust();
        }
    }

    // //保存安排
    // private void SaveSchedule()
    // {
    //     //记录学生的安排
    //     foreach (var studentUnit in studentUnits)
    //     {
    //         for (int i = 0; i < studentUnit.lastWeekSchedule.Length; i++)
    //         {
    //             studentUnit.lastWeekSchedule[i].id = studentUnit.schedule[i].id;
    //         }
    //     }
    // }

    // private IEnumerator OffDayAnimator()
    // {
    //     offDayPanel.SetActive(true);
    //     yield return new WaitForSeconds(1);
    //     DatetimeManager.Instance.DateTimeUpdate(1);
    //     if (storyLineManager.JudgmentEnforceabilityRow())
    //     {
    //         offDayPanel.SetActive(false);
    //         storyLineManager.BeganPlot();
    //         //需要不停获取某个标志，直到标志变化，才能继续日期变化
    //         while (storyLineManager.isPlot)
    //         {
    //             yield return null;
    //         }
    //
    //         offDayPanel.SetActive(true);
    //     }
    //
    //     yield return new WaitForSeconds(1);
    //     DatetimeManager.Instance.DateTimeUpdate(1);
    //
    //     offDayPanel.SetActive(false);
    //
    //     //刷新学生属性
    //     StudentsAttributeUpdate();
    // }
    private IEnumerator OffDayAnimator()
    {
        //显示休息日过场面板
        dayAnimationControl.gameObject.SetActive(true);
        bool isAcrossSemester = false;
        while (date.WhatDay != 0)
        {
            yield return new WaitForSeconds(0.5f);
            DatetimeManager.Instance.DateTimeUpdate(1);
            yield return new WaitForSeconds(0.5f);
            //这里判断期末成绩单
            if (date.Week==1&&date.WhatDay==0)
            {
                timeStream.gameObject.SetActive(true);
                isAcrossSemester = true;
                //关闭过场面板
                dayAnimationControl.gameObject.SetActive(false);
                continue;
            }
            if (date.WhatDay != 0)//周日的剧情结算后触发，所以这里不能是周日
            {
                if (storyLineManager.JudgmentEnforceabilityRow())
                {
                    dayAnimationControl.gameObject.SetActive(false);
                    storyLineManager.BeganPlot();
                    while (storyLineManager.isPlot)
                    {
                        yield return null;
                    }

                    dayAnimationControl.gameObject.SetActive(true);
                }
            }
        }

        if (isAcrossSemester) yield break;//如果这里跨学期了，就不进行学生属性变化实现和放学页面播放
        //播放放学画面
        //设置过场画面的图片
        {
            if (studentUnits.Count <= 2)
            {
                foreach (var sprite in ResourceManager.Instance.interludes)
                {
                    if (sprite.name == "2")
                    {
                        classIsOverImage.sprite = sprite;
                    }
                }
            }
            else
            {
                if (date.Semester == 0)
                {
                    foreach (var sprite in ResourceManager.Instance.interludes)
                    {
                        if (sprite.name == "3")
                        {
                            classIsOverImage.sprite = sprite;
                        }
                    }

                    //classIsOverImage.sprite = ResourceManager.Instance.interludes[1];
                }
                else
                {
                    foreach (var sprite in ResourceManager.Instance.interludes)
                    {
                        if (sprite.name == "4")
                        {
                            classIsOverImage.sprite = sprite;
                        }
                    }

                    //classIsOverImage.sprite = ResourceManager.Instance.interludes[2];
                }
            }
        }
        classIsOverScene.SetActive(true);
        yield return new WaitForSeconds(3f);
        classIsOverScene.SetActive(false);
        //关闭工作日过场面板
        dayAnimationControl.gameObject.SetActive(false);
        //刷新学生属性
        StudentsAttributeUpdate();
    }

    private IEnumerator WorkDayAnimator()
    {
        //显示工作日过场面板
        dayAnimationControl.gameObject.SetActive(true);

        while (date.WhatDay != 5)
        {
            yield return new WaitForSeconds(0.5f);
            DatetimeManager.Instance.DateTimeUpdate(1);
            yield return new WaitForSeconds(0.5f);
            if (storyLineManager.isEnd)
            {
                yield break;
            }
            if (date.WhatDay != 5)//周五的剧情结算后触发，所以这里不能是周五
            {
                if (storyLineManager.JudgmentEnforceabilityRow())
                {
                    dayAnimationControl.gameObject.SetActive(false);
                    storyLineManager.BeganPlot();
                    while (storyLineManager.isPlot)
                    {
                        yield return null;
                    }

                    dayAnimationControl.gameObject.SetActive(true);
                }
            }
        }

        //播放放学画面
        schoolIsOverScene.SetActive(true);
        yield return new WaitForSeconds(3f);
        schoolIsOverScene.SetActive(false);
        //关闭工作日过场面板
        dayAnimationControl.gameObject.SetActive(false);

        //显示结算面板
        playerPropertyChangeControl.gameObject.SetActive(true);
    }

    /// <summary>
    /// 学生属性更新
    /// </summary>
    private void StudentsAttributeUpdate()
    {
        if (studentUnits.Count > 0)
        {
            executeControl.gameObject.SetActive(true);
            executeControl.GetComponent<Animator>().SetTrigger("Trigger");
            executeControl.LoadStudent(studentUnits[0]);
        }
        else
        {
            Debug.LogError("没有学生");
        }
    }

    /// <summary>
    /// 根据学生分配的指示点来加属性
    /// </summary>
    private void StudentsIndicateUpdate()
    {
        foreach (var studentUnit in studentUnits)
        {
            foreach (var grade in studentUnit.indicatingNow)
            {
                grade.score += 20;
            }

            if (studentUnit.indicatingPoints-studentUnit.indicatingNow.Count>0)
            {
                studentUnit.Mood += 10*(studentUnit.indicatingPoints-studentUnit.indicatingNow.Count);
            }
        }
    }
    /// <summary>
    /// 每周随机增加属性
    /// </summary>
    private void StudentWorkUpdate()
    {
        foreach (var studentUnit in studentUnits)
        {
            List<StudentWeeklyPropertyList.Row> findAllID = studentWeeklyPropertyList.FindAll_id(studentUnit.id);
            if (findAllID.Count==0)
            {
                Debug.Log(studentUnit.fullName+"没有每周成长配置");
                return;
            }
            StudentWeeklyPropertyList.Row row = findAllID[Random.Range(0, findAllID.Count)];
            string[] strings = {row.语文, row.数学, row.英语, row.政治, row.历史, row.地理, row.物理, row.化学, row.生物};
            int[] ints= Array.ConvertAll(strings , int.Parse);
            for (int i = 0; i < studentUnit.mainGrade.Count; i++)
            {
                studentUnit.mainGrade[i].score += ints[i];
            }
        }
    }
    /// <summary>
    /// 每周增加信任
    /// </summary>
    private void AddStudentTrust()
    {
        foreach (var studentUnit in studentUnits)
        {
            if (studentUnit.Mood>=80)
            {
                studentUnit.Trust++;

            }
        }
    }


    /// <summary>
    /// 教师技能更新
    /// </summary>
    private void PlayerAttributeUpdate()
    {
        //刷新技能等级与金钱
        playerPropertyChangeControl.moneyControl.LowMoneyNumber = MoneyManager.Instance.Money; //传递旧金钱数
        playerPropertyChangeControl.PlayerInitPanel();
        //遍历玩家的计划，变更教研、打工技能等级
        for (int i = 0; i < playerPlan.Length; i++)
        {
            //没有计划则跳过这次循环
            if (playerPlan[i] == "0") continue;
            PlayerCourse course = playerCourses.Find(x => x.id == playerPlan[i]);
            var coursesAllId = playerCourseLevelList.FindAll_id(playerPlan[i]);
            if (coursesAllId != null && coursesAllId.Count > 0) //现在确认id是教研表中的
            {
                int nextLevelMaxXP = -1;
                int maxLevel = 0;
                foreach (PlayerCourseLevelList.Row row in coursesAllId) //找到当前等级对应升级所需经验，最高等级
                {
                    if (int.Parse(row.等级) == course.level)
                    {
                        nextLevelMaxXP = int.Parse(row.下一级所需经验);
                    }

                    if (maxLevel < int.Parse(row.等级))
                    {
                        maxLevel = int.Parse(row.等级);
                    }
                }

                if (nextLevelMaxXP == -1) Debug.Log("未找到对应等级的最大经验值 " + course.name);
                //判断是否可以升级了
                if (course.level < maxLevel) //没有达到最大等级
                {
                    //先加经验
                    course.empiricalValue++;
                    if (course.empiricalValue >= nextLevelMaxXP)
                    {
                        //升级
                        course.empiricalValue = 0;
                        course.level++;
                    }
                }

                if (course.level >= maxLevel) //此时已经到达最大等级
                {
                    playerPlan[i] = "0";
                    // string target = id;
                    // for (int j = 0; j < playerPlan.Length; j++)
                    // {
                    //     if (playerPlan[j] == target)
                    //     {
                    //         playerPlan[j] = "0";
                    //     }
                    // }
                }
            }

            var workId = playerWorkList.Find_id(playerPlan[i]);
            if (workId != null) //现在确认id是打工表中的
            {
                int nextLevelMaxXP = int.Parse(workId.UpExperience);
                int maxLevel = int.Parse(workId.maxLevel);

                //判断是否可以升级了
                if (course.level < maxLevel) //没有达到最大等级
                {
                    course.empiricalValue++; //加经验
                    if (course.empiricalValue >= nextLevelMaxXP)
                    {
                        //升级
                        course.empiricalValue = 0;
                        course.level++;
                    }
                }

                /*因为还可以继续打工，所以注释掉不可选中
                if(course.level >= maxLevel)//此时已经到达最大等级
                {

                    string target = id;
                    for (int i = 0; i < playerPlan.Length; i++)
                    {
                        if (playerPlan[i]==target)
                        {
                            playerPlan[i] = "0";
                        }
                    }
                }
                */
                int initialYield = int.Parse(workId.InitialYield);
                int levelYield = int.Parse(workId.levelYield);
                MoneyManager.Instance.Money += initialYield + course.level * levelYield;
            }
        }

        playerPropertyChangeControl.PlayerUpdatePanel();
        playerPropertyChangeControl.moneyControl.SetMoneyPanel(MoneyManager.Instance.Money); //传递新的金钱数,更新金钱词条的显示与否、位置
    }
}