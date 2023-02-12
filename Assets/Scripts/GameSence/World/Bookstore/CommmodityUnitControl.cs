using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bookstore
{

public class CommmodityUnitControl : MonoBehaviour
{
    [SerializeField] private Text commodityName;
    [SerializeField] private Text commodityDescription;
    [SerializeField] private Text descriptionPrice;
    [SerializeField] private Text commodityDescription2;
    [SerializeField] private Text manufacturerName;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject subscriptionPrompt;
    private List<StudentCourse> masterCourses;//玩家所拥有的课程列表
    private StoreGoodsList.Row row;
    /// <summary>
    /// true：已购买  false:可购买
    /// </summary>
    private bool isBuy=false;

    public void Init(StoreGoodsList.Row row,List<StudentCourse> masterCourses)
    {
        this.masterCourses = masterCourses;
        this.row = row;
        Course course = masterCourses.Find(x => x.id == row.courseID);
        isBuy = course != null;
        commodityName.text = row.name;
        commodityDescription.text = row.description;
        CourseList.Row courseRow = GameManager.Instance.CourseList.Find_Id(row.courseID);
        if (courseRow==null)
        {
            Debug.Log("错误的row.courseID："+row.courseID);
        }
        else
        {
            if (courseRow.Type == "O")
            {
                commodityDescription2.text = $"订阅后可解锁“{GameManager.Instance.CourseList.Find_Id(row.courseID).Name}”外出项目。";
            }
            else
            {
                commodityDescription2.text = $"订阅后可解锁“{GameManager.Instance.CourseList.Find_Id(row.courseID).Name}”兴趣课程。";
            }
        }

        descriptionPrice.text = row.price;
        manufacturerName.text = row.manufacturerName;

        BookstoreManager.Instance.UIUpdateEvent += UIUpdate;
        //buyButton.GetComponent<Button>().onClick.AddListener(OnClick);
        UIUpdate();
    }

    private void UIUpdate()
    {
        Course course = masterCourses.Find(x => x.id == row.courseID);
        isBuy = course != null;
        buyButton.SetActive(!isBuy);
        subscriptionPrompt.SetActive(isBuy);
    }
    /// <summary>
    /// 点击了订阅按钮
    /// </summary>
    public void OnClick()
    {
        //跳出二次确认面板
        BookstoreManager.Instance.OnSubscription(row);
    }

}
}