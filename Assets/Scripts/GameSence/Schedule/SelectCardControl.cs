using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectCardControl : MonoBehaviour
{
    //当前按钮持有的课程
    public CourseList.Row row;
    private SelectManager selectManager;

    [SerializeField]
    private Text courseName;
    public void Init(CourseList.Row row)
    {
        selectManager = SelectManager.Instance;

        this.row = row;
        courseName.text = row.Name;
        gameObject.name = row.Name;

        gameObject.SetActive(false);
    }
    /// <summary>
    /// 按下鼠标，开始拖拽
    /// </summary>
    public void OnPointerDown()
    {
        selectManager.OnPointerDown(row,transform.position);
    }
    /// <summary>
    /// 点击打开详情面板
    /// </summary>
    public void OpenParticularsPanel()
    {
        selectManager.OpenParticularsPanel(row);
    }
}