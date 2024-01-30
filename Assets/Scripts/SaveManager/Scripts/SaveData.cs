using System;
using System.Collections.Generic;
using GameSence.Classroom;
using GameSence.Hint;
using GameSence.PlayerProperties;
using Unit;
using UnityEngine;

namespace SaveManager.Scripts
{
    [Serializable]
    public class SaveData
    {
        /// <summary>
        /// 初始年份
        /// </summary>
        [SerializeField] public int initYear = 2000;

        /// <summary>
        /// 历史每日新闻
        /// </summary>
        [SerializeField] public List<string> dailyNews = new();

        public int InitYear
        {
            get => initYear;
            set
            {
                initYear = value;
                gameDate.year = value;
                parkDate.year = value;
                midtermDate.year = value;
            }
        }

        /// <summary>
        /// 存档是否是实际可读的
        /// </summary>
        [SerializeField] public bool isInstance = false;

        [SerializeField] public int money = 0;

        /// <summary>
        /// 更新日期必须使用DatetimeManager
        /// </summary>
        [SerializeField] public Date gameDate = new(2000, 0, 1, 0);

        /// <summary>
        /// 世界的解锁状态
        /// </summary>
        [SerializeField] public bool isWorld = false;

        /// <summary>
        /// 上次访问公园的时间
        /// </summary>
        [SerializeField] public Date parkDate = new(2000, 0, 1, 0);

        /// <summary>
        /// 超市上次进货时间
        /// </summary>
        [SerializeField] public Date supermarketDate = new(2000, 0, 1, 0);

        /// <summary>
        /// 超市商品
        /// </summary>
        [SerializeField] public List<Article> supermarketCommodities = new();

        /// <summary>
        /// 上次期中考试的时间
        /// </summary>
        [SerializeField] public Date midtermDate = new(2000, 0, 1, 0);

        /// <summary>
        /// 朋克城
        /// </summary>
        public CoexData coexData = new();

        /// <summary>
        /// 期中考试提升倍率
        /// </summary>
        [SerializeField] public float midtermEfficiency = 0.07f;

        /// <summary>
        /// 分班中
        /// </summary>
        [SerializeField] public bool isDivideIntoClass = false;

        /// <summary>
        /// 成绩单打印中
        /// </summary>
        [SerializeField] public bool isTranscript = false;

        [SerializeField] public DateTime dateTime = DateTime.Now;

        [SerializeField] public PlayerUnit playerUnit = new();
        [SerializeField] public List<StudentUnit> studentUnits = new();

        /// <summary>
        /// 教室的日程
        /// </summary>
        [SerializeField] public Schedule[] classroomSchedule =
        {
            new() { id = "2" }, new() { id = "2" }, new() { id = "0" }, new() { id = "1" }, new() { id = "3" },
            new() { id = "0" }
        };

        /// <summary>
        /// 玩家的计划
        /// </summary>
        [SerializeField] public string[] playerPlan = new string[5] { "0", "0", "0", "0", "0" };

        /// <summary>
        /// 教室学习效果倍率
        /// </summary>
        [SerializeField] public float classroomEfficiency = 1.5f;

        /// <summary>
        /// 剧情所用的存档id
        /// </summary>
        [SerializeField] public List<string> plotSaveID = new();

        /// <summary>
        /// 时间轴记录
        /// </summary>
        [SerializeField] public List<TimerShaftNode> timerShaftNodes = new();

        /// <summary>
        /// 消息记录
        /// </summary>
        [SerializeField] public List<Hint> hints = new();
    }

    /// <summary>
    /// 朋克城数据记录
    /// </summary>
    [Serializable]
    public class CoexData
    {
        /// <summary>
        /// 注意，先复制一份再复制
        /// </summary>
        public Date date = new(2000, 0, 1, 0);

        //未领取的任务
        public List<Task> Tasks = new();

        //已经领取的任务
        public List<Task> LockTasks = new();

        [Serializable]
        public class Task
        {
            /// <summary>
            /// 接取者ID
            /// </summary>
            public string UnitId = "-1";

            public Grade Grade = new();
            public int RewardValue;
            public Date LockingTime;
        }
    }
}
