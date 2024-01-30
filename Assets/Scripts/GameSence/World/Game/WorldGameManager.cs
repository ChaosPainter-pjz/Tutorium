using Basic;
using GameSence.Hint;
using GameSence.World.Game.PhotographyGame;
using GameSence.World.Game.SelecStudent;
using GameSence.World.Game.StartGame;
using UnityEngine;

namespace GameSence.World.Game
{
    public class WorldGameManager : MonoInstance<WorldGameManager>
    {
        [SerializeField] private GameManager.GameManager gameManager;
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
            var photoV = Random.Range(0, 6);
            if (photoV == 1)
            {
                photographyGameStart.gameObject.SetActive(true);
                HintManager.Instance.AddHint(new Hint.Hint("摄影竞赛", "银后杯摄影大赛正在举行，可前往世界参与（免费的还不来吗）"));
            }
            else
            {
                photographyGameStart.gameObject.SetActive(false);
            }
        }
    }
}