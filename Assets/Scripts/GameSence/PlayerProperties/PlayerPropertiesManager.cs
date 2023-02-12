using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPropertiesManager : MonoInstance<PlayerPropertiesManager>
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject playerPropertiesPanel;
    [Header("时间轴")]
    [SerializeField] private Toggle timerShaftButton;
    [SerializeField] private Transform timerShaftParent;
    [SerializeField] private GameObject timerShaftYearObject;
    [SerializeField] private GameObject timerShaftNodeObject;
    [SerializeField] private GameObject timerShaftStudentObject;
    [SerializeField] private GameObject today;
    [Header("教师技能")]

    [SerializeField] private Transform playerSkillItemParent;
    [SerializeField] private GameObject playerSkillItemObject;
    private Date date;
    public string logStudentID;
    public string logText;
    /// <summary>
    /// UI中的大节点
    /// </summary>
    private List<GameObject> nodeList;
    public Sprite[] studentHeadPortrait;
    /// <summary>
    /// 存档中的大节点
    /// </summary>
    private List<TimerShaftNode> timerShaftNodes;
    // 存档中玩家教学技能、工作技能
    private List<PlayerCourse> playerCourses;
    private List<PlayerSkillItemControl> playerSkillItemControls;
    public override void Awake()
    {
        base.Awake();
        gameManager.StartEvent += Init;
    }

    private void Init()
    {
        timerShaftNodes = gameManager.saveObject.SaveData.timerShaftNodes;
        playerCourses = gameManager.saveObject.SaveData.playerUnit.playerCourses;
        nodeList = new List<GameObject>();
        playerSkillItemControls = new List<PlayerSkillItemControl>();
        InitTimerShaftUI();
        SkillUIUpdate();
    }
    //初始化时间轴的UI
    private void InitTimerShaftUI()
    {
        date = gameManager.saveObject.SaveData.gameDate;
        if (timerShaftNodes.Count==0)
        {
            //增加年的节点
            timerShaftNodes.Add(new TimerShaftNode(TimerShaftNode.NodeType.Year,date.Copy(),null));
        }
        foreach (var node in timerShaftNodes)
        {
            switch (node.nodeType)
            {
                case TimerShaftNode.NodeType.WhatDay:
                    GameObject nodeObj = Instantiate(timerShaftNodeObject, timerShaftParent);
                    TimerShaftNodeControl control = nodeObj.GetComponent<TimerShaftNodeControl>();
                    control.Init(node,timerShaftStudentObject);
                    nodeList.Add(nodeObj);
                    break;
                case TimerShaftNode.NodeType.Year:
                    GameObject yearNodeObj = Instantiate(timerShaftYearObject, timerShaftParent);
                    yearNodeObj.GetComponent<TimerShaftYearControl>().ThisDate = node.date;
                    nodeList.Add(yearNodeObj);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        today.transform.SetAsLastSibling();
    }
    /// <summary>
    /// 新增一条时间轴记录
    /// </summary>
    public void AddStudentNode(string studentID, string content)
    {
        TimerShaftStudentNode studentNode = new TimerShaftStudentNode(studentID, content);
        if (timerShaftNodes.Count==0)
        {
            //增加年的节点
            timerShaftNodes.Add(new TimerShaftNode(TimerShaftNode.NodeType.Year,new Date(DatetimeManager.Instance.InitYear,0,1,0),null));
            GameObject yearNodeObj = Instantiate(timerShaftYearObject, timerShaftParent);
            yearNodeObj.GetComponent<TimerShaftYearControl>().ThisDate = date.Copy();
            nodeList.Add(yearNodeObj);
        }
        Date saveLowDate = timerShaftNodes[timerShaftNodes.Count - 1].date;

        //如果时间轴里最后一个普通节点的日期与当前时间相同，且是小节点
        if (saveLowDate == date && timerShaftNodes[timerShaftNodes.Count - 1].nodeType==TimerShaftNode.NodeType.WhatDay)
        {
            timerShaftNodes[timerShaftNodes.Count - 1].timerShaftStudentNodeList.Add(studentNode);
            nodeList[timerShaftNodes.Count - 1].GetComponent<TimerShaftNodeControl>().InspectNode();
        }else if (saveLowDate.Semester != date.Semester)
        {
            //增加年的节点
            timerShaftNodes.Add(new TimerShaftNode(TimerShaftNode.NodeType.Year,date.Copy(),null));
            GameObject yearNodeObj = Instantiate(timerShaftYearObject, timerShaftParent);
            yearNodeObj.GetComponent<TimerShaftYearControl>().ThisDate = date.Copy();
            nodeList.Add(yearNodeObj);

            //增加普通节点
            TimerShaftNode node = new TimerShaftNode(TimerShaftNode.NodeType.WhatDay, date.Copy(), studentNode);
            timerShaftNodes.Add(node);
            GameObject nodeObj = Instantiate(timerShaftNodeObject, timerShaftParent);
            TimerShaftNodeControl control = nodeObj.GetComponent<TimerShaftNodeControl>();
            control.Init(node,timerShaftStudentObject);
            nodeList.Add(nodeObj);
        }else if (saveLowDate.Week != date.Week || saveLowDate.WhatDay != date.WhatDay)
        {
            //增加普通节点
            TimerShaftNode node = new TimerShaftNode(TimerShaftNode.NodeType.WhatDay, date.Copy(), studentNode);
            timerShaftNodes.Add(node);
            GameObject nodeObj = Instantiate(timerShaftNodeObject, timerShaftParent);
            TimerShaftNodeControl control = nodeObj.GetComponent<TimerShaftNodeControl>();
            control.Init(node,timerShaftStudentObject);
            nodeList.Add(nodeObj);
        }
        else
        {
            Debug.Log("不符合条件的时间轴消息");
        }
        today.transform.SetAsLastSibling();
    }
    [ContextMenu("添加时间轴消息")]
    public void AddStudentNode()
    {
        AddStudentNode(logStudentID, logText);
    }
    // //刷新玩家技能的UI
    // public void SkillUIUpdate()
    // {
    //     foreach (var playerCourse in playerCourses)
    //     {
    //         if (!playerCourse.isHave)
    //             continue;
    //
    //         bool isHave = false;
    //         foreach (var unused in playerSkillItemControls)
    //         {
    //             if (unused.thisCourse.id == playerCourse.id)
    //             {
    //                 isHave = true;
    //             }
    //         }
    //         if (isHave)
    //             continue;
    //         var control = Instantiate(playerSkillItemObject,playerSkillItemParent).GetComponent<PlayerSkillItemControl>();
    //         control.Init(playerCourse,gameManager);
    //         playerSkillItemControls.Add(control);
    //     }
    // }
    //刷新玩家技能的UI
    public void SkillUIUpdate()
    {
        while (playerSkillItemControls.Count < playerCourses.Count)
        {
            var control = Instantiate(playerSkillItemObject,playerSkillItemParent).GetComponent<PlayerSkillItemControl>();
            control.Init(gameManager);
            playerSkillItemControls.Add(control);
        }

        for (int i = 0; i < playerCourses.Count; i++)
        {
            playerSkillItemControls[i].UpdateUI( playerCourses[i]);
        }
    }
    /// <summary>
    /// 点击打开玩家面板
    /// </summary>
    public void OnEnterPanel()
    {
        playerPropertiesPanel.SetActive(true);
        SkillUIUpdate();
        timerShaftButton.isOn = true;
    }
}