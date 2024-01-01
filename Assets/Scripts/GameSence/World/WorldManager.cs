using System.Collections;
using System.Collections.Generic;
using Supermarket;
using UnityEngine;
using World;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject worldPanel;
    [SerializeField] private Transform mainPanel;
    [SerializeField] private AudioControl bgmAudioControl;
    [SerializeField] private StoryLineManager storyLineManager;
    [SerializeField] private SupermarketControl supermarketControl;
    [SerializeField] private KTVControl ktvControl;
    private PlotJudgmentList otherPlotJudgment;
    [SerializeField] private CoexControl coexControl;

    public void OnExit()
    {
        if (worldPanel.activeSelf)
        {
            animator.Play("ExitWorldPanel");
            StartCoroutine(exit());
        }
    }

    private IEnumerator exit()
    {
        yield return new WaitForSeconds(1f);
        bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType.Main);
        mainPanel.localScale = Vector3.one;
        worldPanel.SetActive(false);
    }

    /// <summary>
    /// 点击外出按钮
    /// </summary>
    public void GoToWorld()
    {
        bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
        worldPanel.SetActive(true);
        mainPanel.localScale = Vector3.zero;
    }

    public void UnderConstruction()
    {
        HintManager.Instance.AddHint(new Hint("施工中", "该商户还在装修中，尚未开业"));
    }

    public void OnKtv()
    {
        if (gameManager.saveObject.SaveData.gameDate.year == gameManager.saveObject.SaveData.InitYear)
            HintManager.Instance.AddHint(new Hint("施工即将完成", "工作人员告诉你：该商户还在试营业中，将在明年第一周开业"));
        else
            ktvControl.gameObject.SetActive(true);
    }

    public void OnSupermarket()
    {
        if (gameManager.saveObject.SaveData.gameDate.year == gameManager.saveObject.SaveData.InitYear &&
            gameManager.saveObject.SaveData.gameDate.Semester == 0)
            HintManager.Instance.AddHint(new Hint("施工即将完成", "工作人员告诉你：该商户还在试营业中，将在今年下学期开业"));
        else
            supermarketControl.gameObject.SetActive(true);
    }

    /// <summary>
    /// 点击会展中心
    /// </summary>
    public void OnCoex()
    {
        //if (IsCoex())
        if (false)
            coexControl.gameObject.SetActive(true);
        else
            HintManager.Instance.AddHint(new Hint("施工即将完成", "工作人员告诉你：该商户还在装修中，尚不知晓何时开业"));
    }

    /// <summary>
    /// 返回现在会展广场是否开业了
    /// </summary>
    /// <returns></returns>
    private bool IsCoex()
    {
        if (gameManager.saveObject.SaveData.gameDate.year < gameManager.saveObject.SaveData.InitYear + 1) return false;

        if (gameManager.saveObject.SaveData.gameDate.year == gameManager.saveObject.SaveData.InitYear + 1
            && gameManager.saveObject.SaveData.gameDate.Semester == 0)
            return false;

        return true;
    }


    public void OnSchool()
    {
        otherPlotJudgment ??= gameManager.OtherPlotJudgmentList;
        storyLineManager.qualifiedPlot.Add(otherPlotJudgment.Find_PlotId("school"));
        storyLineManager.BeganPlot();
    }
}
