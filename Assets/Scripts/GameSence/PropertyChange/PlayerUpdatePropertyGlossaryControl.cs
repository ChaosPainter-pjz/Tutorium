using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.PropertyChange
{
    /// <summary>
    /// 工作日后显示技能提升信息所用的词条的控制器
    /// </summary>
    public class PlayerUpdatePropertyGlossaryControl : MonoBehaviour
    {
        [SerializeField] private Text skillsName;
        [SerializeField] private Text lowLevel;
        [SerializeField] private Text lowXP;
        [SerializeField] private Text newLevel;
        [SerializeField] private Text newXP;

        public string id = "空";

        public void Init(PlayerCourse lowPlayerCourse)
        {
            id = lowPlayerCourse.id;
            skillsName.text = lowPlayerCourse.name;
            lowLevel.text = "Lv." + lowPlayerCourse.level;
            if (int.Parse(lowPlayerCourse.id) < 100)
            {
                var idAll = GameManager.GameManager.Instance.PlayerCourseLevelList.FindAll_id(lowPlayerCourse.id);
                var maxXP = idAll.Find(x => x.等级 == lowPlayerCourse.level.ToString()).下一级所需经验;
                lowXP.text = lowPlayerCourse.empiricalValue + "/" + maxXP;
            }
            else
            {
                lowXP.text = lowPlayerCourse.empiricalValue + "/" + "10";
            }
        }

        public void UIUpdate(PlayerCourse newPlayerCourse)
        {
            newLevel.text = "Lv." + newPlayerCourse.level;
            if (int.Parse(newPlayerCourse.id) < 100)
            {
                var idAll = GameManager.GameManager.Instance.PlayerCourseLevelList.FindAll_id(newPlayerCourse.id);
                var maxXP = idAll.Find(x => x.等级 == newPlayerCourse.level.ToString()).下一级所需经验;
                newXP.text = newPlayerCourse.empiricalValue + "/" + maxXP;
            }
            else
            {
                newXP.text = newPlayerCourse.empiricalValue + "/" + "10";
            }

            if (lowLevel.text == newLevel.text && lowXP.text == newXP.text)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }

        public void Conceal()
        {
            gameObject.SetActive(false);
        }
    }
}