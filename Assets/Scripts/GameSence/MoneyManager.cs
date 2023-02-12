using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MoneyManager : MonoInstance<MoneyManager>
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Text moneyNumber;

    /// <summary>
    /// 安全的设置金钱数
    /// </summary>
    public int Money
    {
        set
        {
            gameManager.saveObject.SaveData.money = value;
            UpdateMoney();
        }
        get => gameManager.saveObject.SaveData.money;
    }

    /// <summary>
    /// 刷新金钱UI显示
    /// </summary>
    private void UpdateMoney()
    {
        moneyNumber.text = Money.ToString();
    }

    private void Start()
    {
        UpdateMoney();
    }
}