using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 班级消息面板中消息卡片的控制器
/// </summary>
public class HintCardControl : MonoBehaviour
{
    [SerializeField] private Text date;
    [SerializeField] private Text heading;
    [SerializeField] private Text text;

    public void Init(Hint hint)
    {
        gameObject.SetActive(true);
        date.text = hint.Date.ToString(Date.ToStringType.Long);
        heading.text = hint.Headline;
        text.text = hint.Text;
    }
}