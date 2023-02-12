using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ArchiveControl : MonoBehaviour
{
    public SaveObject saveObject;

    [SerializeField ] private Text fullName;
    [SerializeField]private  Text gameDate;
    [SerializeField] private Text dateTime;
    public GameObject delete;

    private int id;
    private SaveData saveData;


    public void Init(int id)
    {
        this.id = id;
        UIUpdate();
    }

    public void UIUpdate()
    {
        saveData = SaveManager.LoadGame(id);
        if (saveData==null || !saveData.isInstance)
        {
            fullName.gameObject.SetActive(false);
            gameDate.text = "空存档";
            dateTime.gameObject.SetActive(false);

            if (delete!=null)
                delete.SetActive(false);
        }
        else
        {
            fullName.gameObject.SetActive(true);

            dateTime.gameObject.SetActive(true);
            if (delete!=null)
                delete.SetActive(true);

            fullName.text = saveData.playerUnit.fullName;
            gameDate.text = saveData.gameDate.ToString();
            dateTime.text = saveData.dateTime.ToString("yyyy.MM.dd hh:mm");
        }
    }
    /// <summary>
    /// 单击了面板
    /// </summary>
    public void OnClick()
    {
        if (saveData == null || saveData.isInstance == false)
            NewGame();
        else
            LoadGame();
    }
    /// <summary>
    /// 将当前saveData中的存档保存到设备中
    /// </summary>
    public void SaveArchive()
    {
        SaveManager.SaveGame(saveObject.SaveData,id);
        saveData = saveObject.SaveData;
        UIUpdate();
    }
    //新游戏
    private void NewGame()
    {
        saveData = new SaveData() {isInstance = true,InitYear = DateTime.Now.Year};
        SaveManager.SaveGame(saveData,id);
        LoadGame();
    }
    // 加载存档以开始游戏
    private void LoadGame()
    {
        saveObject.SaveData = saveData;
        ArchivesManage.Instance.LoadScene();
    }
    /// <summary>
    /// 删除存档
    /// </summary>
    public void DeleteArchive()
    {
        SaveManager.ResetSaveFile(id);
        UIUpdate();
    }
}