using Basic;
using GameSence.StoryLine;
using UnityEngine;

namespace GameSence.Date
{
    public class DayAnimationControl : MonoBehaviour
    {
        [SerializeField] private SaveObject saveObject;
        [SerializeField] private Animator animator;
        [SerializeField] private StoryLineManager storyLineManager;
        private static readonly int WorkDay = Animator.StringToHash("WorkDay");

        public void OnEnable()
        {
            if (saveObject.SaveData.gameDate.WhatDay == 0) //周末~周五
                animator.SetBool(WorkDay, true);
            else //工作日
                animator.SetBool(WorkDay, false);
        }

        private void Update()
        {
            if (storyLineManager.isPlot)
                animator.speed = 0;
            else
                animator.speed = 1;
        }
    }
}