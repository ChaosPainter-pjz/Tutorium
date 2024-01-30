using System;
using UnityEngine;

namespace Unit
{
    /// <summary>
    /// 日期
    /// </summary>
    [Serializable]
    public class Date
    {
        protected bool Equals(Date other)
        {
            return year == other.year && semester == other.semester && week == other.week && whatDay == other.whatDay;
        }

        public Date Copy()
        {
            return new Date(year, semester, week, whatDay);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Date)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = year;
                hashCode = (hashCode * 397) ^ semester;
                hashCode = (hashCode * 397) ^ week;
                hashCode = (hashCode * 397) ^ whatDay;
                hashCode = (hashCode * 397) ^ (WhatDayString != null ? WhatDayString.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        /// 最大周数
        /// </summary>
        public const int MaxWeek = 8;

        /// <summary>
        /// 今是何年
        /// </summary>
        [SerializeField] public int year;

        /// <summary>
        /// 今天是上学期还是下学期，0：上学期。1：下学期
        /// </summary>
        [SerializeField] private int semester;

        /// <summary>
        /// 今天是上学期还是下学期，0：上学期。1：下学期
        /// </summary>
        public int Semester
        {
            get => semester;
            set
            {
                year += value / 2;
                semester = value % 2;
            }
        }

        /// <summary>
        /// 今天是第几周,周数不存在第0周
        /// </summary>
        [SerializeField] private int week;

        public int Week
        {
            get => week;
            set
            {
                if (value % MaxWeek == 0)
                {
                    Semester += value / MaxWeek - 1;
                    week = MaxWeek;
                }
                else
                {
                    Semester += value / MaxWeek;
                    week = value % MaxWeek;
                }
            }
        }

        /// <summary>
        /// 今天是星期几
        /// </summary>
        [SerializeField] private int whatDay;

        public int WhatDay
        {
            get => whatDay;
            set
            {
                Week += value / 7;
                whatDay = value % 7;
            }
        }

        public static string[] WhatDayString =
        {
            "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"
        };

        /// <summary>
        ///创建一个日期
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="semester">学期</param>
        /// <param name="week">周数</param>
        /// <param name="whatDay">周几</param>
        public Date(int year, int semester, int week, int whatDay)
        {
            this.year = year;
            Semester = semester;
            Week = week;
            WhatDay = whatDay;
        }

        public Date()
        {
        }

        public static Date operator -(Date a, Date b)
        {
            var date = new Date
            {
                WhatDay = a.WhatDay - b.WhatDay + 7,
                week = a.week + -1 - b.week + MaxWeek,
                semester = a.semester - 1 - b.semester + 2,
                year = a.year - b.year - 1
            };
            return date;
        }

        public static bool operator ==(Date a, Date b)
        {
            return b is not null && a is not null && a.year == b.year && a.Semester == b.Semester && a.Week == b.Week &&
                   a.WhatDay == b.WhatDay;
        }

        public static bool operator !=(Date a, Date b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
            return a.year != b.year || a.Semester != b.Semester || a.Week != b.Week || a.WhatDay != b.WhatDay;
        }

        public override string ToString()
        {
            var strSemester = semester switch
            {
                0 => "上", 1 => "下",
                _ => throw new ArgumentOutOfRangeException()
            };

            return $"{year}年{strSemester}学期第{week}周{WhatDayString[WhatDay]}";
        }

        public string ToString(ToStringType type)
        {
            var strSemester = semester switch
            {
                0 => "上", 1 => "下",
                _ => throw new ArgumentOutOfRangeException()
            };
            switch (type)
            {
                case ToStringType.Long:
                    return ToString();
                case ToStringType.Brief:
                    return $"{year}年{strSemester}学期第{week}周";
                case ToStringType.Week:
                    return WhatDayString[WhatDay];
                case ToStringType.Year:
                    return $"{year}年";
                case ToStringType.YearAndSemester:
                    return $"{year}年{strSemester}学期";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public enum ToStringType
        {
            Long,
            Brief,
            Week,
            Year,
            YearAndSemester
        }

        public static bool operator <(Date a, Date b)
        {
            return a.year < b.year || a.semester < b.semester || a.week < b.week || a.whatDay < b.whatDay;
        }

        public static bool operator >(Date a, Date b)
        {
            return !(a < b);
        }
    }
}