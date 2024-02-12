using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit
{
    /// <summary>
    /// 成绩
    /// </summary>
    [Serializable]
    public class Grade
    {
        /// <summary>
        /// 成绩的ID
        /// </summary>
        [SerializeField] public string gradeID;

        /// <summary>
        /// 得分
        /// </summary>
        [SerializeField] public int score;

        public int MAXScore => 100;

        /// <summary>
        /// 成绩
        /// </summary>
        public int GradeScore
        {
            get
            {
                int range = (int)(Mathf.Pow(1 - (score / 1000.0f - 1) * (score / 1000.0f - 1), 0.5f) * 100) +
                            Random.Range(-2, 3);
                if (range < 0)
                    range = 0;
                else if (range > MAXScore) range = MAXScore;

                return range;
            }
        }

        [SerializeField] public string name;

        public Grade Copy()
        {
            return new Grade()
            {
                gradeID = this.gradeID,
                score = this.score,
                name = this.name,
            };
        }
        
    }
}
