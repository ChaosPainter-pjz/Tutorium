using System.Collections.Generic;
using GameSence.StoryLine;
using Unit;
using UnityEngine;

namespace GameSence.PropertyChange
{
    /// <summary>
    /// 教师属性更新结果显示面板
    /// </summary>
    public class PlayerPropertyChangeControl : MonoBehaviour
    {
        [SerializeField] private GameManager.GameManager gameManager;
        [SerializeField] private StoryLineManager storyLineManager;
        [SerializeField] private Transform playerPropertyGlossaryParent; //预制体的父物体
        [SerializeField] private GameObject playerUpdatePropertyGlossary; //预制体
        [SerializeField] public PlayerPropertyChangeMoneyControl moneyControl;
        [SerializeField] private Animator dateAnimator;
        private List<PlayerCourse> playerCourses;
        private List<PlayerUpdatePropertyGlossaryControl> playerUpdatePropertyGlossaryControls = new();

        public void Init()
        {
            playerCourses = gameManager.saveObject.SaveData.playerUnit.playerCourses;
        }

        /// <summary>
        /// 传递初始化low中的文本
        /// </summary>
        public void PlayerInitPanel()
        {
            foreach (var control in playerUpdatePropertyGlossaryControls) //隐藏所有词条
                control.Conceal();

            foreach (var playerCourse in playerCourses) //遍历玩家技能
            {
                if (playerCourse.id == "0" || !playerCourse.isHave) continue;

                var control = playerUpdatePropertyGlossaryControls.Find(x => x.id == playerCourse.id);
                if (control == null) //如何集合中没有对应教师技能的词条，那就生成一个
                {
                    control = Instantiate(playerUpdatePropertyGlossary, playerPropertyGlossaryParent)
                        .GetComponent<PlayerUpdatePropertyGlossaryControl>();
                    control.id = playerCourse.id;
                    playerUpdatePropertyGlossaryControls.Add(control);
                }

                control.Init(playerCourse);
            }
        }

        /// <summary>
        /// 传递更新new中的文本
        /// </summary>
        public void PlayerUpdatePanel()
        {
            foreach (var playerCourse in playerCourses)
            {
                if (playerCourse.id == "0" || !playerCourse.isHave) continue;

                var control = playerUpdatePropertyGlossaryControls.Find(x => x.id == playerCourse.id);
                control.UIUpdate(playerCourse);
            }
        }

        /// <summary>
        /// 点击确认或点击空白处
        /// </summary>
        public void OnEnter()
        {
            dateAnimator.SetTrigger("Trigger"); //动画结束后会有剧情触发事件
            gameObject.SetActive(false);
        }
    }
}