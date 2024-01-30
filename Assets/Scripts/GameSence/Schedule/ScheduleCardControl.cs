using System;
using GameSence.Hint;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.Schedule
{
    public class ScheduleCardControl : MonoBehaviour
    {
        [SerializeField] private GameObject courseCard;
        [SerializeField] private Text courseName;
        [SerializeField] private Image courseImage;
        [SerializeField] public bool isSet;
        private bool isClassroom;

        /// <summary>
        /// 当前卡片中显示的日程
        /// </summary>
        public Unit.Schedule studentSchedule;

        /// <summary>
        /// 课程列表中的某一课程
        /// 拖来拖去的那个
        /// </summary>
        public StudentCourse studentCourse;


        public void Init(Unit.Schedule schedule, StudentCourse course, bool isClassroom)
        {
            this.isClassroom = isClassroom;
            studentSchedule = schedule;
            if (course == null) Debug.Log("空" + schedule.id);

            studentCourse = course;
            UpdateUI();
        }

        /// <summary>
        /// 更新临时UI,不记录得到的course
        /// </summary>
        /// <param name="course"></param>
        public void UpdateTemporaryUI(StudentCourse course)
        {
            if (!isSet || studentSchedule.lockTime > 0)
                return;
            if (isClassroom && course.type != StudentCourse.Type.Main) return;

            UpdateUI(course);
            courseImage.color = new Color(0.8f, 0.8f, 0.8f);
            courseCard.SetActive(true);
        }

        /// <summary>
        /// 按持有的studentCourse，刷新UI
        /// </summary>
        public void UpdateUI()
        {
            UpdateUI(studentCourse);
            courseImage.color = new Color(1f, 1f, 1f);
        }

        /// <summary>
        /// 按照所给的course刷新UI
        /// </summary>
        /// <param name="course"></param>
        private void UpdateUI(StudentCourse course)
        {
            if (course.id == "0")
            {
                courseCard.SetActive(false);
                return;
            }

            courseName.text = course.name;
            if (!isClassroom)
                foreach (var control in ScheduleManager.Instance.classroomScheduleControl.scheduleCards)
                    if (control.name == gameObject.name)
                        if (control.studentSchedule.id == studentSchedule.id)
                            courseName.text += "<b>+</b>";

            courseImage.sprite = course.type switch
            {
                StudentCourse.Type.Main => ScheduleManager.Instance.courseImage[0],
                StudentCourse.Type.Interest => ScheduleManager.Instance.courseImage[1],
                StudentCourse.Type.Other => ScheduleManager.Instance.courseImage[2],
                _ => throw new ArgumentOutOfRangeException()
            };
            courseCard.SetActive(true);
        }

        //更新安排的课程，抬手
        public void UpdateCourse(StudentCourse course)
        {
            if (!isSet || studentSchedule.lockTime > 0)
                return;
            if (isClassroom && course.type != StudentCourse.Type.Main) return;

            studentSchedule.id = course.id;
            studentCourse = course;
            ScheduleManager.Instance.UIUpdate();
        }

        /// <summary>
        /// 取消被吸附选中的状态
        /// </summary>
        public void CancelTemporaryUI()
        {
            ScheduleManager.Instance.UIUpdate();
        }

        /// <summary>
        /// 单击，取消这个时间段的安排
        /// </summary>
        public void CancelCourse()
        {
            if (studentSchedule.lockTime <= 0)
            {
                studentSchedule.id = "0";
                studentCourse.id = "0";
                CancelTemporaryUI();
            }
            else
            {
                HintManager.Instance.AddHint(new Hint.Hint("提示", "该日程已被锁定，剩余周数：" + studentSchedule.lockTime));
            }
        }
    }
}