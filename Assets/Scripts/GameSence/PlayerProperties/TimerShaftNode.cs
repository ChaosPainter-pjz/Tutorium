using System.Collections.Generic;
using UnityEngine;

namespace GameSence.PlayerProperties
{
    /// <summary>
    /// 时间轴节点的信息，用于存档
    /// </summary>
    [System.Serializable]
    public class TimerShaftNode
    {
        public enum NodeType
        {
            /// <summary>
            /// 只含年和学期的节点
            /// </summary>
            Year,

            /// <summary>
            /// 包含周数、星期几、学生节点的节点
            /// </summary>
            WhatDay
        }

        [SerializeField] public NodeType nodeType;
        [SerializeField] public Unit.Date date;
        [SerializeField] public List<TimerShaftStudentNode> timerShaftStudentNodeList;

        private TimerShaftNode()
        {
        }

        public TimerShaftNode(NodeType type, Unit.Date date, TimerShaftStudentNode studentNode)
        {
            this.date = date;
            nodeType = type;
            if (studentNode != null) timerShaftStudentNodeList = new List<TimerShaftStudentNode> { studentNode };
        }
    }
}