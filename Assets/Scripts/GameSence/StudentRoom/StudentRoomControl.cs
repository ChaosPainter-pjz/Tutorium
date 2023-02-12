using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 学生房间控制器
/// </summary>
public class StudentRoomControl : MonoBehaviour
{
    public StudentUnit studentUnit;
    [SerializeField] private GameObject backpackPanel;
    [SerializeField] private GameObject enterPanel;
    [SerializeField] private Text studentName;
    [SerializeField] private BackpackControl backpackControl;
    //[SerializeField] private GameObject enterPanel;
    /// <summary>
    /// 游戏主场景UI
    /// </summary>
    [SerializeField]
    private GameObject mainPanel;
    public void Init(StudentUnit studentUnit)
    {
        gameObject.SetActive(true);
        //mainPanel.transform.localScale = Vector3.zero;
        this.studentUnit = studentUnit;
        backpackPanel.SetActive(false);
        enterPanel.SetActive(false);
        //UpdateUI();
        //UpdateScene();
        OnBackpack();
    }

    public void UpdateUI()
    {
        studentName.text = studentUnit.fullName;
        backpackControl.gameObject.SetActive(false);
    }

    public void UpdateScene()
    {

    }

    private void OnDisable()
    {
        backpackPanel.SetActive(false);
        enterPanel.SetActive(false);
        //mainPanel.transform.localScale = Vector3.one;
    }
    /// <summary>
    /// 打开背包
    /// </summary>
    public void OnBackpack()
    {
        backpackControl.Init(studentUnit);
    }

    public void ExitPanel()
    {
        if (enterPanel.activeSelf)
        {
            enterPanel.GetComponent<Animator>().Play("ExitPanel");
        }
        else
        {
            GetComponent<Animator>().Play("StudentPropertiesExitPanel");
        }
    }

    public void Exit()
    {
        GetComponentInChildren<CanvasGroup>().alpha = 1;
        gameObject.SetActive(false);
    }

}