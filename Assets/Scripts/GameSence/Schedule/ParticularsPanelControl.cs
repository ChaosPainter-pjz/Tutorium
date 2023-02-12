using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/// <summary>
/// 详情面板
/// </summary>
public class ParticularsPanelControl : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Animator animator;
    [SerializeField] private Text courseName;
    [SerializeField] private Toggle toggle;
    /// 课程的描述
    [SerializeField] private Text courseDescribe;
    [SerializeField] private Sprite[] icons;
    [SerializeField] private Image iconImage;
    /// 课程的效果词条
    [SerializeField] private GameObject courseAttributesEntryPrefab;
    [SerializeField] private Transform courseAttributesEntryParent;

    [SerializeField] private AudioSource audioSource;
    //UI效果
    //是否正在显示
    public bool isDisplay = false;
    //显示时长
    public float displayTime = 2f;
    //消失时长
    public float extinctionTime = 0.1f;
    //计时器
    public float time = -1f;
    //固定的数据资源
    private PlayerCourseList playerCourseList;
    private PlayerCourseLevelList playerCourseLevelList;
    private readonly string[] basicPropertiesBasic = {"气质", "思维", "口才", "体质", "善恶", "信任", "心情"};

    private List<PlayerCourse> playerCourses;

    private List<CourseAttributesEntry> courseAttributesEntries;
    private bool isClassroom = false;
    private CourseList.Row courseRow;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    public void Init()
    {
        playerCourseList = gameManager.PlayerCourseList;
        playerCourseLevelList = gameManager.PlayerCourseLevelList;

        playerCourses = gameManager.saveObject.SaveData.playerUnit.playerCourses;

        InitDescribe();
    }
    /// <summary>
    /// 反转toggle
    /// </summary>
    public void InversionToggle()
    {
        audioSource.Play();
        isClassroom = !isClassroom;
        DescribeUIUpdate();
    }
    /// <summary>
    /// 打开详情面板
    /// </summary>
    public void OpenPanel(CourseList.Row row)
    {
        audioSource.Play();
        if (animator.GetBool(IsOpen))
        {
            animator.SetBool(IsOpen, false);
        }
        else
        {
            gameObject.SetActive(true);//该物体不应该被关闭的，但是这里以防万一
            courseRow = row;
            time = displayTime;
            isDisplay = true;
            animator.SetBool(IsOpen,true);
            DescribeUIUpdate();
        }
    }
    /// <summary>
    /// 详情面板的刷新
    /// </summary>
    public void DescribeUIUpdate()
    {
        if (courseRow == null) return;
        courseName.text = courseRow.Name;
        if (courseRow.Type !="M")
        {
            toggle.gameObject.SetActive(false);
        }
        else
        {
            toggle.gameObject.SetActive(true);

        }
        toggle.isOn = isClassroom;
        courseDescribe.text = courseRow.Description;
        foreach (var sprite in icons)
        {
            if (sprite.name == courseRow.Icon)
            {
                iconImage.sprite = sprite;
                break;
            }
        }

        float classroomEfficiency = isClassroom ? gameManager.saveObject.SaveData.classroomEfficiency : 1.0f;
        //读取当前课程可以触发的老师的技能
        List<PlayerCourseLevelList.Row> targets = LoadPlayerCourse(courseRow);
        int[] targetsBasicInt = //合并老师技能所带来的加成
        {
            targets.Sum(target => int.Parse(target.气质)),
            targets.Sum(target => int.Parse(target.思维)),
            targets.Sum(target => int.Parse(target.口才)),
            targets.Sum(target => int.Parse(target.体质)),
            targets.Sum(target => int.Parse(target.善恶)),
            targets.Sum(target => int.Parse(target.信任)),
            targets.Sum(target => int.Parse(target.心情))
        };
        int[] courseBasicInt = //课程本身带来的加成 + 教室倍率
        {
            Convert.ToInt32(int.Parse(courseRow.气质) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.思维) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.口才) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.体质) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.善恶) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.信任) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.心情) * classroomEfficiency)
        };

        for (int i = 0; i < basicPropertiesBasic.Length; i++) //遍历刷新UI
        {
            var entry0 = courseAttributesEntries.Find(x => x.propertyNameString == basicPropertiesBasic[i]);
            entry0.UIUpdate(courseBasicInt[i] + targetsBasicInt[i], targetsBasicInt[i]);
        }

        string[] nameGrade = {"语文", "数学", "英语", "政治", "历史", "地理", "物理", "化学", "生物", "音乐", "表演", "舞蹈", "手工", "棋技", "种植", "摄影", "烹饪", "考古", "编程", "绘画", "运动"};
        int[] targetsGradeInt = //合并老师技能所带来的分数加成
        {
            targets.Sum(target => int.Parse(target.语文)),
            targets.Sum(target => int.Parse(target.数学)),
            targets.Sum(target => int.Parse(target.英语)),
            targets.Sum(target => int.Parse(target.政治)),
            targets.Sum(target => int.Parse(target.历史)),
            targets.Sum(target => int.Parse(target.地理)),
            targets.Sum(target => int.Parse(target.物理)),
            targets.Sum(target => int.Parse(target.化学)),
            targets.Sum(target => int.Parse(target.生物)),
            targets.Sum(target => int.Parse(target.音乐)),
            targets.Sum(target => int.Parse(target.表演)),
            targets.Sum(target => int.Parse(target.舞蹈)),
            targets.Sum(target => int.Parse(target.手工)),
            targets.Sum(target => int.Parse(target.棋技)),
            targets.Sum(target => int.Parse(target.种植)),
            targets.Sum(target => int.Parse(target.摄影)),
            targets.Sum(target => int.Parse(target.烹饪)),
            targets.Sum(target => int.Parse(target.考古)),
            targets.Sum(target => int.Parse(target.编程)),
            targets.Sum(target => int.Parse(target.绘画)),
            targets.Sum(target => int.Parse(target.运动))
        };
        int[] courseGradeInt = //课程本身带来的分数加成 + 教室倍率
        {
            Convert.ToInt32(int.Parse(courseRow.语文) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.数学) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.英语) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.政治) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.历史) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.地理) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.物理) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.化学) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.生物) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.音乐) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.表演) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.舞蹈) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.手工) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.棋技) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.种植) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.摄影) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.烹饪) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.考古) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.编程) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.绘画) * classroomEfficiency),
            Convert.ToInt32(int.Parse(courseRow.运动) * classroomEfficiency)
        };
        for (int i = 0; i < nameGrade.Length; i++) //遍历刷新UI
        {
            var entry0 = courseAttributesEntries.Find(x => x.propertyNameString == nameGrade[i]);
            entry0.UIUpdate(courseGradeInt[i] + targetsGradeInt[i], targetsGradeInt[i]);
        }
    }

    private List<PlayerCourseLevelList.Row> LoadPlayerCourse(CourseList.Row inCourse)
    {
        List<PlayerCourseLevelList.Row> targets = new List<PlayerCourseLevelList.Row>();
        //遍历老师当前已有的技能
        foreach (PlayerCourse playerCourse in playerCourses)
        {
            if (!playerCourse.isHave) continue; //此时该技能未解锁
            PlayerCourseLevelList.Row target = null;
            switch (playerCourse.id)
            {
                case "1":
                    //语文进修
                    if (inCourse.Id == "1" && isClassroom) target = GetTarget();
                    break;
                case "10":
                    //快乐课堂
                    if (isClassroom) target = GetTarget();
                    break;
                case "20":
                    if (inCourse.Id == "36") target = GetTarget();
                    break;
                case "21": //奥数进阶
                    if (inCourse.Id == "2" && isClassroom) target = GetTarget();
                    break;
                case "22":
                    if (inCourse.Id == "41") target = GetTarget();
                    break;
                case "23":
                    if (inCourse.Id == "40") target = GetTarget();
                    break;
                case "24":
                    if (inCourse.Id == "17") target = GetTarget();
                    break;
                case "25":
                    if (inCourse.Id == "33") target = GetTarget();
                    break;
            }

            if (target != null)
            {
                targets.Add(target);
            }

            PlayerCourseLevelList.Row GetTarget() //局部函数，通过playerCourse的数据来寻找合适的教师技能加成row条目
            {
                //找到符合该技能ID的升级数据
                List<PlayerCourseLevelList.Row> allId = playerCourseLevelList.FindAll_id(playerCourse.id);
                //找到符合该技能等级的升级数据
                List<PlayerCourseLevelList.Row> allIdLevel = allId.FindAll(x => int.Parse(x.等级) == playerCourse.level);
                if (allIdLevel.Count == 0)
                {
                    Debug.Log("未找到指定id：" + playerCourse.id + "对应的" + playerCourse.level + "级条目");
                }

                //抽取一条来作为属性变化依据
                return allIdLevel[Random.Range(0, allIdLevel.Count - 1)];
            }
        }

        return targets;
    }
    /// <summary>
    /// 初始化详情面板
    /// </summary>
    private void InitDescribe()
    {
        void InstantiateCard(string gradeName,CourseAttributesEntry.AttributeType type)
        {
            var obj = Instantiate(courseAttributesEntryPrefab, courseAttributesEntryParent).GetComponent<CourseAttributesEntry>();
            obj.Init(gradeName,type);
            courseAttributesEntries.Add(obj);
        }

        courseAttributesEntries = new List<CourseAttributesEntry>();
        StudentUnit unit = new StudentUnit();
        foreach (var grade in unit.mainGrade)
            InstantiateCard(grade.name,CourseAttributesEntry.AttributeType.Main);

        foreach (var grade in unit.interestGrade)
            InstantiateCard(grade.name,CourseAttributesEntry.AttributeType.Interest);
        foreach (string str in basicPropertiesBasic)
            InstantiateCard(str,CourseAttributesEntry.AttributeType.Property);
    }

    public void OnPointerEnter()
    {
        time = 1000f;
    }

    public void OnPointerExit()
    {
        time = extinctionTime;
    }

    private void Update()
    {
        if (isDisplay)
        {
            if (time > 0 )
            {
                time -= Time.deltaTime;
            }
            else
            {
                isDisplay = false;
                animator.SetBool(IsOpen,false);
            }
        }

    }
}