using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorldGame
{
    public class RankingCardControl : MonoBehaviour
    {
        [SerializeField] private Text studentName;
        [SerializeField] private Text score;
        [SerializeField] private GameObject emphasis;

        public void UIUpdate(Ranking ranking)
        {
            studentName.text = ranking.Name;
            score.text = ranking.Score.ToString();
            emphasis.SetActive(ranking.IsEmphasis);
        }
    }
}