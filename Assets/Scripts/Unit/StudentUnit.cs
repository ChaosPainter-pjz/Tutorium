using System.Collections.Generic;
using GameSence.Classroom;
using UnityEngine;

namespace Unit
{
    [System.Serializable]
    public class StudentUnit : Unit
    {
        /// <summary>
        /// 学校
        /// </summary>
        [SerializeField] public string school;
        /// <summary>
        /// 入学年份
        /// </summary>
        [SerializeField] public int enrollmentYear;
        /// <summary>
        /// 信任度
        /// </summary>
        [SerializeField] public int trust;

        /// <summary>
        /// 信任度
        /// </summary>
        public int Trust
        {
            get => trust;
            set => trust = value < 0 ? 0 : value;
        }

        [SerializeField] private int mood = 80;

        /// <summary>
        /// 心情
        /// </summary>
        public int Mood
        {
            get => mood;
            set
            {
                if (value > 100)
                {
                    mood = 100;
                }
                else if (value > 0)
                {
                    mood = value;
                }
                else
                {
                    mood = 0;
                }
            }
        }

        /// <summary>
        /// 心情影响的学习倍率
        /// </summary>
        public float MoodRate
        {
            get
            {
                if (Mood < 20)
                    return 0.25f;

                if (Mood < 40)
                    return 0.75f;

                return 1.0f;
            }
        }

        /// <summary>
        /// 总指示点数
        /// </summary>
        [Tooltip("总指示点数")] [SerializeField] public int indicatingPoints = 1;

        /// <summary>
        /// 当前的指示
        /// </summary>
        [SerializeField] public List<Grade> indicatingNow = new List<Grade>();
        /// <summary>
        /// 不包含心情和信任的五大基本属性
        /// </summary>
        public List<Grade> properties = new List<Grade>()
        {
            new Grade {gradeID = "temperament", name = "气质", score = 0},
            new Grade {gradeID = "thought", name = "思维", score = 0},
            new Grade {gradeID = "eloquence", name = "口才", score = 0},
            new Grade {gradeID = "physique", name = "身体素质", score = 0},
            new Grade {gradeID = "goodAndEvil", name = "善恶", score = 0},
        };


        /// <summary>
        /// 气质
        /// </summary>
        public Grade Temperament => properties[0];
        /// <summary>
        /// 思维
        /// </summary>
        public Grade Thought => properties[1];
        /// <summary>
        /// 口才
        /// </summary>
        public Grade Eloquence => properties[2];
        /// <summary>
        /// 身体素质
        /// </summary>
        public Grade Physique => properties[3];
        /// <summary>
        /// 善恶
        /// </summary>
        public Grade GoodAndEvil => properties[4];

        /// <summary>
        /// 人际关系
        /// </summary>
        [SerializeField] public List<Relationship> interpersonalRelationship = new List<Relationship>();

        /// <summary>
        /// 主修课程的成绩
        /// </summary>
        [SerializeField] public List<Grade> mainGrade = new List<Grade>
        {
            new Grade {gradeID = "0", name = "语文", score = 0},
            new Grade {gradeID = "1", name = "数学", score = 0},
            new Grade {gradeID = "2", name = "英语", score = 0},
            new Grade {gradeID = "3", name = "政治", score = 0},
            new Grade {gradeID = "4", name = "历史", score = 0},
            new Grade {gradeID = "5", name = "地理", score = 0},
            new Grade {gradeID = "6", name = "物理", score = 0},
            new Grade {gradeID = "7", name = "化学", score = 0},
            new Grade {gradeID = "8", name = "生物", score = 0}
        };

        /// <summary>
        /// 兴趣课程的成绩
        /// </summary>
        [SerializeField] public List<Grade> interestGrade = new List<Grade>()
        {
            new Grade {gradeID = "10", name = "音乐", score = 0},
            new Grade {gradeID = "11", name = "表演", score = 0},
            new Grade {gradeID = "12", name = "舞蹈", score = 0},
            new Grade {gradeID = "13", name = "手工", score = 0},
            new Grade {gradeID = "14", name = "棋技", score = 0},
            new Grade {gradeID = "15", name = "种植", score = 0},
            new Grade {gradeID = "16", name = "摄影", score = 0},
            new Grade {gradeID = "17", name = "烹饪", score = 0},
            new Grade {gradeID = "18", name = "考古", score = 0},
            new Grade {gradeID = "19", name = "编程", score = 0},
            new Grade {gradeID = "20", name = "绘画", score = 0},
            new Grade {gradeID = "21", name = "运动", score = 0}
        };

        // /// <summary>
        // /// 上周的日程
        // /// </summary>
        // [SerializeField] public Schedule[] lastWeekSchedule =
        // {
        //     new Schedule {id = "0"}, new Schedule {id = "0"}, new Schedule {id = "0"}, new Schedule {id = "0"}, new Schedule {id = "0"}, new Schedule {id = "0"}
        // };

        /// <summary>
        /// 当前的日程,注意：不可以更改Schedule本身，要改ID和lockTime，否则会出现问题
        /// </summary>
        [SerializeField] public Schedule[] schedule =
        {
            new Schedule {id = "0"}, new Schedule {id = "0"}, new Schedule {id = "0"}, new Schedule {id = "0"}, new Schedule {id = "0"}, new Schedule {id = "0"}
        };

        /// <summary>
        /// 相册
        /// </summary>
        [SerializeField] public List<string> photoAlbum = new List<string>();
        /// <summary>
        /// 该学生拥有的物品
        /// </summary>
        [SerializeField] public List<Article> articles = new List<Article>();

        /// <summary>
        /// 高考分班的选择
        /// </summary>
        [SerializeField] public List<Grade> NceeSelect;
        /// <summary>
        /// 高考分班的选择
        /// </summary>
        public List<Grade> NCEESelect
        {
            get
            {
                if (NceeSelect==null||NceeSelect.Count==0)
                {
                    NceeSelect = new List<Grade>()
                    {
                        mainGrade[3],
                        mainGrade[4],
                        mainGrade[5],
                    };
                }

                return NceeSelect;
            }
        }
    }
}
