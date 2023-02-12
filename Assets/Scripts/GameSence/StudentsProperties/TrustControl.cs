using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 信任度5个手手的控制器
/// </summary>
public class TrustControl : MonoBehaviour
{
    [SerializeField] private List<Image> trustList;
    private StudentUnit studentUnit;
    /// <summary>
    /// 初始化信任面板
    /// </summary>
    /// <param name="studentUnit"></param>
    public void InitTrust(StudentUnit studentUnit)
    {
        this.studentUnit = studentUnit;
        SetTrust();
    }
    /// <summary>
    /// 刷新信任值
    /// </summary>
    public void UpdateTrust()
    {
        if (studentUnit!=null)
        {
            SetTrust();

        }
    }
    /// <summary>
    /// 设置信任度
    /// </summary>
    private void SetTrust()
    {
        float mood = (studentUnit.Trust / 100f) * trustList.Count;
        for (int i = 0; i < trustList.Count; i++)
        {
            if (mood > i + 1f)
            {
                trustList[i].fillAmount = 1;
            }
            else if (mood > i)
            {
                trustList[i].fillAmount = mood - i;
            }
            else
            {
                trustList[i].fillAmount = 0;
            }
        }
    }
}