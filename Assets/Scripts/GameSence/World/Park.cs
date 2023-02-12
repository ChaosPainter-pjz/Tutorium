using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Park : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private StoryLineManager storyLineManager;
    [SerializeField] private PlotJudgmentList otherPlotJudgment;
    private SaveData saveData;

    public void OnButton()
    {
        saveData ??= gameManager.saveObject.SaveData;
        otherPlotJudgment ??= gameManager.OtherPlotJudgmentList;
        if (saveData.parkDate.Week==saveData.gameDate.Week)
        {
            //此时为重复访问公园
            HintManager.Instance.AddHint(new Hint("已闭园","天色已晚，公园已经关闭。下周再来吧！"));
        }
        else
        {
            saveData.parkDate.Week = saveData.gameDate.Week;
            int range = Random.Range(0, 100);
            if (range<50)
            {
                storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("park1"));
                foreach (var unit in saveData.studentUnits)
                {
                    unit.Mood += 5;
                }

            }else if (range<70)
            {
                storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("park2"));
                foreach (var unit in saveData.studentUnits)
                {
                    unit.Mood += 10;
                }
            }else if (range<85)
            {
                storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("park3"));
                foreach (var unit in saveData.studentUnits)
                {
                    unit.Mood += 6;
                    Grade grade = unit.interestGrade.Find(x => x.gradeID == "20");
                    grade.score += 10;
                }
            }else if (range<95)
            {
                storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("park4"));
                foreach (var unit in saveData.studentUnits)
                {
                    unit.Mood += 8;
                    Grade grade = unit.interestGrade.Find(x => x.gradeID == "18");
                    grade.score += 10;
                }
            }
            else
            {
                storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("park5"));
                foreach (var unit in saveData.studentUnits)
                {
                    unit.Mood += 30;
                    Grade grade = unit.mainGrade.Find(x => x.gradeID == "0");
                    grade.score += 20;
                }
            }
            storyLineManager.BeganPlot();
        }
    }
}