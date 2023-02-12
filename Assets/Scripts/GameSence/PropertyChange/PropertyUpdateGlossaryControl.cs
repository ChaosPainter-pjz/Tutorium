using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyUpdateGlossaryControl : MonoBehaviour
{
    [SerializeField] private Text propertyName;

    /// <summary>
    /// 原来的值
    /// </summary>
    [SerializeField] private Text BeforeValue;

    /// <summary>
    /// 新的值
    /// </summary>
    /// <returns></returns>
    [SerializeField] private Text NewValue;

    /// <summary>
    /// 剔除教师技能的变化值
    /// </summary>
    [SerializeField] private Text ChangingValue;

    /// <summary>
    /// 教师技能附加的变化值
    /// </summary>
    [SerializeField] private Text SkillValue;

    public string entryName;

    public enum AttributeType
    {
        Main,Interest,Other,Property
    }
    public void Init(string entryName,AttributeType type)
    {
        this.entryName = entryName;
        this.propertyName.text = entryName + "  ";
        switch (type)
        {
            case AttributeType.Main:
                propertyName.color = new Color(0.9245283f, 0.5397389f, 0);
                break;
            case AttributeType.Interest:
                propertyName.color = new Color(0.1435386f, 0.548781f, 0.6792453f);
                break;
            case AttributeType.Other:
                propertyName.color = new Color(0, 0, 0);
                break;
            case AttributeType.Property:
                propertyName.color = new Color(0.06162617f, 0.5754717f, 0);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

    }

    ///  <summary>
    /// 更新括号里的内容
    ///  </summary>
    public void UIUpdate(int lowValue,int newValue, ExecuteControl.Location location)
    {

        switch (location)
        {
            case ExecuteControl.Location.Low2New:
            {
                BeforeValue.text = lowValue.ToString();
                if (lowValue>newValue)
                {
                    NewValue.color = Color.red;
                }else
                {
                    NewValue.color = new Color(0.2f,0.82f,0.2f);
                }
                NewValue.text = newValue.ToString();

                ChangingValue.text = "(0)";
                SkillValue.text = "(0)";
                if (lowValue == newValue)
                {
                    gameObject.SetActive(false);
                }
                break;
            }
            case ExecuteControl.Location.Low2Class:
                if (newValue == 0)
                {
                    ChangingValue.text = "";
                    break;
                }

                ChangingValue.text = "(" + (newValue > 0 ? "+" + newValue : newValue.ToString()) + ")";
                gameObject.SetActive(true);
                break;
            case ExecuteControl.Location.Class2Player:
                if (newValue == 0)
                {
                    SkillValue.text = "";
                    break;
                }
                SkillValue.text = "(" + (newValue > 0 ? "+" + newValue : newValue.ToString()) + ")";
                gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(location), location, null);
        }
    }

    public void UIUpdate(int newValue, ExecuteControl.Location location)
    {
        UIUpdate(0, newValue, location);
    }
}