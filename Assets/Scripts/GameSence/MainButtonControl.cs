using System;
using System.Linq;
using Basic;
using GameSence.Plan;
using GameSence.PlayerProperties;
using GameSence.Schedule;
using GameSence.StoryLine;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence
{
    /// <summary>
    /// 主场景右下角三个按钮的控制器
    /// </summary>
    public class MainButtonControl : MonoBehaviour
    {
        [SerializeField] private SaveObject saveObject;
        [SerializeField] private AudioSource mainButtonAudio;
        [Header("学生安排")] [SerializeField] private Button scheduleButton;
        [SerializeField] private GameObject scheduleRed;
        [SerializeField] private GameObject schedulePanel;
        [NonSerialized] public bool IsRemindSchedule = false;
        [Header("工作计划")] [SerializeField] private Button planButton;
        [SerializeField] private GameObject planRed;
        [SerializeField] private GameObject planPanel;
        [NonSerialized] public bool IsRemindPlan;
        [Header("玩家面板")] [SerializeField] private PlayerPropertiesManager propertiesManager;

        [Header("继续")] [SerializeField] private GameObject executeButton;
        private bool isExecute;

        [SerializeField] private Image image;
        [SerializeField] private GameObject executeText;
        [Header("0启用，1禁用")] [SerializeField] private Sprite[] executeSprites;

        [SerializeField] private Text date;
        [Header("打印成绩单")] [SerializeField] private GameObject cjdButton;
        [SerializeField] private TranscriptControl transcriptControl;

        private float time = 1f;

        private void Update()
        {
            if (time < 1f)
            {
                time = 2f;
                ExecuteButtonUpdate();
            }
            else
            {
                time -= Time.deltaTime;
            }
        }

        /// <summary>
        /// 设置继续按钮
        /// </summary>
        private void ExecuteButtonUpdate()
        {
            //如果打印成绩单的按钮是激活的，那么现在不能点击继续按钮
            if (cjdButton.activeSelf)
            {
                executeButton.SetActive(false);
                return;
            }
            else
            {
                if (!executeButton.activeSelf) executeButton.SetActive(true);
            }

            //根据日程、计划的状态，更新继续按钮的状态
            var isS = ScheduleButtonUpdate();
            var isP = PlanButtonUpdate();
            isExecute = !(isP || isS);
            if (isP || isS)
            {
                image.sprite = executeSprites[1];
            }
            else
            {
                image.sprite = executeSprites[0];
                executeText.SetActive(false);
            }

            //更新小字内容
            date.text = saveObject.SaveData.gameDate.WhatDay switch
            {
                0 => "从星期日到星期五",
                5 => "从星期五到星期日",
                _ => date.text
            };
        }

        private bool ScheduleButtonUpdate()
        {
            if (saveObject.SaveData.studentUnits.Count == 0)
            {
                scheduleButton.gameObject.SetActive(false);
                return false;
            }

            if (!scheduleButton.gameObject.activeSelf) scheduleButton.gameObject.SetActive(true);
            if (saveObject.SaveData.gameDate.WhatDay != 5)
            {
                scheduleRed.SetActive(false);
                return false;
            }

            if (IsRemindSchedule)
            {
                scheduleRed.SetActive(true);
                return true;
            }

            foreach (var studentUnit in saveObject.SaveData.studentUnits)
                if (studentUnit.schedule.Any(schedule => schedule.id == "0"))
                {
                    scheduleRed.SetActive(true);
                    return true;
                }

            scheduleRed.SetActive(false);
            return false;
        }

        /// <summary>
        /// 工作计划按钮的状态更新
        /// </summary>
        private bool PlanButtonUpdate()
        {
            if (saveObject.SaveData.gameDate.WhatDay != 0)
            {
                planRed.SetActive(false);
                return false;
            }

            if (IsRemindPlan)
            {
                planRed.SetActive(true);
                return true;
            }

            var haveHint = saveObject.SaveData.playerPlan.Any(str => str == "0");
            planRed.SetActive(haveHint);
            return haveHint;
        }

        public void OnScheduleButton()
        {
            IsRemindSchedule = false;
            ScheduleManager.Instance.UIUpdate();
            schedulePanel.SetActive(true);
            mainButtonAudio.Play();
        }

        public void OnPlanButton()
        {
            IsRemindPlan = false;
            PlanManger.Instance.UIUpdateAll();
            planPanel.SetActive(true);
            mainButtonAudio.Play();
        }

        public void OnPlayerButton()
        {
            propertiesManager.OnEnterPanel();
            mainButtonAudio.Play();
        }

        public void OnExecuteButton()
        {
            if (isExecute)
            {
                ExecuteManager.Instance.OnClick();
                mainButtonAudio.Play();
            }
            else
            {
                executeText.SetActive(true);
            }
        }

        /// <summary>
        /// 点击成绩单按钮
        /// </summary>
        public void OnCjdButton()
        {
            transcriptControl.gameObject.SetActive(true);
            mainButtonAudio.Play();
            cjdButton.SetActive(false);
            saveObject.SaveData.isTranscript = false;
        }
    }
}