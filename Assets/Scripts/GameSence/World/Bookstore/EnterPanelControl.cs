using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bookstore
{

/// <summary>
/// 确认订阅面板控制器
/// </summary>
public class EnterPanelControl : MonoBehaviour
{

    [SerializeField] private Text tradeName;
    [SerializeField] private Text tradePrice;
    [SerializeField] private Button enterButton;
    [SerializeField] private MainButtonControl mainButtonControl;
    private StoreGoodsList.Row row;
    /// <summary>
    /// 打开该面板
    /// </summary>
    public void OnPanel(StoreGoodsList.Row row)
    {
        this.row = row;
        tradeName.text = row.name;
        tradePrice.text = row.price;
        gameObject.SetActive(true);
        enterButton.interactable = MoneyManager.Instance.Money >= int.Parse(row.price);
    }
    /// <summary>
    /// 确认订阅
    /// </summary>
    public void OnEnter()
    {
        BookstoreManager.Instance.Money -= int.Parse(row.price);
        CourseList courseList = GameManager.Instance.CourseList;
        string courseName = courseList.Find_Id(row.courseID).Name;
        StudentCourse.Type type = courseList.Find_Id(row.courseID).Type switch
        {
            "M" => StudentCourse.Type.Main,
            "I" => StudentCourse.Type.Interest,
            "O" => StudentCourse.Type.Other,
            _ => throw new ArgumentOutOfRangeException()
        };
        GameManager.Instance.saveObject.SaveData.playerUnit.masterStudentCourse.Add(
            new StudentCourse(row.courseID,courseName,type)
        );
        HintManager.Instance.AddHint(new Hint("订阅完成",$"成功订阅“{row.name}”,可以在“学生日程”中找到他！"));
        mainButtonControl.IsRemindSchedule = true;
        gameObject.SetActive(false);//关闭确认页面
        BookstoreManager.Instance.UpdateUI();
    }
}
}