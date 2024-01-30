using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GameSence.Setting
{
    public class SettingMainControl : MonoBehaviour
    {
        public enum ControlType
        {
            SetGlobalVolume,
            Music,
            Sound
        }

        private string typeString
        {
            get
            {
                return type switch
                {
                    ControlType.SetGlobalVolume => "MainVolume",
                    ControlType.Music => "MusicVolume",
                    ControlType.Sound => "SoundVolume",
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        public ControlType type;
        [Header("Switch")] [SerializeField] private Image openImage;
        [SerializeField] private Image closeImage;
        [SerializeField] private Transform square;
        [SerializeField] private GameObject openText;
        [SerializeField] private GameObject closeText;
        [Header("滑动条")] [SerializeField] private Slider slider;
        [Header("")] [SerializeField] private AudioMixer audioMixer;

        public void Init()
        {
            slider.value = SaveManager.Scripts.SaveManager.GetVolume(typeString);
            UpdateToggle(typeString);
            if (SaveManager.Scripts.SaveManager.GetIsVolume(typeString) == "true")
                audioMixer.SetFloat(typeString, SaveManager.Scripts.SaveManager.GetVolume(typeString));
            else
                audioMixer.SetFloat(typeString, -100);
        }

        /// <summary>
        /// 设置音量值
        /// </summary>
        public void SetValue(float volume)
        {
            //Debug.Log(SaveManager.GetIsVolume(typeString));
            if (SaveManager.Scripts.SaveManager.GetIsVolume(typeString) == "true")
                audioMixer.SetFloat(typeString, volume);
            SaveManager.Scripts.SaveManager.SetVolume(typeString, volume);
            UpdateToggle(typeString);
        }

        /// <summary>
        /// 反转开关
        /// </summary>
        public void InversionToggle()
        {
            var volume = SaveManager.Scripts.SaveManager.GetIsVolume(typeString);
            volume = volume == "true" ? "false" : "true";
            SaveManager.Scripts.SaveManager.SetIsVolume(typeString, volume);
            var value = volume == "true" ? SaveManager.Scripts.SaveManager.GetVolume(typeString) : -100f;
            if (volume == "true") slider.value = SaveManager.Scripts.SaveManager.GetVolume(typeString);
            audioMixer.SetFloat(typeString, value);
            UpdateToggle(typeString);
        }

        private void UpdateToggle(string typeName)
        {
            var value = SaveManager.Scripts.SaveManager.GetIsVolume(typeName);
            if (value == "true")
            {
                openImage.gameObject.SetActive(true);
                closeImage.gameObject.SetActive(false);
                openText.SetActive(true);
                closeText.SetActive(false);
                square.localPosition = new Vector3(-49, 0, 0);
            }
            else
            {
                openImage.gameObject.SetActive(false);
                closeImage.gameObject.SetActive(true);
                openText.SetActive(false);
                closeText.SetActive(true);
                square.localPosition = new Vector3(49, 0, 0);
            }
        }
    }
}