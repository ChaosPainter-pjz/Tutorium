using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源管理器，某些重复利用且不方便挂载的资源，在这里获取
/// </summary>
public class ResourceManager : MonoInstance<ResourceManager>
{
    /// <summary>
    /// npc的头像数组，.name为对应对象的id
    /// </summary>
    [Header("npc头像")]
    public Sprite[] npcHeadPortraits;
    [Header("cg")]
    public Sprite[] PhotoAlbum;
    [Header("物品图")] public Sprite[] articleList;
    [Header("走时间过场图")] public Sprite[] interludes;
    /// <summary>
    /// 学生头像，按照ID排序
    /// </summary>
    [Header("学生头像")] public Sprite[] studentHeadPortrait;
    /// <summary>
    /// 摄影游戏可用的背景图
    /// </summary>
    [Header("摄影小游戏所用到的背景图")] public Sprite[] photographyGame;

    [Header("场景")] public Sprite[] scenes;
    public void InitData()
    {
        // npcHeadPortraits = Resources.LoadAll<Sprite>("npcHeadPortraits");
        // if (npcHeadPortraits == null)
        // {
        //     Debug.LogError("npcHeadPortraits未读取到内容");
        // }

    }
}