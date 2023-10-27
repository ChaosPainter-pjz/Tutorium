using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 单个属性变化提示面板的控制器
/// </summary>
public class ExecuteControl : MonoBehaviour
{
    /// <summary>
    /// 当前控制的学生
    /// </summary>
    private StudentUnit studentUnit;

    private List<StudentUnit> studentUnits;
    private int ids = 0;
    [SerializeField] private Text studentName;
    [SerializeField] private StoryLineManager storyLineManager;
    [SerializeField] private GameObject[] studentsPortrayals; // 学生的肖像
    [SerializeField] private MoodControl moodControl;
    [SerializeField] private Transform parent; //属性变化词条的父物体
    [SerializeField] private GameObject attributeText; // 属性变化词条的预制体

    [Header("点击确认后日期面板动画")] [SerializeField]
    private Animator animator;

    private CourseList courseList; //所有课程列表
    private PlayerCourseLevelList playerCourseLevelList; //玩家所有技能等级表

    private List<PlayerCourse> playerCourses; //存档中的玩家技能数据

    //本周教室的日程安排
    private Schedule[] classroomSchedule;

    /// <summary>
    /// 用于保存生成的属性变化词条
    /// </summary>
    private List<PropertyUpdateGlossaryControl> propertyUpdateGlossaryControls;

    public void Init()
    {
        storyLineManager = StoryLineManager.Instance;
        courseList = GameManager.Instance.CourseList;
        studentUnits = GameManager.Instance.saveObject.SaveData.studentUnits;
        classroomSchedule = GameManager.Instance.saveObject.SaveData.classroomSchedule;
        playerCourses = GameManager.Instance.saveObject.SaveData.playerUnit.playerCourses;
        playerCourseLevelList = GameManager.Instance.PlayerCourseLevelList;
        propertyUpdateGlossaryControls = new List<PropertyUpdateGlossaryControl>();
        InitEntry();
    }

    /// <summary>
    /// 生成所有属性变化词条
    /// </summary>
    private void InitEntry()
    {
        void InstantiateEntry(string gradeName, PropertyUpdateGlossaryControl.AttributeType type)
        {
            var control = Instantiate(attributeText, parent).GetComponent<PropertyUpdateGlossaryControl>();
            control.Init(gradeName, type);
            propertyUpdateGlossaryControls.Add(control);
        }

        var tUnit = new StudentUnit();
        foreach (Grade grade in tUnit.properties)
            InstantiateEntry(grade.name, PropertyUpdateGlossaryControl.AttributeType.Property);
        foreach (Grade grade in tUnit.mainGrade)
            InstantiateEntry(grade.name, PropertyUpdateGlossaryControl.AttributeType.Main);
        foreach (Grade grade in tUnit.interestGrade)
            InstantiateEntry(grade.name, PropertyUpdateGlossaryControl.AttributeType.Interest);
        InstantiateEntry("信任", PropertyUpdateGlossaryControl.AttributeType.Other);
    }

    /// <summary>
    /// 装载一个学生
    /// </summary>
    /// <param name="_studentUnit"></param>
    public void LoadStudent(StudentUnit _studentUnit)
    {
        studentUnit = _studentUnit;
        studentName.text = _studentUnit.fullName;
        //这里固定肖像
        foreach (GameObject obj in studentsPortrayals) obj.SetActive(obj.CompareTag(_studentUnit.id));

        StudentAttributeUpdate();
    }

    /// <summary>
    /// 审查日程，使锁定的日程减少
    /// </summary>
    private void ReviewSchedule()
    {
        for (var i = 0; i < studentUnit.schedule.Length; i++)
        {
            Schedule schedule = studentUnit.schedule[i];
            if (schedule.lockTime > 0)
            {
                schedule.lockTime--;
                if (schedule.lockTime <= 0)
                {
                    schedule.id = "0";
                    schedule.lockTime = 0;
                }
            }
        }
    }

