using System;
using UnityEngine;
using UnityEngine.UI;

namespace PhotographyGame
{
    /// <summary>
    /// 摄影游戏积分卡片控制器
    /// </summary>
    public class ScoreCardControl : MonoBehaviour
    {
        [SerializeField] private Image photo;
        [SerializeField] private Text score;
        [SerializeField] private Image medal;
        /// <summary>
        /// 奖章图标，0是金牌
        /// </summary>
        private Sprite[] medals;
        private PhotographyGameControl.Achievement achievement;

        public void Init(PhotographyGameControl.Achievement _achievement, Sprite[] _medals)
        {
            achievement = _achievement;
            medals = _medals;
            UIUpdate();
        }

        private void UIUpdate()
        {
            photo.sprite = achievement.Sprite;
            score.text = achievement.IntValue.ToString();
            medal.sprite = medals[achievement.Appraise];
        }

    }
}