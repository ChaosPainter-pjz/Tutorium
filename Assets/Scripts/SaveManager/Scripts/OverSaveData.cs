using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveManager.Scripts
{
    /// <summary>
    /// 追忆数据
    /// </summary>
    [Serializable]
    public class OverSaveData
    {
        [SerializeField] public List<OverUnit> overUnits = new();
    }

    /// <summary>
    /// 追忆单元
    /// </summary>
    [Serializable]
    public class OverUnit
    {
        [SerializeField] public string studentName;
        [SerializeField] public string playerName;
        public DateTime dateTime;
        [SerializeField] public string overId;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="studentName">学生的名字</param>
        /// <param name="playerName">玩家的名字</param>
        /// <param name="dateTime">结局时间</param>
        /// <param name="overId">结局的ID</param>
        public OverUnit(string studentName, string playerName, DateTime dateTime, string overId)
        {
            this.studentName = studentName;
            this.playerName = playerName;
            this.dateTime = dateTime;
            this.overId = overId;
        }

        private OverUnit()
        {
        }
    }
}
