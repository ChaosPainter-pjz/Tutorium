using System;

namespace GameSence.World.Game
{
    /// <summary>
    /// 小游戏排名单位
    /// </summary>
    public class Ranking : IComparable
    {
        public readonly string Name;
        public readonly int Score;

        /// <summary>
        /// 是否高亮该学生的名次
        /// </summary>
        public bool IsEmphasis = false;

        public Ranking(string name, int score)
        {
            Name = name;
            Score = score;
        }

        private Ranking()
        {
        }

        public int CompareTo(object obj)
        {
            if (obj is Ranking other)
                return -Score.CompareTo(other.Score);
            else
                throw new ArgumentException("Object is not a Ranking");
        }
    }
}