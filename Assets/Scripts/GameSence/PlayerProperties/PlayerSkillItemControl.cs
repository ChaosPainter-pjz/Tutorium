using System.Collections.Generic;
using System.Linq;
using Basic.CSV2Table;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.PlayerProperties
{
    /// <summary>
    /// 玩家技能词条控制器
    /// </summary>
    public class PlayerSkillItemControl : MonoBehaviour
    {
        [SerializeField] private Text skillName;
        [SerializeField] private Text skillLevel;
        [SerializeField] private Image red;
        [SerializeField] private GameObject maxText;
        [SerializeField] private Text skillDescription;
        private GameManager.GameManager gameManager;
        private PlayerCourseList playerCourseList;
        private PlayerCourseLevelList playerCourseLevelList;
        private PlayerWorkList playerWorkList;
        public PlayerCourse thisCourse;

        public void Init(GameManager.GameManager gameManager)
        {
            this.gameManager = gameManager;
            playerCourseList = gameManager.PlayerCourseList;
            playerCourseLevelList = gameManager.PlayerCourseLevelList;
            playerWorkList = gameManager.PlayerWorkList;
        }

        public void UpdateUI(PlayerCourse playerCourse)
        {
            thisCourse = playerCourse;
            if (playerCourse.isHave)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
                return;
            }

            skillLevel.text = "Lv." + playerCourse.level;
            var courseRow = playerCourseList.Find_Id(playerCourse.id);
            if (courseRow != null) //该playerCourse是技能
            {
                var courseLevelRows = playerCourseLevelList.FindAll_id(playerCourse.id);
                //获取该技能的最高等级
                var maxLevel = courseLevelRows.Select(row => int.Parse(row.等级)).Prepend(-1).Max();

                if (playerCourse.level < maxLevel)
                {
                    var levelRow = courseLevelRows.Find(x => int.Parse(x.等级) == playerCourse.level);
                    red.fillAmount = (playerCourse.empiricalValue + 0f) / float.Parse(levelRow.下一级所需经验);
                }
                else if (playerCourse.level == maxLevel)
                {
                    maxText.SetActive(true);
                    red.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log(playerCourse.id + "等级超出最高等级");
                }

                skillName.text = courseRow.Name;
                skillDescription.text = courseRow.description;
                return;
            }

            var workRow = playerWorkList.Find_id(playerCourse.id);
            if (workRow != null) //该playerCourse是工作
            {
                if (playerCourse.level < int.Parse(workRow.maxLevel))
                    red.fillAmount = (playerCourse.empiricalValue + 0f) / 10f;
                skillName.text = workRow.name;
                skillDescription.text = workRow.description;
            }
        }
    }
}