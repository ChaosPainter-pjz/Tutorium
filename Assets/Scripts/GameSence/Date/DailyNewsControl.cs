using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 每周要问控制器
/// </summary>
public class DailyNewsControl : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SaveObject saveObject;
    [SerializeField] private DatetimeManager datetimeManager;
    [SerializeField] private Text text;

    private DailyNews dailyNews;
    void Start()
    {
        datetimeManager.AddWeekEvent += UpdateUI;
        dailyNews = gameManager.DailyNews;
    }

    void UpdateUI()
    {
        Invoke(nameof(UpdateNews),2f);
    }

    void UpdateNews()
    {
        List<DailyNews.Row> candidates = new List<DailyNews.Row>();
        foreach (var row in dailyNews.FindAll_year(datetimeManager.DateTime.year.ToString()))
        {
            if (saveObject.SaveData.dailyNews.All(str => str!=row.id))
            {
                candidates.Add(row);
            }
        }

        if (candidates.Count!=0)
        {
            DailyNews.Row target = candidates[Random.Range(0, candidates.Count)];
            text.text = target.text;
            saveObject.SaveData.dailyNews.Add(target.id);
        }
    }
}