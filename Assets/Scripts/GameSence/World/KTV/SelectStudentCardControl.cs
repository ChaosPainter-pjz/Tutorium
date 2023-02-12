using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace World.KTV
{
    /// <summary>
    /// KTV中学生卡片的控制器
    /// </summary>
    public class SelectStudentCardControl : MonoBehaviour
    {
        [SerializeField] private Image headPortrait;
        [SerializeField] private Transform propertyParent;
        [SerializeField] private GameObject propertyPrefab;
        [SerializeField] private Toggle toggle;

        [SerializeField] private List<WorldGame.ScoreCardControl> scoreCardControls;

        public StudentUnit studentUnit;
        private UnityAction<StudentUnit,bool> callBack;
        public void UpdateUI(StudentUnit _studentUnit, List<Grade> grades, UnityAction<StudentUnit,bool> callBack,bool isOn)
        {
            gameObject.SetActive(true);
            studentUnit = _studentUnit;
            this.callBack = callBack;

            headPortrait.sprite = ResourceManager.Instance.studentHeadPortrait[int.Parse(_studentUnit.id)-1];
            scoreCardControls ??= new List<WorldGame.ScoreCardControl>();
            foreach (var control in scoreCardControls)
            {
                control.gameObject.SetActive(false);
            }

            for (int i = 0; i < grades.Count; i++)
            {
                if (i >= scoreCardControls.Count)
                {
                    var control = Instantiate(propertyPrefab, propertyParent).GetComponent<WorldGame.ScoreCardControl>();
                    scoreCardControls.Add(control);
                }

                scoreCardControls[i].UpdateUI(grades[i].name, grades[i].score.ToString());
            }
        }
        /// <summary>
        /// 点击卡片
        /// </summary>
        /// <param name="isOn"></param>
        public void OnToggle(bool isOn)
        {
            callBack(studentUnit,isOn);
        }
    }
}