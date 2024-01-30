using GameSence.StoryLine;
using UnityEngine;

namespace GameSence.Date
{
    public class DateAnimEvent : MonoBehaviour
    {
        [SerializeField] private StoryLineManager storyLineManager;
        [SerializeField] private GameManager.GameManager gameManager;
        [SerializeField] private DatetimeManager datetimeManager;
        private bool isFirstTime = true;

        public void StartStoryLine()
        {
            if (gameManager.saveObject.SaveData.gameDate == new Unit.Date(datetimeManager.InitYear, 0, 1, 0))
                isFirstTime = false;
            if (isFirstTime)
                isFirstTime = false;
            //Debug.Log("初次");
            else
                storyLineManager.StartPlotJudgment();
        }
    }
}