using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoInstance<GameManager>
{
    public SaveObject saveObject;
    public DataObject dataObject;
    public AudioControl bgmAudioControl;
    public AudioSource sound;
    [SerializeField] private GameObject startGameObject;
    [SerializeField] private ResourceManager resourceManager;
    /// <summary>
    /// 学生所有的课程的初始列表
    /// </summary>
    public CourseList CourseList;

    /// <summary>
    /// 玩家所有的技能初始列表
    /// </summary>
    public PlayerCourseList PlayerCourseList;

    /// <summary>
    /// 玩家所有技能的等级列表
    /// </summary>
    public PlayerCourseLevelList PlayerCourseLevelList;
    /// <summary>
    /// 书店所能出售的物品
    /// </summary>
    public StoreGoodsList StoreGoodsList;

    public SupermarketGoodsList SupermarketGoodsList;
    /// <summary>
    /// 道具物品表
    /// </summary>
    public ArticleList articleList;
    /// <summary>
    /// 玩家所有的工作列表
    /// </summary>
    public PlayerWorkList PlayerWorkList;

    /// <summary>
    /// 学生初始列表
    /// </summary>
    public StudentsList StudentsList;
    /// <summary>
    /// 学生去学校增加的属性，每周的
    /// </summary>
    public StudentWeeklyPropertyList StudentWeeklyPropertyList;
    /// <summary>
    /// 学生成绩评价表
    /// </summary>
    public StudentMarkList StudentMarkList;
    /// <summary>
    /// NPC信息列表
    /// </summary>
    public NpcList NpcList;

    /// <summary>
    /// 触发式表
    /// </summary>
    public PlotJudgmentList PlotJudgmentList;

    /// <summary>
    /// 分支式表
    /// </summary>
    public BranchedPlotList BranchedPlotList;

    /// <summary>
    /// 直线式表
    /// </summary>
    public LinearPlotList LinearPlotList;
    /// <summary>
    /// 其他触发式剧情表
    /// </summary>
    public PlotJudgmentList OtherPlotJudgmentList;
    /// <summary>
    /// 剧情效果表
    /// </summary>
    public StudentsAttributePlotList StudentsAttributePlotList;
    public UniversalPlotList UniversalPlotList;
    public TimerAndHintList TimerAndHintList;
    /// <summary>
    /// 每日新闻
    /// </summary>
    public DailyNews DailyNews;
    /// <summary>
    /// 游戏结局主表
    /// </summary>
    public GameOverList GameOverList;
    /// <summary>
    /// 学生在大学内的兴趣状况表
    /// </summary>
    public SchoolWorkList SchoolWorkList;
    /// <summary>
    /// 升学情况表
    /// </summary>
    public EnterHigherSchoolList EnterHigherSchoolList;
    public event UnityAction InitGameEvent;
    public event UnityAction StartEvent;

    public override void Awake()
    {
        base.Awake();
        InitData();
    }
    private void Start()
    {
        startGameObject.SetActive(true);
        //InitData();
        InitGameEvent?.Invoke();
        StartEvent?.Invoke();
    }

    private void InitData()
    {
        CourseList = new CourseList(dataObject.studentsCourseList);
        PlayerCourseList = new PlayerCourseList(dataObject.playerCourseList);
        PlayerCourseLevelList = new PlayerCourseLevelList(dataObject.playerCourseLevelList);
        StoreGoodsList = new StoreGoodsList(dataObject.storeGoodsList);
        SupermarketGoodsList = new SupermarketGoodsList(dataObject.supermarketGoodsList);
        articleList = new ArticleList(dataObject.articleList);
        PlayerWorkList = new PlayerWorkList(dataObject.playerWorkList);
        StudentsList = new StudentsList(dataObject.studentList);
        StudentMarkList = new StudentMarkList(dataObject.studentMarkList);
        StudentWeeklyPropertyList = new StudentWeeklyPropertyList(dataObject.studentWeeklyPropertyList);
        NpcList = new NpcList(dataObject.npcList);
        PlotInit();
        DailyNewsInit();
        GameOverInit();
        resourceManager.InitData();
    }
    /// <summary>
    /// 载入剧情相关表格
    /// </summary>
    private void PlotInit()
    {
        PlotJudgmentList = new PlotJudgmentList(dataObject.plotJudgmentList);
        BranchedPlotList = new BranchedPlotList(dataObject.branchedPlotList);
        LinearPlotList = new LinearPlotList(dataObject.linearPlotList);
        OtherPlotJudgmentList = new PlotJudgmentList(dataObject.otherPlotJudgmentList);
        StudentsAttributePlotList = new StudentsAttributePlotList(dataObject.studentsAttributePlotList);
        UniversalPlotList = new UniversalPlotList(dataObject.universalPlotList);
        TimerAndHintList = new TimerAndHintList(dataObject.timerAndHintList);

    }
    /// <summary>
    /// 载入新闻相关表格
    /// </summary>
    private void DailyNewsInit()
    {
        DailyNews = new DailyNews(dataObject.dailyNews);
    }
    /// <summary>
    /// 载入结局相关表格
    /// </summary>
    private void GameOverInit()
    {
        GameOverList = new GameOverList(dataObject.gameOverList);
        SchoolWorkList = new SchoolWorkList(dataObject.schoolWorkList);
        EnterHigherSchoolList = new EnterHigherSchoolList(dataObject.enterHigherSchool);
    }
}