    /// <summary>
    /// 更新学生的属性
    /// </summary>
    private void StudentAttributeUpdate()
    {
        //该学生的日程集合
        var rows = new List<CourseList.Row>();
        foreach (Schedule schedule in studentUnit.schedule)
        {
            CourseList.Row row = courseList.Find_Id(schedule.id);
            rows.Add(row);
        }

        ReviewSchedule();
        //复制一个学生，用来对比属性的变化
        //这个是初始的学生数值
        StudentUnit lowStudent = Copy(studentUnit);
        //这个是结算仅课程加成的学生数值
        for (var i = 0; i < rows.Count; i++)
        {
            var learningMultiple = 1f; //学习效率的倍数，教室的提升效果
            if (classroomSchedule[i].id == rows[i].Id)
                //此时当前课程是在教室上的
                learningMultiple = GameManager.Instance.saveObject.SaveData.classroomEfficiency;

            //变更学生的属性
            string[] mainStrings =
            {
                rows[i].语文, rows[i].数学, rows[i].英语, rows[i].政治, rows[i].历史, rows[i].地理, rows[i].物理, rows[i].化学,
                rows[i].生物
            };
            var mainRows = Array.ConvertAll(mainStrings, int.Parse);
            for (var j = 0; j < mainRows.Length; j++)
                studentUnit.mainGrade[j].score +=
                    Convert.ToInt32(mainRows[j] * learningMultiple * studentUnit.MoodRate);

            string[] interestStrings =
            {
                rows[i].音乐, rows[i].表演, rows[i].舞蹈, rows[i].手工, rows[i].棋技, rows[i].种植, rows[i].摄影, rows[i].烹饪,
                rows[i].考古, rows[i].编程, rows[i].绘画, rows[i].运动
            };
            var interestRows = Array.ConvertAll(interestStrings, int.Parse);
            for (var j = 0; j < interestRows.Length; j++)
                studentUnit.interestGrade[j].score +=
                    Convert.ToInt32(interestRows[j] * learningMultiple * studentUnit.MoodRate);

            string[] propertyString = { rows[i].气质, rows[i].思维, rows[i].口才, rows[i].体质, rows[i].善恶 };
            var propertyRows = Array.ConvertAll(propertyString, int.Parse);

            for (var j = 0; j < propertyRows.Length; j++)
                studentUnit.properties[j].score +=
                    Convert.ToInt32(propertyRows[j] * learningMultiple * studentUnit.MoodRate);

            studentUnit.Trust += Convert.ToInt32(int.Parse(rows[i].信任) * learningMultiple * studentUnit.MoodRate);
            studentUnit.Mood += Convert.ToInt32(int.Parse(rows[i].心情) * learningMultiple); //最后变心情,且心情不受自身倍率影响
        }

        StudentUnit classStudent = Copy(studentUnit); //class
        for (var i = 0; i < rows.Count; i++)
        {
            var isClassroom = classroomSchedule[i].id == rows[i].Id;
            //存储当前课程可以触发的老师的技能
            var targets = new List<PlayerCourseLevelList.Row>();
            //遍历老师当前已有的技能
            foreach (PlayerCourse playerCourse in playerCourses)
            {
                if (!playerCourse.isHave) continue; //此时该技能未解锁
                PlayerCourseLevelList.Row target = null;
                switch (playerCourse.id)
                {
                    case "1":
                        //语文进修
                        if (rows[i].Id == "1" && isClassroom) target = GetTarget();
                        break;
                    case "10":
                        //快乐课堂
                        if (isClassroom) target = GetTarget();
                        break;
                    case "20":
                        if (rows[i].Id == "36") target = GetTarget();
                        break;
                    case "21": //奥数进阶
                        if (rows[i].Id == "2" && isClassroom) target = GetTarget();
                        break;
                    case "22":
                        if (rows[i].Id == "41") target = GetTarget();
                        break;
                    case "23":
                        if (rows[i].Id == "40") target = GetTarget();
                        break;
                    case "24":
                        if (rows[i].Id == "17") target = GetTarget();
                        break;
                    case "25":
                        if (rows[i].Id == "33") target = GetTarget();
                        break;
                }

                if (target != null) targets.Add(target);

                PlayerCourseLevelList.Row GetTarget() //局部函数，通过playerCourse的数据来寻找合适的row条目
                {
                    //找到符合该技能ID的升级数据
                    List<PlayerCourseLevelList.Row> allId = playerCourseLevelList.FindAll_id(playerCourse.id);
                    //找到符合该技能等级的升级数据
                    List<PlayerCourseLevelList.Row> allIdLevel =
                        allId.FindAll(x => int.Parse(x.等级) == playerCourse.level);
                    if (allIdLevel.Count == 0)
                        Debug.Log("未找到指定id：" + playerCourse.id + "对应的" + playerCourse.level + "级条目");

                    //抽取一条来作为属性变化依据
                    return allIdLevel[Random.Range(0, allIdLevel.Count - 1)];
                }
            }

            //开始老师技能影响的属性变化
            foreach (PlayerCourseLevelList.Row target in targets)
            {
                string[] mainRowsString =
                {
                    target.语文, target.数学, target.英语, target.政治, target.历史, target.地理, target.物理, target.化学, target.生物
                };
                var mainRows = Array.ConvertAll(mainRowsString, int.Parse);
                for (var j = 0; j < mainRows.Length; j++)
                    studentUnit.mainGrade[j].score += Convert.ToInt32(mainRows[j] * studentUnit.MoodRate);

                string[] interestRowsString =
                {
                    target.音乐, target.表演, target.舞蹈, target.手工, target.棋技, target.种植, target.摄影, target.烹饪, target.考古,
                    target.编程, target.绘画, target.运动
                };
                var interestRows = Array.ConvertAll(interestRowsString, int.Parse);
                for (var j = 0; j < interestRows.Length; j++)
                    studentUnit.interestGrade[j].score += Convert.ToInt32(interestRows[j] * studentUnit.MoodRate);

                string[] propertyString = { target.气质, target.思维, target.口才, target.体质, target.善恶 };
                var propertyRows = Array.ConvertAll(propertyString, int.Parse);
                for (var j = 0; j < propertyRows.Length; j++)
                    studentUnit.properties[j].score += Convert.ToInt32(propertyRows[j] * studentUnit.MoodRate);

                studentUnit.Trust += Convert.ToInt32(int.Parse(target.信任) * studentUnit.MoodRate);
                studentUnit.Mood += Convert.ToInt32(int.Parse(target.心情));
            }
        }

        GenerateAttributeChangeEntries(studentUnit, lowStudent, Location.Low2New);
        GenerateAttributeChangeEntries(classStudent, lowStudent, Location.Low2Class);
        GenerateAttributeChangeEntries(studentUnit, classStudent, Location.Class2Player);
    }

