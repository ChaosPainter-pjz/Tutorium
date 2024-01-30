using SaveManager.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameSence.World.Market
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
            CardUpdate();
        }

        /// <summary>
        /// 刷新卡片的显示
        /// </summary>
        public void CardUpdate()
        {
            if (!gameObject.activeSelf) return;
            if (_saveData.coexData.Tasks.Contains(_task))
                buttonText.text = "参加";
            else if (_saveData.coexData.LockTasks.Contains(_task))
                buttonText.text = "放弃";
            grade.text = $"类目：{_task.Grade.name}    要求分数：{_task.Grade.score}";
            var student = _saveData.studentUnits.Find(st => st.id == _task.UnitId);
            if (student != null)
            {
                var pro = student.properties.Find(grade => grade.gradeID == _task.Grade.gradeID);
                if (pro == null) pro = student.interestGrade.Find(grade => grade.gradeID == _task.Grade.gradeID);

                if (pro == null) pro = student.mainGrade.Find(grade => grade.gradeID == _task.Grade.gradeID);

                if (pro.score >= _task.Grade.score) buttonAccomplish.gameObject.SetActive(true);
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
