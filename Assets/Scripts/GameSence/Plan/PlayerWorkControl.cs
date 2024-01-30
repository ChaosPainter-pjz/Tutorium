using Basic.CSV2Table;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.Plan
{
    public class PlayerWorkControl : MonoBehaviour
    {
        [SerializeField] private Text workName;
        [SerializeField] private Text workYield;
        [SerializeField] private GameObject upGameObject;
        [SerializeField] private Image redImage;
        [SerializeField] private Image greenImage;
        [SerializeField] private Text max;
        [SerializeField] private Text planNumberText;
        [SerializeField] private Button subtraction;
        [SerializeField] private Button addition;
        [SerializeField] private Text description;

        private PlanManger planManager;

        /// <summary>
        /// 该工作的原始数据
        /// </summary>
        private PlayerWorkList.Row workRow;

        /// <summary>
        /// 存档中该技能的数据
        /// </summary>
        private PlayerCourse playerCourse;

        private int PlanNumber
        {
            get
            {
                var planNumber = 0;
                foreach (var id in planManager.playerPlan)
                    if (id == playerCourse.id)
                        planNumber++;

                return planNumber;
            }
        }

        /// <summary>
        /// 初始化词条
        /// </summary>
        public void Init(PlayerWorkList.Row row, PlayerCourse m_playerCourse)
        {
            planManager = PlanManger.Instance;
            workRow = row;
            playerCourse = m_playerCourse;
            workName.text = row.name;
            description.text = row.description;
            subtraction.onClick.AddListener(OnSub);
            addition.onClick.AddListener(OnAdd);

            planManager.UIUpdateEvent += UIUpdate;
            planManager.ClearPlanNumberEvent += ClearPlanNumber;
        }

        private void UIUpdate()
        {
            gameObject.SetActive(playerCourse.isHave);
            if (!playerCourse.isHave) return;
            workYield.text =
                "￥" + (playerCourse.level * int.Parse(workRow.levelYield) + int.Parse(workRow.InitialYield));

            planNumberText.text = PlanNumber.ToString();
            Progress(PlanNumber);
            addition.interactable = planManager.RemainingDays > 0;
            //到达最高等级
            if (playerCourse.level >= int.Parse(workRow.maxLevel))
            {
                //addition.interactable = false;
                max.gameObject.SetActive(true);
                greenImage.gameObject.SetActive(false);
                upGameObject.SetActive(false);
                // for (int i = 0; i < planManager.playerPlan.Length; i++)
                // {
                //     if (planManager.playerPlan[i] == playerCourse.id)
                //     {
                //         planManager.playerPlan[i] = "0";
                //     }
                // }
            }
            else
            {
                max.gameObject.SetActive(false);
                greenImage.gameObject.SetActive(true);
            }

            //设置能否按减号
            subtraction.interactable = PlanNumber != 0;
        }

        /// <summary>
        /// 设置经验条
        /// </summary>
        private void Progress(int planNumber)
        {
            var maxEx = playerCourse.level >= int.Parse(workRow.maxLevel) ? 0 : int.Parse(workRow.UpExperience);

            redImage.fillAmount = (playerCourse.empiricalValue + 0f) / (maxEx + 0f);
            greenImage.fillAmount = (playerCourse.empiricalValue + planNumber + 0f) / (maxEx + 0f);
            upGameObject.SetActive(playerCourse.empiricalValue + planNumber >= maxEx);
        }

        /// <summary>
        /// 清空分派的计划点数
        /// </summary>
        private void ClearPlanNumber()
        {
            planNumberText.text = "0";
        }

        private void OnAdd()
        {
            planManager.OnAdd(playerCourse.id);
            planNumberText.text = PlanNumber.ToString();
        }

        private void OnSub()
        {
            planManager.OnSub(playerCourse.id);
            planNumberText.text = PlanNumber.ToString();
        }
    }
}