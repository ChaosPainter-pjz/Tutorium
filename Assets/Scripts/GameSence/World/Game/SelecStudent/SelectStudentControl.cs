using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorldGame
{
    public class SelectStudentControl : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Button enter;
        [SerializeField] private ToggleGroup group;
        [SerializeField] private Transform studentCardParent;
        [SerializeField] private GameObject studentCartPrefab;
        /// <summary>
        /// 进入该面板的按钮，传递过来是为了在进入游戏时关闭入口，让玩家不能重复进入
        /// </summary>
        private GameObject enterButton;
        public delegate void WorldGameStartDelegate(StudentUnit _studentUnit, int number);

        private WorldGameStartDelegate @delegate;
        private List<SelectStudentCardControl> studentCardControls;
        private List<StudentUnit> StudentUnits => gameManager.saveObject.SaveData.studentUnits;
        private StudentUnit student;
        private int number;

        ///  <summary>
        /// 刷新页面，传递数据
        ///  </summary>
        ///  <param name="gradeIds">"心情""信任""temperament""1""2"等</param>
        ///  <param name="_number">游戏内参考数据</param>
        ///  <param name="_enterButton">进入选择面板的按钮，传递过来是为了在进入游戏时关闭入口，让玩家不能重复进入</param>
        ///  <param name="_delegate">启动游戏方法</param>
        public void UpdateUI(List<string> gradeIds, int _number,GameObject  _enterButton,WorldGameStartDelegate _delegate)
        {
            this.@delegate = _delegate;
            this.number = _number;
            enterButton = _enterButton;
            studentCardControls ??= new List<SelectStudentCardControl>();
            for (int i = 0; i < StudentUnits.Count; i++)
            {
                if (i >= studentCardControls.Count)
                {
                    var control = Instantiate(studentCartPrefab, studentCardParent).GetComponent<SelectStudentCardControl>();
                    studentCardControls.Add(control);
                }

                List<Grade> grades = new List<Grade>();
                foreach (var id in gradeIds)
                {
                    if (id == "心情")
                        grades.Add(new Grade() {name = "心情", score = StudentUnits[i].Mood});
                    else if (id == "信任")
                        grades.Add(new Grade() {name = "信任", score = StudentUnits[i].Trust});
                    else if (StudentUnits[i].properties.Exists(x => x.gradeID == id))
                        grades.Add(StudentUnits[i].properties.Find(x => x.gradeID == id));
                    else if (StudentUnits[i].mainGrade.Exists(x => x.gradeID == id))
                        grades.Add(StudentUnits[i].mainGrade.Find(x => x.gradeID == id));
                    else if (StudentUnits[i].interestGrade.Exists(x => x.gradeID == id))
                        grades.Add(StudentUnits[i].interestGrade.Find(x => x.gradeID == id));
                    else
                        Debug.LogError("无效的id" + id);
                }

                studentCardControls[i].UpdateUI(StudentUnits[i], grades);
            }
            gameObject.SetActive(true);
        }

        public void OnEnter()
        {
            IEnumerable<Toggle> activeToggles = group.ActiveToggles();
            Toggle toggle = null;
            foreach (Toggle to in activeToggles)
            {
                if (to.isOn)
                {
                    toggle = to;
                }
            }
            if (toggle is { }) student = toggle.GetComponent<SelectStudentCardControl>().studentUnit;
            @delegate?.Invoke(student, number);
            enterButton.SetActive(false);
            gameObject.SetActive(false);
        }

        private void Update()
        {
            enter.interactable = group.AnyTogglesOn();

        }
    }
}