using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldGame;

public class WorldGameManager : MonoInstance<WorldGameManager>
{
    [SerializeField] private GameManager gameManager;
    public GameAwardControl gameAwardControl;
    /// <summary>
    /// 世界小游戏选人界面控制器
    /// </summary>
    public SelectStudentControl selectStudentControl;

    public PhotographyGameStart photographyGameStart;
    public PhotographyGameControl photographyGameControl;
    /// <summary>
    /// 刷新世界游戏，
    /// </summary>
    public void UpdateWorldGame()
    {
        //photographyGameStart.gameObject.SetActive(false);
        int photoV = Random.Range(0, 6);
        if (photoV==1)
        {
            photographyGameStart.gameObject.SetActive(true);
            HintManager.Instance.AddHint(new Hint("摄影竞赛","银后杯摄影大赛正在举行，可前往世界参与（免费的还不来吗）"));
        }
        else
        {
            photographyGameStart.gameObject.SetActive(false);
        }
    }
}