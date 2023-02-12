using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 成绩单页面，单个学科成绩词条的控制器
/// </summary>
public class TranscriptGradeCardControl : MonoBehaviour
{
    [SerializeField] private Text gradeName;
    [SerializeField] private Text gradeScore;
    public void UpdateUI(Grade grade)
    {
        gameObject.SetActive(true);
        gradeName.text = grade.name;
        gradeScore.text = grade.GradeScore.ToString();
    }
}