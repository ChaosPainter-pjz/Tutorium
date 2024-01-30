using UnityEngine;
using UnityEngine.UI;

namespace GameSence.World.Game
{
    /// <summary>
    /// 奖励界面的奖励控制器
    /// </summary>
    public class AwardCardControl : MonoBehaviour
    {
        [SerializeField] private Text cardName;
        [SerializeField] private Text score;

        public void UpdateUI(Award award)
        {
            cardName.text = award.CardName;
            score.text = "+" + award.Score;
        }
    }
}