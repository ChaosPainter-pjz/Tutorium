using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatingPointsControl : MonoBehaviour
{
    [SerializeField] private Text number;
    [SerializeField] private List<IndicatingControl> indicatingList;
    public List<string> indicatingNow;

    private void Start()
    {
        GetComponentInParent<StudentPropertiesControl>().UIUpdateEvent += UIUpdate;
        UIUpdate();
    }

    private void UIUpdate()
    {
        StudentUnit studentUnit = GetComponentInParent<StudentPropertiesControl>().studentUnit;
        number.text = ( studentUnit.indicatingPoints- studentUnit.indicatingNow.Count).ToString();
    }
}