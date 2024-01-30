using Basic.CSV2Table;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.Plan
{
    public class PlayerLearnControl : MonoBehaviour
    {
        [SerializeField] private PlanManger planManager;
        [SerializeField] private Text courseName;
        [SerializeField] private Text courseLevel;
        [SerializeField] private Text planNumberText;
        [SerializeField] private GameObject upIcon;
        [SerializeField] private Image green;
        [SerializeField] private Image red;
        [SerializeField] private Button subtraction;
        [SerializeField] private Button addition;
        [SerializeField] private Text description;
        [SerializeField] private GameObject max;

        /// <summary>
        /// 计划中该技能的分配数
        /// </summary>
        private int PlanNumber
        {
            get
            {
                var planNumber = 0;
                foreach (var i in planManager.playerPlan)
                    if (i == originalDataRow.Id)
                        planNumber++;

                return planNumber;
            }
        }

        /// <summary>
        /// 玩家技能的原始数据
        /// </summary>
        private PlayerCourseList.Row originalDataRow;

        /// <summary>
        /// 存档中该技能的数据
        /// </summary>
        private PlayerCourse playerCourse;

        /// <summary>
        /// 技能的效果数据
        /// </summary>
        private PlayerCourseLevelList playerCourseLevelList;

        //该技能的最大等级
        private int MAXLevel
        {
            get
            {
                var max = 0;
                var allId = playerCourseLevelList.FindAll_id(originalDataRow.Id);
                foreach (var row in allId)
                    if (int.Parse(row.等级) > max)
                        max = int.Parse(row.等级);
                return max;
            }
        }

        /// <summary>
        /// 初始化词条
        /// </summary>
        /// <param name="row">该词条对应的技能初始信息</param>
        /// <param name="m_playerCourse">该词条对应的存档等级与经验</param>
        /// <param name="playerCourseLevelList">玩家原始技能等级表</param>
        public void Init(PlayerCourseList.Row row, PlayerCourse m_playerCourse,
            PlayerCourseLevelList playerCourseLevelList)
        {
            planManager = PlanManger.Instance;
            originalDataRow = row;
            playerCourse = m_playerCourse;
            this.playerCourseLevelList = playerCourseLevelList;
            courseName.text = row.Name;
            description.text = row.description;
            planManager.UIUpdateEvent += UIUpdate;
            planManager.ClearPlanNumberEvent += ClearPlanNumber;
        }

        private void UIUpdate()
        {
            gameObject.SetActive(playerCourse.isHave);
            if (!playerCourse.isHave) return;

            courseLevel.text = "LV." + playerCourse.level;
            //在这里计算升级所需经验
            Progress();
            //设置能否按加号
            addition.interactable = planManager.RemainingDays > 0;
            //判断是否到了最高等级
            if (playerCourse.level >= MAXLevel)
            {
                addition.interactable = false;
                for (var index = 0; index < planManager.playerPlan.Length; index++)
                    if (planManager.playerPlan[index] == playerCourse.id)
                        planManager.playerPlan[index] = "0";

                green.gameObject.SetActive(false);
                upIcon.gameObject.SetActive(false);
            }

            max.gameObject.SetActive(playerCourse.level >= MAXLevel);
            //设置能否按减号
            subtraction.interactable = PlanNumber != 0;
            //设置数字
            planNumberText.text = PlanNumber.ToString();
        }

        /// <summary>
        /// 清空分派的计划点数
        /// </summary>
        private void ClearPlanNumber()
        {
            planNumberText.text = PlanNumber.ToString();
        }

        public void OnAdd()
        {
            planManager.OnAdd(playerCourse.id);
            planNumberText.text = PlanNumber.ToString();
        }

        public void OnSub()
        {
            planManager.OnSub(playerCourse.id);
            planNumberText.text = PlanNumber.ToString();
        }

        /// <summary>
        /// 设置经验条
        /// </summary>
        private void Progress()
        {
            var maxEx = 0;
            if (playerCourse.level >= MAXLevel)
            {
                maxEx = 0;
            }
            else
            {
                var allId = playerCourseLevelList.FindAll_id(originalDataRow.Id);
                foreach (var row in allId)
                    if (row.等级 == playerCourse.level.ToString())
                    {
                        maxEx = int.Parse(row.下一级所需经验);
                        break;
                    }
            }

            red.fillAmount = (playerCourse.empiricalValue + 0f) / (maxEx + 0f);
            green.fillAmount = (playerCourse.empiricalValue + PlanNumber + 0f) / (maxEx + 0f);
            upIcon.SetActive(playerCourse.empiricalValue + PlanNumber >= maxEx);
        }
    }
}