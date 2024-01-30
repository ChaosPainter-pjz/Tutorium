using System;
using UnityEngine;

namespace Unit
{
    /// <summary>
    /// 性别
    /// </summary>
    [Serializable]
    public enum Gender
    {
        None,
        Man,
        Woman
    }


    /// <summary>
    /// 描述人际关系
    /// </summary>
    [Serializable]
    public class Relationship
    {
        /// <summary>
        /// 对方ID
        /// </summary>
        [SerializeField] public string id;

        /// <summary>
        /// 双方友谊值
        /// </summary>
        [SerializeField] public int value;

        /// <summary>
        /// 聊天记录
        /// </summary>
        [SerializeField] public string messageLogging = "你好";
    }

    /// <summary>
    /// 玩家所能教的课程、学生能学的课程
    /// </summary>
    [Serializable]
    public abstract class Course
    {
        [SerializeField] public string id;
        [SerializeField] public string name;
    }

    /// <summary>
    /// 玩家所掌握的技能
    /// </summary>
    [Serializable]
    public class PlayerCourse : Course
    {
        [SerializeField] public int level;

        /// <summary>
        /// 经验值
        /// </summary>
        [SerializeField] public int empiricalValue;

        /// <summary>
        /// 是否已解锁
        /// </summary>
        [SerializeField] public bool isHave;
    }

    [Serializable]
    public class StudentCourse : Course
    {
        public enum Type
        {
            Main,
            Interest,
            Other
        }

        [SerializeField] public Type type;

        public StudentCourse(string id, string name, Type type)
        {
            this.id = id;
            this.name = name;
            this.type = type;
        }
    }

    /// <summary>
    /// 学生的日程
    /// </summary>
    [Serializable]
    public class Schedule
    {
        /// <summary>
        /// 课程的ID
        /// </summary>
        [SerializeField] public string id;

        /// <summary>
        /// 大于0时就不能更改这个安排
        /// </summary>
        [SerializeField] public int lockTime;
    }
}