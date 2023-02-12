using Basic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 单个学生的日程的控制器
/// </summary>
public class ScheduleControl : MonoBehaviour
{
    [SerializeField] private Text fullName;
    [SerializeField] private Text mood;
    [SerializeField] public ScheduleCardControl[] scheduleCards;
    public bool isClassroom = false;
    private CourseList courseList;
    private Schedule[] schedule;
    private StudentUnit unit;
    /// <summary>
    /// 初始化，当前管理的是学生
    /// </summary>
    public void Init(StudentUnit unit,CourseList _courseList)
    {
        fullName.text = unit.fullName;
        this.unit = unit;
        Init(unit.schedule,_courseList);
    }
    /// <summary>
    /// 初始化，当前管理的是教室或者学生
    /// </summary>
    public void Init (Schedule[] _schedule,CourseList _courseList)
    {
        this.schedule = _schedule;
        courseList = _courseList;
        InitCard();
    }

    private void InitCard()
    {
        for (int i = 0; i < scheduleCards.Length; i++)
        {
            scheduleCards[i].Init(schedule[i],courseList.Find_Id(schedule[i].id).Row2StudentCourse(),isClassroom);
        }
    }
    /// <summary>
    /// 刷新卡片的UI为存档的日程
    /// </summary>
    public void UpdateUI()
    {
        if (!gameObject.activeSelf)
            return;
        foreach (var control in scheduleCards)
        {
            control.UpdateUI();
        }
    }

    public void UpdateMood()
    {
        if (!isClassroom)
        {
            mood.text = $"心情:{unit.Mood}";
        }
    }
}