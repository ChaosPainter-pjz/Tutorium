using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenModeControl : MonoBehaviour
{
    [SerializeField]private Dropdown dropdown;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetSystemMetrics(int nIndex);
    private static int SM_CXSCREEN = 0; //主屏幕分辨率宽度
    private static int SM_CYSCREEN = 1; //主屏幕分辨率高度
    private static int SM_CYCAPTION = 4; //标题栏高度
    private static int SM_CXFULLSCREEN = 16; //最大化窗口宽度（减去任务栏）
    private static int SM_CYFULLSCREEN = 17; //最大化窗口高度（减去任务栏）

    public void Init()
    {
        dropdown.value = SaveManager.GetScreenMode();
    }

    public void SetMode()
    {
        SaveManager.SetScreenMode(dropdown.value);
        switch (dropdown.value)
        {
            case 0:
                Screen.SetResolution(GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN), FullScreenMode.FullScreenWindow);
                break;
            case 1:
                Screen.SetResolution(GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN), FullScreenMode.ExclusiveFullScreen);
                //Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2:
                //Screen.SetResolution(GetSystemMetrics(SM_CXFULLSCREEN), GetSystemMetrics(SM_CYFULLSCREEN), FullScreenMode.MaximizedWindow);
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 3:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }
    private void Test()
    {
        // 屏幕分辨率
        int x = GetSystemMetrics(SM_CXSCREEN);
        int y = GetSystemMetrics(SM_CYSCREEN);

        // 屏幕WorkingArea
        int x1 = GetSystemMetrics(SM_CXFULLSCREEN);
        int y1 = GetSystemMetrics(SM_CYFULLSCREEN);

        // 标题栏高度
        int title = GetSystemMetrics(SM_CYCAPTION);

        // 不最大化、不全屏的最大窗口高度
        int maxHeight = y1 - title;
    }
}