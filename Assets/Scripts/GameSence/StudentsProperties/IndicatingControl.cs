using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 指示图标控制器
/// </summary>
public class IndicatingControl : MonoBehaviour
{
    [SerializeField] private GameObject checkmark;
    private StudentPropertiesControl studentPropertiesControl;
    private ScoreEntryControl scoreEntryControl;
    private bool isInit = false;
    void Start()
    {
        GetComponentInParent<StudentPropertiesControl>().UIUpdateEvent += UIUpdate;
        UIUpdate();
    }

    private void UIUpdate()
    {
        if (!isInit)
        {
            studentPropertiesControl = GetComponentInParent<StudentPropertiesControl>();
            scoreEntryControl = GetComponentInParent<ScoreEntryControl>();
            isInit = true;
        }
        Grade grade = studentPropertiesControl.studentUnit.indicatingNow.Find(x => x.gradeID == scoreEntryControl.grade.gradeID);
        checkmark.SetActive(grade != null);
    }

    public void OnClick()
    {
        if (!isInit)
        {
            studentPropertiesControl = GetComponentInParent<StudentPropertiesControl>();
            scoreEntryControl = GetComponentInParent<ScoreEntryControl>();
            isInit = true;
        }
        checkmark.SetActive(!checkmark.activeSelf);
        if (studentPropertiesControl.studentUnit.indicatingNow.Find(x => x.gradeID == scoreEntryControl.grade.gradeID) == null)
        {
            if (studentPropertiesControl.studentUnit.indicatingNow.Count >= studentPropertiesControl.studentUnit.indicatingPoints)
            {
                studentPropertiesControl.studentUnit.indicatingNow.RemoveAt(0);
            }

            studentPropertiesControl.studentUnit.indicatingNow.Add(scoreEntryControl.grade);
        }
        else
        {
            studentPropertiesControl.studentUnit.indicatingNow.Remove(scoreEntryControl.grade);
        }

        GetComponentInParent<StudentPropertiesControl>().UIUpdate();
    }
}