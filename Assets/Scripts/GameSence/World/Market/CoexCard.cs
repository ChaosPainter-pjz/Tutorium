using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace World
{
    /// <summary>
    /// 任务卡片
    /// </summary>
    public class CoexCard : MonoBehaviour
    {
        [SerializeField] private Text studentName;
        [SerializeField] private Text grade; //要求的学科和分数
        [SerializeField] private Button buttonTake;
        [SerializeField] private Text buttonText;
        [SerializeField] private Button buttonAccomplish; //完成按钮

        private SaveData _saveData;
        private CoexData.Task _task;

        private bool isTask; //显示的是参与还是放弃
        private UnityAction _buttonCallback;

        private void Awake()
        {
            buttonTake.onClick.AddListener(OnTakeButton);
            buttonAccomplish.onClick.AddListener(OnAccomplishButton);
        }

        /// <summary>
        /// 初始化卡片
        /// </summary>
        public void Init(SaveData saveData, CoexData.Task task, UnityAction buttonCallback)
        {
            gameObject.SetActive(true);
            _buttonCallback = buttonCallback;
            _saveData = saveData;
            _task = task;

            if (saveData.coexData.Tasks.Contains(task))
                buttonText.text = "参加";
            else if (saveData.coexData.LockTasks.Contains(task)) buttonText.text = "放弃";
            grade.text = $"类目：{task.Grade.name}    要求分数：{task.Grade.score}";
            StudentUnit student = saveData.studentUnits.Find(st => st.id == task.UnitId);
            if (student != null)
            {
                Grade pro = student.properties.Find(grade => grade.gradeID == task.Grade.gradeID);
                if (pro == null) pro = student.interestGrade.Find(grade => grade.gradeID == task.Grade.gradeID);

                if (pro == null) pro = student.mainGrade.Find(grade => grade.gradeID == task.Grade.gradeID);

                if (pro.score >= task.Grade.score) buttonAccomplish.gameObject.SetActive(true);
                studentName.text = $"接取人：{student.fullName}";
            }
            else
            {
                studentName.text = "待报名";
            }
        }

        /// <summary>
        /// 接取任务
        /// </summary>
        public void OnTakeButton()
        {
            if (isTask)
            {
                _task.LockingTime = _saveData.gameDate.Copy();
                _saveData.coexData.LockTasks.Add(_task);
                _saveData.coexData.Tasks.Remove(_task);
            }
            else
            {
                _saveData.coexData.LockTasks.Remove(_task);
                _saveData.coexData.Tasks.Remove(_task);
            }

            _buttonCallback?.Invoke();
        }

        /// <summary>
        /// 交任务
        /// </summary>
        public void OnAccomplishButton()
        {
            _saveData.coexData.LockTasks.Remove(_task);
            _saveData.coexData.Tasks.Remove(_task);
            gameObject.SetActive(false);
            //这里还要给奖励
            _buttonCallback?.Invoke();
        }
    }
}
