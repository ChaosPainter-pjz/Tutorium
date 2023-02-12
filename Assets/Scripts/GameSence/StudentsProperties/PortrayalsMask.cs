using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortrayalsMask : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private StudentPropertiesControl studentPropertiesControl;
    private SpriteRenderer[] spriteRenderers;
    public void Awake()
    {
        studentPropertiesControl.UIUpdateEvent += UpdateUI;
    }

    private void UpdateUI()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (spriteRenderers==null)
        {
            UpdateUI();
        }
        foreach (var renderer in spriteRenderers)
        {
            renderer.color = new Color(1,1,1,canvasGroup.alpha) ;
        }
    }
}