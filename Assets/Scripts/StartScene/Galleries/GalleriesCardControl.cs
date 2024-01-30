using System;
using Basic.CSV2Table.GameOver;
using SaveManager.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace StartScene.Galleries
{
    /// <summary>
    /// 追忆卡片控制器
    /// </summary>
    public class GalleriesCardControl : MonoBehaviour
    {
        [SerializeField] private Text state;
        [SerializeField] private GameObject details;
        [SerializeField] private Text gameOverName;
        [SerializeField] private Text playerName;
        [SerializeField] private Text studentName;
        [SerializeField] private Text time;

        public void Init(OverUnit unit, GameOverList.Row row)
        {
            state.text = row.Level switch
            {
                "0" => "X",
                "1" => "C",
                "2" => "B",
                "3" => "B+",
                "4" => "A",
                "5" => "S",
                _ => throw new ArgumentOutOfRangeException()
            };
            gameOverName.text = row.EndName;
            playerName.text = unit.playerName;
            studentName.text = unit.studentName;
            time.text = unit.dateTime.ToString("yyyy/MM/dd");
        }
    }
}