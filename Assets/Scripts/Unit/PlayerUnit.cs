using System;
using System.Collections.Generic;
using GameSence.Classroom;
using UnityEngine;

namespace Unit
{
    [Serializable]
    public class PlayerUnit : Unit
    {
        /// <summary>
        /// 老师名字的第一个字
        /// </summary>
        public string abbName;

        /// <summary>
        /// 现在可以在选课页面选择的课程列表
        /// </summary>
        [SerializeField] public List<StudentCourse> masterStudentCourse = new()
        {
            new("1", "语文", StudentCourse.Type.Main),
            new("2", "数学", StudentCourse.Type.Main),
            new("3", "英语", StudentCourse.Type.Main),
            new("4", "政治", StudentCourse.Type.Main),
            new("5", "历史", StudentCourse.Type.Main),
            new("6", "地理", StudentCourse.Type.Main),
            new("7", "物理", StudentCourse.Type.Main),
            new("8", "化学", StudentCourse.Type.Main),
            new("9", "生物", StudentCourse.Type.Main),
            new("30", "公园散步", StudentCourse.Type.Other),
            new("32", "写额外作业", StudentCourse.Type.Other),
            new("33", "睡觉", StudentCourse.Type.Other)
        };

        /// <summary>
        /// 计划中可选的教研学习，有经验的工作，列表
        /// </summary>
        [SerializeField] public List<PlayerCourse> playerCourses = new()
        {
            new PlayerCourse() { empiricalValue = 0, id = "10", level = 0, name = "快乐课堂", isHave = true }
        };

        /// <summary>
        /// 背包
        /// </summary>
        [SerializeField] public List<Article> articles = new();

        /// <summary>
        /// 向背包添加一个物品
        /// </summary>
        /// <param name="_id">物品id</param>
        /// <param name="_name">物品名称</param>
        /// <param name="_number">物品数量</param>
        public void AddArticle(string _id, string _name, int _number)
        {
            var article = articles.Find(art => art.id == _id);
            if (article == null)
                articles.Add(new Article(_id, _name, _number));
            else
                article.number += _number;
            articles.Sort();
        }
    }
}