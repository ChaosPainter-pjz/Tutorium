using System;
using UnityEngine;

namespace World
{
    /// <summary>
    /// 朋克城控制器
    /// </summary>
    public class MarketControl : MonoBehaviour
    {
        private void OnEnable()
        {
            AudioControl.Instance.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
        }
    }
}