using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourseAttributesEntry : MonoBehaviour
{
    [SerializeField] private Text propertyName;
    [SerializeField] private Text allValue;
    [SerializeField] private Text addValue;
    public string propertyNameString;

    public enum AttributeType
    {
        Main,Interest,Other,Property
    }
    public void Init(string mName,AttributeType type)
    {
        propertyName.text = mName;
        propertyNameString = mName;
        switch (type)
        {
            case AttributeType.Main:
                propertyName.color = new Color(0.9245283f, 0.5397389f, 0);
                break;
            case AttributeType.Interest:
                propertyName.color = new Color(0.68f, 0.22f, 0.38f);

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
        gameObject.SetActive(false);
    }

    public void UIUpdate(int all,int add)
    {
        allValue.text = all==0 ? "" : all.ToString();
        addValue.text = add==0 ? "" : addValue.text = "(" + add + ")";
        gameObject.SetActive(!(all == 0 && add == 0));
    }

}