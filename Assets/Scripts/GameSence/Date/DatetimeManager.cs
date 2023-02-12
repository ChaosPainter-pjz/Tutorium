using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DatetimeManager : MonoInstance<DatetimeManager>
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Text datetimeText;
    [SerializeField] private Text weekText;
    [SerializeField] private GameObject[] autumn;
    /// <summary>
    /// 日期标志刷新后事件
    /// </summary>
    public event UnityAction AddDateEvent;
    /// <summary>
    /// 周数变化后时间
    /// </summary>
    public event UnityAction AddWeekEvent;
    public Date DateTime
    {
        get => gameManager.saveObject.SaveData.gameDate;
        set => gameManager.saveObject.SaveData.gameDate = value;
    }
    /// <summary>
    /// 上周的周数
    /// </summary>
    private int lastWeek=0;
    /// <summary>
    /// 初始年份
    /// </summary>
    public int InitYear => gameManager.saveObject.SaveData.InitYear;
    /// <summary>
    /// 年级
    /// </summary>
    public string Grade => Equation.CaToCh[DateTime.year - InitYear + 1 ];

    /// <summary>
    /// 添加指定的日子
    /// </summary>
    /// <param name="i"></param>
    public void DateTimeUpdate(int i)
    {
        DateTime.WhatDay += i;
        UIUpdate();
        AddTimeEvent();
    }

    private void Start()
    {
        UIUpdate();
        AddTimeEvent();
    }

    private void UIUpdate()
    {
        datetimeText.text = DateTime.ToString();
        weekText.text=DateTime.ToString(Date.ToStringType.Week);
        switch (DateTime.Semester)
        {
            case 0:
                if (DateTime.Week < Date.MaxWeek/2)
                {
                    autumn[0].SetActive(true);
                    autumn[1].SetActive(false);
                    autumn[2].SetActive(false);
                    autumn[3].SetActive(false);
                }
                else
                {
                    autumn[0].SetActive(false);
                    autumn[1].SetActive(true);
                    autumn[2].SetActive(false);
                    autumn[3].SetActive(false);
                }
                break;
            case 1:
                if (DateTime.Week < Date.MaxWeek/2)
                {
                    autumn[0].SetActive(false);
                    autumn[1].SetActive(false);
                    autumn[2].SetActive(true);
                    autumn[3].SetActive(false);
                }
                else
                {
                    autumn[0].SetActive(false);
                    autumn[1].SetActive(false);
                    autumn[2].SetActive(false);
                    autumn[3].SetActive(true);
                }
                break;
        }

    }

    private void AddTimeEvent()
    {
        AddDateEvent?.Invoke();

        if (lastWeek!=DateTime.Week)
        {
            AddWeekEvent?.Invoke();
        }

        lastWeek = DateTime.Week;
    }
}