    public enum Location
    {
        Low2New,
        Low2Class,
        Class2Player
    }

    /// <summary>
    /// 生成属性更改条目
    /// </summary>
    private void GenerateAttributeChangeEntries(StudentUnit newStudentUnit, StudentUnit lowStudentUnit,
        Location location)
    {
        switch (location)
        {
            case Location.Low2New:
            {
                //传入新旧属性值
                //传入课程本身的分数变化值
                void Low2New(IReadOnlyList<Grade> newUnit, IReadOnlyList<Grade> lowUnit)
                {
                    for (var i = 0; i < lowUnit.Count; i++)
                    {
                        Grade lowGrade = lowUnit[i];
                        Grade newGrade = newUnit[i];
                        propertyUpdateGlossaryControls.Find(controls => controls.entryName == lowGrade.name)
                            .UIUpdate(lowGrade.score, newGrade.score, location);
                    }
                }

                Low2New(newStudentUnit.properties, lowStudentUnit.properties);
                Low2New(newStudentUnit.mainGrade, lowStudentUnit.mainGrade);
                Low2New(newStudentUnit.interestGrade, lowStudentUnit.interestGrade);
                moodControl.DoubleSet(lowStudentUnit.Mood, newStudentUnit.Mood);
                //propertyUpdateGlossaryControls.Find(controls => controls.entryName == "心情").UIUpdate(lowStudentUnit.Mood, newStudentUnit.Mood, location);
                propertyUpdateGlossaryControls.Find(controls => controls.entryName == "信任")
                    .UIUpdate(lowStudentUnit.Trust, newStudentUnit.Trust, location);
                break;
            }
            default:
            {
                void Default(IReadOnlyList<Grade> newUnit, IReadOnlyList<Grade> lowUnit)
                {
                    for (var i = 0; i < lowUnit.Count; i++)
                    {
                        Grade lowGrade = lowUnit[i];
                        Grade newGrade = newUnit[i];
                        propertyUpdateGlossaryControls.Find(controls => controls.entryName == lowGrade.name)
                            .UIUpdate(newGrade.score - lowGrade.score, location);
                    }
                }

                //传入课程本身的分数变化值
                Default(newStudentUnit.properties, lowStudentUnit.properties);
                Default(newStudentUnit.mainGrade, lowStudentUnit.mainGrade);
                Default(newStudentUnit.interestGrade, lowStudentUnit.interestGrade);

                //propertyUpdateGlossaryControls.Find(controls => controls.entryName == "心情").UIUpdate(newStudentUnit.Mood - lowStudentUnit.Mood, location);
                propertyUpdateGlossaryControls.Find(controls => controls.entryName == "信任")
                    .UIUpdate(newStudentUnit.Trust - lowStudentUnit.Trust, location);
                break;
            }
        }
    }

    /// <summary>
    /// 点击确认后，切换
    /// </summary>
    public void OnEnter()
    {
        if (studentUnits[studentUnits.Count - 1].id == studentUnit.id)
        {
            ids = 0;
            animator.SetTrigger("Trigger"); //动画结束后会有剧情触发事件
            gameObject.SetActive(false);
        }
        else
        {
            ids++;
            LoadStudent(studentUnits[ids]);
        }
    }

    /// <summary>
    /// 复制一个实例
    /// </summary>
    private static T Copy<T>(T realObject)
    {
        using Stream objectStream = new MemoryStream();
        //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制
        IFormatter formatter = new BinaryFormatter();
        formatter.Serialize(objectStream, realObject);
        objectStream.Seek(0, SeekOrigin.Begin);
        return (T)formatter.Deserialize(objectStream);
    }
}