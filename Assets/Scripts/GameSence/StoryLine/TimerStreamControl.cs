using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameSence.StoryLine
{
    public class TimerStreamControl : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Text date;
        [SerializeField] private GameManager.GameManager gameManager;
        [SerializeField] private StoryLineManager storyLineManager;

        private static readonly int Steam = Animator.StringToHash("steam");

        private void OnEnable()
        {
            date.text = gameManager.saveObject.SaveData.gameDate.ToString(Unit.Date.ToStringType.YearAndSemester);
            if (gameManager.saveObject.SaveData.gameDate.Semester == 0)
                animator.SetInteger(Steam, Random.Range(3, 5));
            else
                animator.SetInteger(Steam, Random.Range(0, 3));
        }

        /// <summary>
        /// 动画结束节点事件
        /// </summary>
        private void OverPanel()
        {
            //这里要触发剧情
            if (storyLineManager.JudgmentEnforceabilityRow()) storyLineManager.BeganPlot();

            gameObject.SetActive(false);
        }

        /// <summary>
        /// 动画开始节点事件
        /// </summary>
        private void StartOverPanel()
        {
            audioSource.Play();
        }
    }
}