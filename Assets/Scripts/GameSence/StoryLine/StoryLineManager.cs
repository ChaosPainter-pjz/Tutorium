using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 剧情管理器
/// </summary>
public class StoryLineManager : MonoInstance<StoryLineManager>
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private WorldManager worldManager;
    [SerializeField] private MainButtonControl mainButtonControl;
    [SerializeField] private Animator animator;
    [Header("剧情效果表")] [SerializeField] private DialogueManager dialogueManager;

    /// <summary>
    /// 正在剧情中的标志
    /// </summary>
    public bool isPlot;
    /// <summary>
    /// 游戏是否结局了
    /// </summary>
    public bool isEnd = false;

    private SaveData saveData;
    private PlayerCourseList playerCourseList;
    private PlotJudgmentList plotJudgmentList;
    private LinearPlotList linearPlotList;
    private BranchedPlotList branchedPlotList;
    private StudentsAttributePlotList studentsAttributePlotList;
    private UniversalPlotList universalPlotList;
    private TimerAndHintList timerAndHintList;
    private ArticleList articleList;

    /// <summary>
    /// 待触发的剧情列表
    /// </summary>
    public List<PlotJudgmentList.Row> qualifiedPlot;

    public override void Awake()
    {
        base.Awake();
        gameManager.InitGameEvent += Init;
        //gameManager.StartEvent += StartGame;
    }

    /// <summary>
    /// 开始剧情
    /// </summary>
    public void StartPlotJudgment()
    {
        if (JudgmentEnforceabilityRow())
        {
            BeganPlot();
        }
    }


    private void Init()
    {
        dialogueManager = DialogueManager.Instance;
        playerCourseList = gameManager.PlayerCourseList;
        plotJudgmentList = gameManager.PlotJudgmentList;
        linearPlotList = gameManager.LinearPlotList;
        branchedPlotList = gameManager.BranchedPlotList;
        studentsAttributePlotList = gameManager.StudentsAttributePlotList;
        universalPlotList = gameManager.UniversalPlotList;
        timerAndHintList = gameManager.TimerAndHintList;
        articleList = gameManager.articleList;
        saveData = gameManager.saveObject.SaveData;
        qualifiedPlot = new List<PlotJudgmentList.Row>();
    }

    /// <summary>
    /// 保存应该运行的剧情
    /// </summary>
    public bool JudgmentEnforceabilityRow()
    {
        qualifiedPlot.Clear();
        foreach (PlotJudgmentList.Row row in plotJudgmentList.GetRowList())
        {
            if (row.PlotId == "")
            {
                continue;
            }

            if (row.Year != "null" && row.Year != "")
            {
                if (int.Parse(row.Year) + DatetimeManager.Instance.InitYear == saveData.gameDate.year
                    && int.Parse(row.Semester) == saveData.gameDate.Semester
                    && int.Parse(row.Week) == saveData.gameDate.Week
                    && int.Parse(row.WhatDay) == saveData.gameDate.WhatDay) //此时符合日期要求
                {
                }
                else
                {
                    continue;
                }
            }

            if (row.SaveID != "null" && row.SaveID != "")
            {
                if (!saveData.plotSaveID.Exists(x => x == row.SaveID)) //存档没有它，则跳过
                {
                    continue;
                }
            }

            if (row.NoSaveID != "null" && row.NoSaveID != "")
            {
                if (saveData.plotSaveID.Exists(x => x == row.NoSaveID)) //存档中有它，此时剧情被跳过
                {
                    continue;
                }
            }

            if (row.RoleID != "null" && row.RoleID != "") //对学生有要求的
            {
                StudentUnit studentUnit = saveData.studentUnits.Find(x => x.id == row.RoleID);
                if (studentUnit == null) continue; //没找到这个学生，说明不符合要求
                if ((row.FriendshipID != "null" || row.FriendshipID != "")
                    && row.FriendshipValue != ""
                    && studentUnit.interpersonalRelationship.Exists(x => x.id == row.FriendshipID)
                )
                {
                    int requiredValue = int.Parse(row.FriendshipValue);
                    int currentValue = studentUnit.interpersonalRelationship.Find(x => x.id == row.FriendshipID).value;
                    if (requiredValue <= currentValue) //此时满足友谊值要求
                    {
                    }
                    else
                    {
                        continue;
                    }
                }

                int[] unitBasic =
                {
                    studentUnit.Mood,
                    studentUnit.Trust,
                    studentUnit.properties[0].score,
                    studentUnit.properties[1].score,
                    studentUnit.properties[2].score,
                    studentUnit.properties[3].score,
                    studentUnit.properties[4].score
                };
                bool isContinue = false;
                for (int i = 0; i < row.AllProperties.Length; i++)
                    if (row.AllProperties[i] > unitBasic[i])
                        isContinue = true;
                for (int i = 0; i < row.MainGrade.Length; i++)
                    if (row.MainGrade[i] > studentUnit.mainGrade[i].score)
                        isContinue = true;
                for (int i = 0; i < row.InterestGrade.Length; i++)
                    if (row.InterestGrade[i] > studentUnit.interestGrade[i].score)
                        isContinue = true;

                if (isContinue)
                    continue;
            }

            if (row.CourseID != "null" && row.CourseID != "") //对拥有课程有要求的
            {
                if (saveData.playerUnit.masterStudentCourse.Exists(x => x.id == row.CourseID)) //此时满足拥有课程的要求
                {
                }
                else
                {
                    continue;
                }
            }


            qualifiedPlot.Add(row);
        }

        isPlot = qualifiedPlot.Count > 0;
        return qualifiedPlot.Count > 0;
    }

    /// <summary>
    /// 从待触发列表开始剧情
    /// </summary>
    public void BeganPlot()
    {
        if (qualifiedPlot.Count == 0)
        {
            isPlot = false;
            dialoguePanel.SetActive(false);
            //关闭世界面板
            worldManager.OnExit();
            //重新播放主页音乐
            gameManager.bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType.Main);
            return;
        }

        bool isSwitch = dialoguePanel.activeSelf;//是否启用切换动画
        if (!dialoguePanel.activeSelf) dialoguePanel.SetActive(true);

        isPlot = true;

        string plotID = qualifiedPlot[0].PlotId;

        //设置场景
        if (qualifiedPlot[0].SceneName != "null" && qualifiedPlot[0].SceneName != "")
        {
            dialogueManager.SetScene(qualifiedPlot[0].SceneName,isSwitch);
        }
        else
        {
            dialogueManager.SetScene("class",isSwitch);
        }

        //设置bgm
        if (qualifiedPlot[0].MusicName != "null" && qualifiedPlot[0].MusicName != "")
        {
            if (qualifiedPlot[0].MusicName == "stop")
            {
                gameManager.bgmAudioControl.StopPlayAll();
            }
            else
            {
                gameManager.bgmAudioControl.PlayLoop(qualifiedPlot[0].MusicName,AudioControl.BackgroundMusicType.StoryLine);
            }
        }

        //播放音效
        if (qualifiedPlot[0].SoundName != "null" && qualifiedPlot[0].SoundName != "")
        {
            gameManager.bgmAudioControl.PlaySound(qualifiedPlot[0].SoundName);
        }

        dialogueManager.gameObject.SetActive(true);
        if (!dialogueManager.StartDialogue(linearPlotList.Find_PlotId(plotID)))
        {
            dialogueManager.StartDialogue(branchedPlotList.Find_PlotId(plotID));
        }

        qualifiedPlot.RemoveAt(0);
    }

    /// <summary>
    /// 剧情结果，变化属性之类的
    /// </summary>
    public void PlotResults(string results)
    {
        if (results == "null" || results == "") return;

        //变化学生的属性
        if (ChangeStudentProperty(results)) return;
        //触发提示和时间轴记录
        if (MakeHint(results)) return;
        //通用行为
        Universal(results);
    }

    /// <summary>
    ///  通用行为
    /// </summary>
    private bool Universal(string results)
    {
        var u = universalPlotList.Find_PlotId(results);
        if (u != null)
        {
            if (u.StudentID != "null" && u.StudentID != "")
                StudentsManager.Instance.AddStudent(u.StudentID);

            //加钱
            MoneyManager.Instance.Money += int.Parse(u.Money);
            if (u.SkillID != "null"&& u.SkillID !="") //添加教师技能，教师没有这个技能才添加
            {
                var row = playerCourseList.Find_Id(u.SkillID);
                if (!saveData.playerUnit.playerCourses.Exists(x => x.id == row.Id))
                {
                    saveData.playerUnit.playerCourses.Add(new PlayerCourse
                    {
                        empiricalValue = 0, id = u.SkillID, isHave = true, level = 0, name = row.Name
                    });
                }
            }

            if (u.SceneID != "null" && u.SceneID != "") //设置场景
                dialogueManager.SetScene(u.SceneID,true);
            if (u.MusicID != "null" && u.MusicID != "") //设置bgm
                gameManager.bgmAudioControl.PlayLoop(u.MusicID,AudioControl.BackgroundMusicType.StoryLine);
            if (u.SoundID != "null" && u.SoundID != "") //设置音效
                gameManager.bgmAudioControl.PlaySound(u.SoundID);

            if (u.ArticleID!="")
            {
                var row = articleList.Find_ID(u.ArticleID);
                if (row==null)
                {
                    Debug.Log("物品配置表中找不到ID："+u.ArticleID+"的物品");
                }
                else
                {
                    saveData.playerUnit.AddArticle(u.ArticleID,row.Name,int.Parse(u.ArticleNumber));
                    HintManager.Instance.AddHint(new Hint($"得到{row.Name}*{u.ArticleNumber}",row.description));
                }
            }
            return true;
        }

        return false;
    }

    private bool ChangeStudentProperty(string results)
    {
        var sa = studentsAttributePlotList.Find_PlotId(results);
        if (sa != null)
        {
            StudentUnit unit = saveData.studentUnits.Find(x => x.id == sa.StudentID);
            if (unit==null)
            {
                Debug.LogError(sa.StudentID+"不存在");
            }
            /*
             * 服装替换
             * 表情变化
             */
            int[] basics = {unit.Trust, unit.Mood, unit.properties[0].score, unit.properties[1].score, unit.properties[2].score, unit.properties[3].score, unit.properties[4].score};
            string[] saBasic = {sa.信任, sa.心情, sa.气质, sa.思维, sa.口才, sa.体质, sa.善恶};
            string[] saMainGrade = {sa.语文, sa.数学, sa.英语, sa.政治, sa.历史, sa.地理, sa.物理, sa.化学, sa.生物};
            string[] saInterestGrade = {sa.音乐, sa.表演, sa.舞蹈, sa.手工, sa.棋技, sa.种植, sa.摄影, sa.烹饪, sa.考古, sa.编程, sa.绘画, sa.运动};
            for (int i = 0; i < basics.Length; i++)
                basics[i] += int.Parse(saBasic[i]);

            for (int i = 0; i < saMainGrade.Length; i++)
                unit.mainGrade[i].score += int.Parse(saMainGrade[i]);

            for (int i = 0; i < saInterestGrade.Length; i++)
                unit.interestGrade[i].score += int.Parse(saInterestGrade[i]);

            if (sa.SaveID != "null" && sa.SaveID != "")//添加剧情ID
                saveData.plotSaveID.Add(sa.SaveID);

            if (sa.savePhoto != "null" && sa.savePhoto != "") //添加照片进相册
                unit.photoAlbum.Add(sa.savePhoto);

            if (sa.TimeBucket != "" && sa.TimeBucket != "null") //更改日程
            {
                unit.schedule[int.Parse(sa.TimeBucket)] = new Schedule {id = sa.CourseID, lockTime = int.Parse(sa.LockTime)};
                //触发小红点
                mainButtonControl.IsRemindSchedule = true;
            }
            if (sa.FriendID != "" && sa.FriendID != "null")// 变化友谊值
            {
                Relationship relationship = unit.interpersonalRelationship.Find(x => x.id == sa.FriendID);
                if (relationship==null)
                {
                    relationship = new Relationship() {id = sa.FriendID, value = 0};
                    unit.interpersonalRelationship.Add(relationship);
                }
                relationship.value += int.Parse(sa.FriendValue);
                relationship.messageLogging = sa.FriendMessage;
            }
            return true;
        }

        return false;
    }

    // /// <summary>
    // /// 变化友谊值
    // /// </summary>
    // private bool ChangeFriendship(string results)
    // {
    //     if (cf != null)
    //     {
    //         StudentUnit unit = saveData.studentUnits.Find(x => x.id == cf.StudentID);
    //         Relationship relationship = unit.interpersonalRelationship.Find(x => x.id == cf.FriendID);
    //         relationship.value += int.Parse(cf.Value);
    //         relationship.messageLogging = cf.Message;
    //         return true;
    //     }
    //
    //     return false;
    // }


    /// <summary>
    /// 触发提示、添加时间轴
    /// </summary>
    /// <param name="results"></param>
    /// <returns></returns>
    private bool MakeHint(string results)
    {
        var th = timerAndHintList.Find_PlotId(results);
        if (th != null)
        {
            if (th.isHint == "true") //是提示
                HintManager.Instance.AddHint(new Hint(th.headline, th.text));

            if (th.isTimerShaftNode == "true") //是时间轴消息
                PlayerPropertiesManager.Instance.AddStudentNode(th.RoleID, th.text);

            if (th.saveID != "" && th.saveID != "null") //有保存ID
                saveData.plotSaveID.Add(th.saveID);

            return true;
        }

        return false;
    }
}