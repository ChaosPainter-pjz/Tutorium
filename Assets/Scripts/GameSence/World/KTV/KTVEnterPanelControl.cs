using System.Collections;
using System.Collections.Generic;
using Basic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace World.KTV
{
    /// <summary>
    /// KTV确认面板控制器
    /// </summary>
    public class KTVEnterPanelControl : MonoBehaviour
    {
        [SerializeField] private SaveObject saveObject;
        [SerializeField] private Button enterButton;
        [SerializeField] private Text priceText;
        [SerializeField] private Toggle isDay6Toggle;
        [SerializeField] private Toggle isDay0Toggle;


        public bool isDay6;
        public bool isDay0;
        private int price; //单价
        private int allPrice; //总价
        private int time; //所选择的时间段，早中晚
        [Header("students")] [SerializeField] private Transform cardParent;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private List<SelectStudentCardControl> studentCardControls;
        private UnityAction callBack;
        /// <summary>
        /// 被选中的学生
        /// </summary>
        [SerializeField] private List<StudentUnit> selectStudentUnits = new List<StudentUnit>();

        /// <summary>
        /// KTV的安排,不要直接将它赋值到学生日程，应该赋值它的值
        /// </summary>
        private readonly Schedule thisSchedule = new Schedule() { id = "42", lockTime = 1 };

        /// <summary>
        /// 进入确认面版
        /// </summary>
        /// <param name="price">当前选中的房间的价格</param>
        /// <param name="time">当前房间的时间段</param>
        public void OnPanel(int price, int time,UnityAction callBack)
        {
            gameObject.SetActive(true);
            this.price = price;
            this.time = time;
            this.callBack = callBack;
            isDay6Toggle.isOn = isDay6;
            isDay0Toggle.isOn = isDay0;
            UpdateStudentCard();
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (isDay0 && isDay6)
            {
                allPrice = 2 * price;
                enterButton.interactable = true;
            }
            else if (!isDay0 && !isDay6)
            {
                allPrice = 0;
                enterButton.interactable = false;
            }
            else
            {
                allPrice = price;
                allPrice = price;
                enterButton.interactable = true;
            }

            priceText.text = allPrice.ToString();

            if (enterButton.interactable)
            {
                if (selectStudentUnits == null || selectStudentUnits.Count == 0)
                {
                    enterButton.interactable = false;
                }
            }
        }

        /// <summary>
        /// 刷新学生卡片
        /// </summary>
        private void UpdateStudentCard()
        {
            var students = saveObject.SaveData.studentUnits;
            while (students.Count > studentCardControls.Count)
            {
                var control = Instantiate(cardPrefab, cardParent).GetComponent<SelectStudentCardControl>();
                studentCardControls.Add(control);
            }

            foreach (var control in studentCardControls)
            {
                control.gameObject.SetActive(false);
            }

            for (int i = 0; i < students.Count; i++)
            {
                //词条会显示心情和音乐数值
                var grade = new List<Grade>()
                {
                    new Grade() { name = "心情", score = students[i].Mood },
                    students[i].interestGrade.Find(grade => grade.gradeID == "10")
                };
                bool isOn = selectStudentUnits.Contains(students[i]);
                studentCardControls[i].UpdateUI(students[i], grade, OnCard, isOn);
            }
        }

        public void SetDay0(bool isDay)
        {
            isDay0 = isDay;
            UpdateUI();
        }

        public void SetDay6(bool isDay)
        {
            isDay6 = isDay;
            UpdateUI();
        }

        /// <summary>
        /// 点击了卡片
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="isOn">此次按下后，卡片的勾选状态</param>
        private void OnCard(StudentUnit unit, bool isOn)
        {
            if (isOn)
            {
                selectStudentUnits.Add(unit);
            }
            else
            {
                selectStudentUnits.Remove(unit);
            }

            UpdateUI();
        }

        public void OnEnterButton()
        {
            if (MoneyManager.Instance.Money < allPrice)
            {
                HintManager.Instance.AddHint(new Hint("支付失败", $"余额不足，还差{allPrice - MoneyManager.Instance.Money}元，你非常尴尬"));
            }
            else
            {
                MoneyManager.Instance.Money -= allPrice;
                foreach (var unit in selectStudentUnits)
                {
                    if (isDay0)
                    {
                        unit.schedule[time + 3].id = thisSchedule.id;
                        unit.schedule[time + 3].lockTime = thisSchedule.lockTime;

                    }

                    if (isDay6)
                    {
                        unit.schedule[time].id = thisSchedule.id;
                        unit.schedule[time].lockTime = thisSchedule.lockTime;
                    }
                }

                if (selectStudentUnits.Count<=1)
                {
                    HintManager.Instance.AddHint(new Hint("支付成功", $"{selectStudentUnits[0].fullName}会在周末前往蜂窝KTV娱乐"));
                }
                else
                {
                    HintManager.Instance.AddHint(new Hint("支付成功", $"{selectStudentUnits[0].fullName}等多名学生会在周末前往蜂窝KTV娱乐"));
                }

                callBack();
                gameObject.SetActive(false);
            }

        }
    }
}