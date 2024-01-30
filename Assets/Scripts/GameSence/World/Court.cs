using Basic.CSV2Table.Plot;
using GameSence.Hint;
using GameSence.StoryLine;
using SaveManager.Scripts;
using UnityEngine;

namespace GameSence.World
{
    /// <summary>
    /// 球场
    /// </summary>
    public class Court : MonoBehaviour
    {
        [SerializeField] private GameManager.GameManager gameManager;
        [SerializeField] private StoryLineManager storyLineManager;
        private PlotJudgmentList otherPlotJudgment;
        private SaveData saveData;

        public void OnButton()
        {
            saveData ??= gameManager.saveObject.SaveData;
            otherPlotJudgment ??= gameManager.OtherPlotJudgmentList;
            if (saveData.parkDate.Week == saveData.gameDate.Week)
            {
                //此时为重复访问，每周只能访问一个奖励地点
                HintManager.Instance.AddHint(new Hint.Hint("广场舞时间", "天色已晚，球场已被舞蹈团占领。下周再来吧！"));
            }
            else
            {
                saveData.parkDate.Week = saveData.gameDate.Week;
                var range = Random.Range(0, 100);
                if (range == 1)
                {
                    storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("court4"));
                    foreach (var studentUnit in saveData.studentUnits)
                    {
                        //studentUnit.Physique.score += 1;
                        studentUnit.Mood += 10;
                        studentUnit.interestGrade.Find(x => x.gradeID == "21").score += 30;
                        studentUnit.interestGrade.Find(x => x.gradeID == "11").score += 5;
                    }
                }
                else if (range < 15)
                {
                    storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("court3"));
                    foreach (var studentUnit in saveData.studentUnits)
                    {
                        studentUnit.Physique.score += 1;
                        studentUnit.Mood += 5;
                        studentUnit.interestGrade.Find(x => x.gradeID == "21").score += 10;
                    }
                }
                else if (range < 40)
                {
                    storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("court2"));
                    foreach (var studentUnit in saveData.studentUnits)
                    {
                        studentUnit.Physique.score += 1;
                        studentUnit.Mood += 2;
                        studentUnit.interestGrade.Find(x => x.gradeID == "21").score += 6;
                    }
                }
                else
                {
                    storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("court1"));
                    foreach (var studentUnit in saveData.studentUnits)
                    {
                        studentUnit.Physique.score += 1;
                        studentUnit.interestGrade.Find(x => x.gradeID == "21").score += 6;
                    }
                }

                storyLineManager.BeganPlot();
            }
        }
    }
}
