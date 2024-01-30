using UnityEngine;
using UnityEngine.UI;

namespace GameSence.World.Game
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