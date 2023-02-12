using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 时间轴，普通节点控制器.
/// </summary>
public class TimerShaftNodeControl : MonoBehaviour
{
    public TimerShaftNode timerShaftNode;
    private List<TimerShaftStudentNode> timerShaftStudentNodes;
    private GameObject timerShaftStudentObject;
    private List<StudentNodeControl> studentNodeControls;
    [SerializeField] private Text whatDay;

    public void Init(TimerShaftNode timerShaftNode,GameObject timerShaftStudentObject)
    {
        this.timerShaftNode = timerShaftNode;
        this.timerShaftStudentObject = timerShaftStudentObject;
        timerShaftStudentNodes = timerShaftNode.timerShaftStudentNodeList;
        studentNodeControls = new List<StudentNodeControl>();
        SetWhatDay();
        InitStudentNode();
    }

    private void SetWhatDay()
    {
        whatDay.text = "第" + timerShaftNode.date.Week + "周" + timerShaftNode.date.ToString(Date.ToStringType.Week);
    }

    private void InitStudentNode()
    {
        foreach (var studentNode in timerShaftStudentNodes)
        {
            var nodeControl = Instantiate(timerShaftStudentObject, transform).GetComponent<StudentNodeControl>();
            nodeControl.Init(studentNode);
            studentNodeControls.Add(nodeControl);
        }
    }
    /// <summary>
    /// 检查词条，如果不符合，则修改
    /// </summary>
    public void InspectNode()
    {
        foreach (var studentNode in timerShaftStudentNodes)//遍历存档中的，该普通节点的所有学生节点
        {
            //寻找控制器中是否有与该节点相同的节点
            bool isHave = studentNodeControls.Any(control => control.node == studentNode);
            //有则跳出
            if (isHave)
                continue;
            //没有则增加
            var nodeControl = Instantiate(timerShaftStudentObject, transform).GetComponent<StudentNodeControl>();
            nodeControl.Init(studentNode);
            studentNodeControls.Add(nodeControl);
        }
    }
}