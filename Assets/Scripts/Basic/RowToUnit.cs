using System;
using System.Diagnostics;
using Basic.CSV2Table;
using GameSence.Classroom;
using Unit;

namespace Basic
{
    /// <summary>
    /// 拓展方法类
    /// </summary>
    public static class RowToUnit
    {
        /// <summary>
        /// 将一个ROW转化为学生可用的课程
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static StudentCourse Row2StudentCourse(this CourseList.Row row)
        {
            var type = row.Type switch
            {
                "M" => StudentCourse.Type.Main,
                "I" => StudentCourse.Type.Interest,
                "O" => StudentCourse.Type.Other,
                "N" => StudentCourse.Type.Other,
                _ => throw new ArgumentOutOfRangeException()
            };
            return new StudentCourse(row.Id, row.Name, type);
        }

        /// <summary>
        /// 将一个物品row，转化为用于存档的类
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Article Row2Article(this ArticleList.Row row)
        {
            return new Article(row.ID, row.Name, 0);
        }
    }
}