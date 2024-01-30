using System.Collections.Generic;
using GameSence.GameManager;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.World.Game.SelecStudent
{
    /// <summary>
    /// 小游戏选人界面学生卡片控制器
    /// </summary>
    public class SelectStudentCardControl : MonoBehaviour
    {
        [SerializeField] private Image headPortrait;
        [SerializeField] private Transform propertyParent;
        [SerializeField] private GameObject propertyPrefab;
        [SerializeField] private Toggle toggle;

        [SerializeField] private List<ScoreCardControl> scoreCardControls;

        public StudentUnit studentUnit;

        public void UpdateUI(StudentUnit _studentUnit, List<Grade> grades)
        {
            studentUnit = _studentUnit;
            OnToggle();

            headPortrait.sprite = ResourceManager.Instance.studentHeadPortrait[int.Parse(_studentUnit.id) - 1];
            scoreCardControls ??= new List<ScoreCardControl>();
            foreach (var control in scoreCardControls) control.gameObject.SetActive(false);

            for (var i = 0; i < grades.Count; i++)
            {
                if (i >= scoreCardControls.Count)
                {
                    var control = Instantiate(propertyPrefab, propertyParent).GetComponent<ScoreCardControl>();
                    scoreCardControls.Add(control);
                }

                scoreCardControls[i].UpdateUI(grades[i].name, grades[i].score.ToString());
            }
        }

        public void OnToggle()
        {
            if (toggle.group == null) toggle.group = GetComponentInParent<ToggleGroup>();
        }
    }
}