using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ArchiveControl[] loadArchiveControls;
    [SerializeField] private ArchiveControl[] saveArchiveControls;
    [SerializeField] private FullScreenModeControl fullScreenModeControl;
    [SerializeField] private SettingMainControl[] settingMainControls;

    private void Awake()
    {
        gameManager.StartEvent += Init;
    }

    private void Init()
    {
        for (int i = 0; i < loadArchiveControls.Length; i++)
        {
            loadArchiveControls[i].Init(i);
        }

        for (int i = 0; i < saveArchiveControls.Length; i++)
        {
            saveArchiveControls[i].Init(i);
        }
        fullScreenModeControl.Init();
        foreach (var control in settingMainControls)
        {
            control.Init();
        }
    }

    /// <summary>
    /// 返回主菜单页面
    /// </summary>
    public void ReturnMain()
    {
        SceneManager.LoadSceneAsync(sceneBuildIndex: 0);
    }
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}