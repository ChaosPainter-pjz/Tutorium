using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bookstore
{

public class BookstoreManager : MonoInstance<BookstoreManager>
{
    [SerializeField]
    private Text moneyNumber;
    public int Money
    {
        get
        {
            return MoneyManager.Instance.Money;
        }
        set
        {
            MoneyManager.Instance.Money = value;
            MoneyUpdate();
        }
    }
    /// <summary>
    /// 商品卡片预制体
    /// </summary>
    public GameObject commodityUnit;
    [SerializeField] private Transform parent;
    [SerializeField] private EnterPanelControl enterPanelControl;
    private StoreGoodsList storeGoodsList;
    private List<StudentCourse> masterCourses;
    public event UnityAction UIUpdateEvent;

    public override void Awake()
    {
        base.Awake();
        GameManager.Instance.InitGameEvent += Init;
    }

    private void Init()
    {
        MoneyUpdate();
        storeGoodsList = GameManager.Instance.StoreGoodsList;
        masterCourses = GameManager.Instance.saveObject.SaveData.playerUnit.masterStudentCourse;
        foreach (StoreGoodsList.Row row in storeGoodsList.GetRowList())
        {
            GameObject obj = Instantiate(commodityUnit, parent);
            obj.GetComponent<CommmodityUnitControl>().Init(row,masterCourses);
        }


    }
    /// <summary>
    /// 更新存款的显示
    /// </summary>
    private void MoneyUpdate()
    {
        moneyNumber.text = Money.ToString();
    }
    /// <summary>
    /// 点击了订阅按钮
    /// </summary>
    /// <param name="row"></param>
    public void OnSubscription(StoreGoodsList.Row row)
    {
        enterPanelControl.OnPanel(row);
    }
    /// <summary>
    /// 刷新页面的显示
    /// </summary>
    public void UpdateUI()
    {
        UIUpdateEvent?.Invoke();
        MoneyUpdate();
    }
}
}