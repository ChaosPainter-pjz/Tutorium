using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace World
{
    /// <summary>
    /// 任务卡片
    /// </summary>
    public class CoexCard : MonoBehaviour
    {
        [SerializeField] private Text studentName;
        [SerializeField] private Text gradeName;
        [SerializeField] private Text gradeScoreName; //要求的分数
        [SerializeField] private Text rewardText; //奖励
        [SerializeField] private Button buttonTake;
        [SerializeField] private Button buttonAbandon;
        [SerializeField] private Button buttonAccomplish;

        private SaveData _saveData;
        private CoexData.Task _task;

        /// <summary>
        /// 初始化卡片
        /// </summary>
        public void Init(SaveData saveData, CoexData.Task task)
        {
            gameObject.SetActive(true);
            _saveData = saveData;
            _task = task;

            if (saveData.coexData.Tasks.Contains(task))
            {
                buttonTake.interactable = true;
                buttonAbandon.interactable = false;
            }
            else if (saveData.coexData.LockTasks.Contains(task))
            {
                buttonTake.interactable = false;
                buttonAbandon.interactable = true;
            }

            StudentUnit student = saveData.studentUnits.Find(st => st.id == task.UnitId);
            Grade pro = student.properties.Find(grade => grade.gradeID == task.Grade.gradeID);
            if (pro == null) pro = student.interestGrade.Find(grade => grade.gradeID == task.Grade.gradeID);

            if (pro == null) pro = student.mainGrade.Find(grade => grade.gradeID == task.Grade.gradeID);
            if (pro.score >= task.Grade.score) buttonAccomplish.interactable = true;
        }

        public void OnTakeButton()
        {
            buttonTake.interactable = false;
            _task.LockingTime = _saveData.gameDate.Copy();
            _saveData.coexData.LockTasks.Add(_task);
            _saveData.coexData.Tasks.Remove(_task);
        }

        public void OnAbandonButton()
        {
            _saveData.coexData.LockTasks.Remove(_task);
            _saveData.coexData.Tasks.Remove(_task);
            gameObject.SetActive(false);
        }

        public void OnAccomplishButton()
        {
            _saveData.coexData.LockTasks.Remove(_task);
            _saveData.coexData.Tasks.Remove(_task);
            gameObject.SetActive(false);
            //这里还要给奖励
        }
    }
}
