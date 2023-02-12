using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Class (*Static*) | 保存管理器类，用于保存和加载游戏
/// </summary>
public static class SaveManager
{
    private static string SaveFile_Over = "/GalleriesFile.sf";
    public static void SaveGame(SaveData data,int id)
    {
        data.dateTime = DateTime.Now;
        string savePath = Application.persistentDataPath + "/SaveFile"+id+".sf";

        // 删除了旧的保存文件，这样做是为了避免在加载时出现问题，因为改变类会导致错误，所以没有做。
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.OpenOrCreate);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    /// <summary>
    /// 追忆存档专用
    /// </summary>
    /// <param name="data"></param>
    public static void SaveOverGame(OverSaveData data )
    {
        string savePath = Application.persistentDataPath + SaveFile_Over;

        // 删除了旧的保存文件，这样做是为了避免在加载时出现问题，因为改变类会导致错误，所以没有做。
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.OpenOrCreate);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SaveData LoadGame(int id)
    {
        string savePath = Application.persistentDataPath + "/SaveFile"+id+".sf";

        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);
            SaveData data = new SaveData();
            try
            {
                data = formatter.Deserialize(stream) as SaveData;
            }
            catch (Exception)
            {
                // ignored
            }


            stream.Close();

            return data;
        }
        return null;
    }
    /// <summary>
    /// 加载追忆文件
    /// </summary>
    /// <returns></returns>
    public static OverSaveData LoadGame()
    {
        string savePath = Application.persistentDataPath + SaveFile_Over;

        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);
            OverSaveData data = new OverSaveData();
            try
            {
                data = formatter.Deserialize(stream) as OverSaveData;
            }
            catch (Exception)
            {
                // ignored
            }


            stream.Close();

            return data;
        }
        else
        {
            OverSaveData date = new OverSaveData();
            SaveOverGame(date);
            return date;
        }
    }

    /// <summary>
    /// Static | 将保存文件重置为没有保存值的空白文件。
    /// </summary>
    public static void ResetSaveFile(int id)
    {
        string savePath = Application.persistentDataPath + "/SaveFile"+id+".sf";

        // 擦掉了旧的保存文件，这样做是为了避免加载出现问题，因为换类会导致错误没有完成。
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.OpenOrCreate);
        SaveData saveData = new SaveData() {isInstance = false};
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static int GetScreenMode()
    {
        return PlayerPrefs.GetInt("ScreenMode",0);
    }

    public static void SetScreenMode(int i)
    {
        PlayerPrefs.SetInt("ScreenMode",i);
    }
    /// <summary>
    /// 获取音量大小
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static float GetVolume(string name)
    {
        return PlayerPrefs.GetFloat(name, 0);
    }
    /// <summary>
    /// 获取音量的开关
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetIsVolume(string name)
    {
        return PlayerPrefs.GetString(name, "true");
    }
    /// <summary>
    /// 设置音量的大小
    /// </summary>
    /// <param name="name"></param>
    /// <param name="volume"></param>
    public static void SetVolume(string name,float volume)
    {
        PlayerPrefs.SetFloat(name,volume);
    }
    /// <summary>
    /// 设置音量的开关
    /// </summary>
    /// <param name="name"></param>
    /// <param name="toggle"></param>
    public static void SetIsVolume(string name,string toggle)
    {
        PlayerPrefs.SetString(name,toggle);
    }

}