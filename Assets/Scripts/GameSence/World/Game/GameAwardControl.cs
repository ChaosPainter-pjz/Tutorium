using System.Collections.Generic;
using Basic;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.World.Game
{
    public class GameAwardControl : MonoBehaviour
    {
        [SerializeField] private RankingCardControl[] rankingCardControls;
        [SerializeField] private Text money;
        [SerializeField] private GameObject awardPrefab;
        [SerializeField] private Transform awardParent;
        [SerializeField] private List<AwardCardControl> awardCardControls = new();

        /// <summary>
        /// 刷新世界游戏的排名、奖励结算面板
        /// </summary>
        /// <param name="rankings">排名序列</param>
        /// <param name="studentUnit">参与的学生</param>
        /// <param name="awards">奖励序列</param>
        /// <param name="_money">奖励金钱数</param>
        public void UpdateUI(List<Ranking> rankings, StudentUnit studentUnit, List<Award> awards, int _money)
        {
            gameObject.SetActive(true);
            UpdateRanking(rankings);
            UpdateAward(awards);

            if (_money > 0)
                money.text = "+" + _money;
            else if (_money < 0)
                money.text = "-" + _money;
            else
                money.text = "" + _money;
        }

        /// <summary>
        /// 更新排名
        /// </summary>
        private void UpdateRanking(List<Ranking> rankings)
        {
            rankings.Sort();
            for (var i = 0; i < rankingCardControls.Length; i++) rankingCardControls[i].UIUpdate(rankings[i]);
        }

        /// <summary>
        /// 更新奖励内容的显示
        /// </summary>
        private void UpdateAward(List<Award> awards)
        {
            foreach (var control in awardCardControls) control.gameObject.SetActive(false);

            for (var i = 0; i < awards.Count; i++)
            {
                if (i >= awardCardControls.Count)
                {
                    var control = Instantiate(awardPrefab, awardParent).GetComponent<AwardCardControl>();
                    awardCardControls.Add(control);
                }

                awardCardControls[i].gameObject.SetActive(true);
                awardCardControls[i].UpdateUI(awards[i]);
            }
        }

        public void OnEnter()
        {
            GameManager.GameManager.Instance.bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType
                .World);
            gameObject.SetActive(false);
        }
    }
}