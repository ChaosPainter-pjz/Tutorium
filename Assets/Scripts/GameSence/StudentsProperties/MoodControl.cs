using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodControl : MonoBehaviour
{
    [SerializeField] private Image[] moodList;
    private float speed;
    private float lowValue;
    private float newValue;
    /// <summary>
    /// 直接设置心情
    /// </summary>
    public void UpdateUI(float studentMood)
    {
        float mood = studentMood / 20f;
        for (int i = 0; i < moodList.Length; i++)
        {
            if (mood > i + 1f)
            {
                moodList[i].fillAmount = 1;
            }
            else if (mood > i)
            {
                moodList[i].fillAmount = mood - i;
            }
            else
            {
                moodList[i].fillAmount = 0;
            }
        }
    }

    /// <summary>
    /// 带动画的设置心情
    /// </summary>
    public void DoubleSet(int lowMood,int newMood)
    {
        lowValue = lowMood;
        newValue = newMood;
        //Mathf.SmoothDamp(lowMood, newMood, ref speed, 1);
        speed = 0;

    }

    private void Update()
    {
        if (Mathf.Abs(newValue-lowValue) <1f)
        {
            UpdateUI(newValue);
        }
        else
        {
            lowValue =  Mathf.SmoothDamp(lowValue, newValue, ref speed, 1f);
            UpdateUI(lowValue);
        }
    }
}