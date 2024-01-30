using System.Collections.Generic;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.World.Game.PhotographyGame
{
    /// <summary>
    /// 摄影小游戏结算面板控制器
    /// </summary>
    public class PhotographyResultControl : MonoBehaviour
    {
        [SerializeField] private PhotographyGameControl photographyGameControl;
        [SerializeField] private Sprite[] medals;
        [SerializeField] private Transform scoreCardParent;
        [SerializeField] private GameObject scoreCardPrefab;
        [SerializeField] private Text totalPoints;
        private List<PhotographyGameControl.Achievement> achievements;
        private List<ScoreCardControl> scoreCardControls;
        private StudentUnit studentUnit;
        private int totalScore;

        public void Init(List<PhotographyGameControl.Achievement> _achievements, StudentUnit _studentUnit)
        {
            achievements = _achievements;
            studentUnit = _studentUnit;
            scoreCardControls ??= new List<ScoreCardControl>();
            for (var i = 0; i < achievements.Count; i++)
            {
                if (i >= scoreCardControls.Count)
                {
                    var control = Instantiate(scoreCardPrefab, scoreCardParent).GetComponent<ScoreCardControl>();
                    scoreCardControls.Add(control);
                }

                scoreCardControls[i].Init(_achievements[i], medals);
            }

            totalScore = 0;
            foreach (var achievement in achievements) totalScore += achievement.IntValue;

            totalPoints.text = totalScore.ToString();
        }

        public void OnEnter()
        {
            gameObject.SetActive(false);
            photographyGameControl.gameObject.SetActive(false);
            CreationAward();
        }

        /// <summary>
        /// 创建奖励
        /// </summary>
        private void CreationAward()
        {
            //创建排名
            var npcName = GameManager.GameManager.Instance.dataObject.randomName;
            var nameList = GameMathf.RandomList(0, npcName.Length - 1, 5);
            var rankings = new List<Ranking>();
            for (var i = 0; i < nameList.Count; i++)
            {
                var score = Random.Range(150 + i * 25, 250 + i * 15 > 300 ? 300 : 250 + i * 10);
                rankings.Add(new Ranking(npcName[nameList[i]], score));
            }

            var ranking = new Ranking(studentUnit.fullName, totalScore) { IsEmphasis = true };
            rankings.Add(ranking);
            rankings.Sort();

            //创建奖励,studentRanking为学生的名次
            var studentRanking = rankings.IndexOf(ranking) + 1;
            int money;
            var awards = new List<Award>();
            switch (studentRanking)
            {
                case 1:
                    studentUnit.Mood += 10;
                    studentUnit.Trust += 8;
                    studentUnit.interestGrade.Find(x => x.gradeID == "16").score += 60;
                    awards.Add(new Award("心情", 10));
                    awards.Add(new Award("信任", 8));
                    awards.Add(new Award("摄影", 60));
                    money = 1000;
                    break;
                case 2:
                    studentUnit.Mood += 5;
                    studentUnit.Trust += 5;
                    studentUnit.interestGrade.Find(x => x.gradeID == "16").score += 30;
                    awards.Add(new Award("心情", 5));
                    awards.Add(new Award("信任", 5));
                    awards.Add(new Award("摄影", 30));
                    money = 350;
                    break;
                case 3:
                    studentUnit.Mood += 5;
                    studentUnit.Trust += 5;
                    studentUnit.interestGrade.Find(x => x.gradeID == "16").score += 20;
                    awards.Add(new Award("心情", 5));
                    awards.Add(new Award("信任", 5));
                    awards.Add(new Award("摄影", 20));
                    money = 50;
                    break;
                default:
                    studentUnit.interestGrade.Find(x => x.gradeID == "16").score += 10;
                    awards.Add(new Award("摄影", 10));
                    money = 0;
                    break;
            }

            MoneyManager.Instance.Money += money;
            WorldGameManager.Instance.gameAwardControl.UpdateUI(rankings, studentUnit, awards, money);
        }
    }
}