using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 存档管理器
/// </summary>
public class ArchivesManage : MonoInstance<ArchivesManage>
{
    [SerializeField] private ArchiveControl[] archiveControls;
    [SerializeField] private GameObject loadPanel;
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < archiveControls.Length; i++)
        {
            archiveControls[i].Init(i);
        }
    }

    public void LoadScene()
    {
        StartCoroutine(LoadLeaver());
    }

    private IEnumerator LoadLeaver()
    {
        loadPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = true;
        while(!operation.isDone)   //当场景没有加载完毕
        {
            yield return null;
        }
    }
}