using System;
using UnityEngine;

namespace World
{
    /// <summary>
    /// 朋克城控制器
    /// </summary>
    public class CoexControl : MonoBehaviour
    {
        private void OnEnable()
        {
            AudioControl.Instance.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
        }
        /// <summary>
        /// 点击了返回按钮
        /// </summary>
        public void OnReturn()
        {
            AudioControl.Instance.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
            gameObject.SetActive(false);
        }
    }
}