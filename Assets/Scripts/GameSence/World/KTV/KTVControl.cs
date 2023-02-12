using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using World.KTV;

/// <summary>
/// KTV的管理器
/// </summary>
public class KTVControl : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private Text money;
    [SerializeField] private SubscribeButtonControl[] aSubscribes;
    [SerializeField] private SubscribeButtonControl[] bSubscribes;
    [SerializeField] private SubscribeButtonControl[] cSubscribes;

    [SerializeField] private KTVEnterPanelControl enterPanelControl;
    //已用UnityAction<int>代替
    //public delegate void KtvButton(int buttonNumber);
    private void OnEnable()
    {
        gameManager.bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType.Ktv);
        UpdateUI();
    }

    private void UpdateUI()
    {

        money.text = moneyManager.Money.ToString();
        for (int i = 0; i < aSubscribes.Length; i++)
        {
            bool isBuy6 = gameManager.saveObject.SaveData.studentUnits.Any(unit => unit.schedule[i].id == "42");
            bool isBuy0 = gameManager.saveObject.SaveData.studentUnits.Any(unit => unit.schedule[i+3].id == "42");
            aSubscribes[i].UpdateUI(!(isBuy0 || isBuy6),i,OnButton);
            bSubscribes[i].UpdateUI(!(isBuy0 || isBuy6),i,OnButton);
            cSubscribes[i].UpdateUI(!(isBuy0 || isBuy6),i,OnButton);
        }
    }

    /// <summary>
    /// 按下预约按钮
    /// </summary>
    /// <param name="ButtonNumber">按钮的序号</param>
    /// <param name="price">预约价格</param>
    public void OnButton(int ButtonNumber,int price)
    {
        //显示选择面板
        enterPanelControl.OnPanel(price,ButtonNumber,UpdateUI);

    }

    public void OnExit()
    {
        gameManager.bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
        gameObject.SetActive(false);
    }
}