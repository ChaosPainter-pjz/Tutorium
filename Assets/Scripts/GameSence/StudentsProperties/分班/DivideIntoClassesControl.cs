using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 高考分科面板控制器
/// </summary>
public class DivideIntoClassesControl : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Text remainingWeeks;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardParent;
    private List<StudentUnit> studentUnits;
    private List<DivideStudentCardControl> cardControls;
    /// <summary>
    /// 分班的周数
    /// </summary>
    public static int DivideIntoClassesWeek = 5;

    public bool isDivideIntoClasses = false;
    private void OnEnable()
    {
        studentUnits ??= gameManager.saveObject.SaveData.studentUnits;
        cardControls ??= new List<DivideStudentCardControl>();
        isDivideIntoClasses = true;
        InitUI();
    }

    private void InitUI()
    {
        //设置剩余周数
        remainingWeeks.text = $"{DivideIntoClassesWeek-gameManager.saveObject.SaveData.gameDate.Week}周";

        foreach (var control in cardControls)
        {
            control.gameObject.SetActive(false);
        }

        while (cardControls.Count<studentUnits.Count)
        {
            var control = Instantiate(cardPrefab, cardParent).GetComponent<DivideStudentCardControl>();
            cardControls.Add(control);
        }

        for (int i = 0; i < studentUnits.Count; i++)
        {
            cardControls[i].Init(studentUnits[i]);
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < studentUnits.Count; i++)
        {
            cardControls[i].UpdateUI();
        }
    }

    public void DivideIntoClasses()
    {
        if (cardControls == null)
        {
            OnEnable();
        }
        foreach (var control in cardControls)
        {
            control.ChooseSubject();
        }
    }
}