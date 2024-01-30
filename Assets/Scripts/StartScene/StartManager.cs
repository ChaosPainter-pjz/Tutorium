using System.Collections.Generic;
using System.Linq;
using Basic;
using GameSence.Setting;
using Plugins.Steamworks.NET.autogen;
using UnityEngine;
using UnityEngine.UI;

namespace StartScene
{
    public class StartManager : MonoInstance<StartManager>
    {
        [SerializeField] private List<SettingMainControl> settingMainControls;
        [SerializeField] private Text steamName;
        [SerializeField] private FullScreenModeControl fullScreenModeControl;
        [SerializeField] private SaveObject saveObject;
        [SerializeField] private Button galleriesButton;

        public void Start()
        {
            foreach (var control in settingMainControls) control.Init();
            fullScreenModeControl.Init();
            steamName.text = SteamFriends.GetPersonaName();
            InitGalleries();
        }

        /// <summary>
        /// 初始化追忆
        /// </summary>
        private void InitGalleries()
        {
            saveObject.OverSaveData = SaveManager.Scripts.SaveManager.LoadGame();
            galleriesButton.interactable = saveObject.OverSaveData.overUnits.Any();
        }

        public void OnSetting()
        {
        }

        public void OnHuaLang()
        {
        }

        public void OnKaiFaTuanDui()
        {
        }

        public void OnExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }
}