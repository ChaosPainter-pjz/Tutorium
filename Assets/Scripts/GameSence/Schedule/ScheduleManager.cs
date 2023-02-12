using System.Collections.Generic;
using Basic;
using UnityEngine;
using static System.Single;

/// <summary>
/// 安排面板管理器
/// </summary>
public class ScheduleManager : MonoInstance<ScheduleManager>
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SelectManager selectManager;
    public CourseList CourseList;
    public ScheduleControl classroomScheduleControl;
    public ScheduleControl[] scheduleControl;
    private List<StudentUnit> studentUnits;
    //卡片的背景
    [SerializeField] public Sprite[] courseImage;
    public override void Awake()
    {
        base.Awake();
        gameManager.InitGameEvent += Init;
    }


    private void Init()
    {
        CourseList = gameManager.CourseList;
        studentUnits = gameManager.saveObject.SaveData.studentUnits;
        classroomScheduleControl.Init(gameManager.saveObject.SaveData.classroomSchedule,CourseList);

        ScheduleControlUpdate();
        //UpdateUI();
    }

    private void ScheduleControlUpdate()
    {
        for (int i = 0; i < studentUnits.Count; i++)
        {
            scheduleControl[i].gameObject.SetActive(true);
            scheduleControl[i].Init(studentUnits[i],CourseList);
        }

        for (int i = studentUnits.Count; i < scheduleControl.Length; i++)
            scheduleControl[i].gameObject.SetActive(false);
    }
     public void UIUpdate()
    {

        classroomScheduleControl.UpdateUI();
        ScheduleControlUpdate();
        SelectManager.Instance.UpdateUI();
    }

    /// <summary>
    /// 点亮最近的卡片
    /// </summary>
     public void LightNearestCard(Vector3 dragPosition)
     {
         SearchNearestCard(dragPosition)?.UpdateTemporaryUI(selectManager.row.Row2StudentCourse());
     }
    /// <summary>
    /// 设置最近的卡片为拖拽的卡片
    /// </summary>
    public void LockNearestCard(Vector3 dragPosition)
    {
        SearchNearestCard(dragPosition)?.UpdateCourse(selectManager.row.Row2StudentCourse());
    }
    /// <summary>
    /// 寻找最近的卡片
    /// </summary>
    /// <param name="dragPosition"></param>
    /// <returns></returns>
    private ScheduleCardControl SearchNearestCard(Vector3 dragPosition)
    {
        float distance = MaxValue;
        ScheduleCardControl scheduleCardControl = null;
        foreach (var control in classroomScheduleControl.scheduleCards)
        {
            control.CancelTemporaryUI();
            float dis = Vector3.Distance(control.transform.position, dragPosition);
            if (dis < distance)
            {
                distance = dis;
                scheduleCardControl = control;
            }
        }

        foreach (var control in scheduleControl)
        {
            if (!control.gameObject.activeSelf) continue;//如果这个日程表没有被激活，就跳过
            foreach (var cardControl in control.scheduleCards)
            {
                cardControl.CancelTemporaryUI();
                float dis = Vector3.Distance(cardControl.transform.position, dragPosition);
                if (dis<distance)
                {
                    distance = dis;
                    scheduleCardControl = cardControl;
                }
            }
        }

        if (Vector3.Distance(scheduleCardControl.gameObject.transform.position,dragPosition)>0.1f)
        {
            return null;
        }
        //Debug.Log(distance);
        return scheduleCardControl;
    }
}