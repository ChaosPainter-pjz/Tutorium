using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 是个角色都有的基础数据脚本
/// </summary>
[Serializable]
public abstract class Unit
{
    [SerializeField] public string id = "-1";

    /// <summary>
    /// 全名
    /// </summary>
    [SerializeField] public string fullName = "无名";

    /// <summary>
    /// 生日
    /// </summary>
    [SerializeField] public Date birthday = new Date(1, 0, 1, 0);

    /// <summary>
    /// 性别
    /// </summary>
    [SerializeField] public Gender gender = Gender.None;

    /// <summary>
    /// 个人资料
    /// </summary>
    [SerializeField] public string personalData = "无";
    /// <summary>
    /// 返回这位学生的性别
    /// </summary>
    /// <param name="isTa">是否是第三人称</param>
    /// <returns></returns>
    public string GetGenderString(bool isTa=false)
    {
        if (!isTa)
        {
            return gender switch
            {
                Gender.None => "未知",
                Gender.Man => "男",
                Gender.Woman => "女",
                _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
            };
        }
        else
        {
            return gender switch
            {
                Gender.None => "它",
                Gender.Man => "他",
                Gender.Woman => "她",
                _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
            };
        }

    }
}