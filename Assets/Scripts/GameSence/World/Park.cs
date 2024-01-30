using Basic.CSV2Table.Plot;
using GameSence.Hint;
using GameSence.StoryLine;
using SaveManager.Scripts;
using Unit;
using UnityEngine;

namespace GameSence.World
{
    public class Park : MonoBehaviour
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
                //此时为重复访问公园
                HintManager.Instance.AddHint(new Hint.Hint("已闭园", "天色已晚，公园已经关闭。下周再来吧！"));
            }
            else
            {
                saveData.parkDate.Week = saveData.gameDate.Week;
                var range = Random.Range(0, 100);
                if (range < 50)
                {
                    storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("park1"));
                    foreach (var unit in saveData.studentUnits) unit.Mood += 5;
                }
                else if (range < 70)
                {
                    storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("park2"));
                    foreach (var unit in saveData.studentUnits) unit.Mood += 10;
                }
                else if (range < 85)
                {
                    storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("park3"));
                    foreach (var unit in saveData.studentUnits)
                    {
                        unit.Mood += 6;
                        var grade = unit.interestGrade.Find(x => x.gradeID == "20");
                        grade.score += 10;
                    }
                }
                else if (range < 95)
                {
                    storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("park4"));
                    foreach (var unit in saveData.studentUnits)
                    {
                        unit.Mood += 8;
                        var grade = unit.interestGrade.Find(x => x.gradeID == "18");
                        grade.score += 10;
                    }
                }
                else
                {
                    storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("park5"));
                    foreach (var unit in saveData.studentUnits)
                    {
                        unit.Mood += 30;
                        var grade = unit.mainGrade.Find(x => x.gradeID == "0");
                        grade.score += 20;
                    }
                }

                storyLineManager.BeganPlot();
            }
        }
    }
}
