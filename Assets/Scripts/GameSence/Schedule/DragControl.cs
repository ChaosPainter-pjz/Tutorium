using System.Collections;
using System.Collections.Generic;
using Basic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选课界面，拖拽物的控制器
/// </summary>
public class DragControl : MonoBehaviour
{
    [SerializeField] private GameObject[] image;
    [SerializeField] private Text text;

    /// <summary>
    /// 当前拖拽的课程
    /// </summary>
    private StudentCourse course;

    public void UpdateUI(CourseList.Row _course)
    {
        course = _course.Row2StudentCourse();
        text.text = course.name;
        image[0].SetActive(course.type==StudentCourse.Type.Main);
        image[1].SetActive(course.type==StudentCourse.Type.Interest);
        image[2].SetActive(course.type==StudentCourse.Type.Other);
    }
}