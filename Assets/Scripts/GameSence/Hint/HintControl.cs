using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 浮动消息控制器
/// </summary>
public class HintControl : MonoBehaviour
{
   [SerializeField] private HintManager hintManage;
   [SerializeField] private Text headline;//标题
   [SerializeField] private Text details;//内容
   [SerializeField] private AudioSource audioSource;//点击这个面板的音效
   [SerializeField] private Animator animator;
   [SerializeField] private Text countDown;
   /// <summary>
   /// 提醒倒计时
   /// </summary>
   private float countDownFloat;
   /// <summary>
   /// 是否正在提醒
   /// </summary>
   private bool isRemind = false;
   /// <summary>
   /// 鼠标正悬停在面板上
   /// </summary>
   private bool isHover = false;

   private static readonly int Activate = Animator.StringToHash("Activate");
   private static readonly int Exit1 = Animator.StringToHash("exit");

   //点击面板
   public void OnEnter()
   {
      audioSource.Play();
      animator.SetBool(Exit1,true);
      isRemind = false;
      isHover = false;
   }

   public void UpdateUI(Hint hint)
   {
      headline.text = hint.Headline;
      details.text = hint.Text;
   }

   private void FixedUpdate()
   {
      if (isRemind)//正在提醒
      {
         animator.SetBool(Activate,true);
         animator.SetBool(Exit1,false);
      }
      else//不在提醒
      {
         animator.SetBool(Activate,false);
         animator.SetBool(Exit1,true);

         Hint hint = hintManage.DeQueue();
         if (hint!=null)
         {
            UpdateUI(hint);
            isRemind = true;
            countDownFloat = 5f;
         }
      }
   }

   private void Update()
   {
      if (isRemind && !isHover)
      {
         countDownFloat -= Time.deltaTime;
         countDown.text = countDownFloat.ToString("0");
      }

      if (countDownFloat <= 0f )
      {
         isRemind = false;
         if (hintManage.hints.Count!=0)
         {
            hintManage.audioSource.Play();
         }
      }

      if (!isRemind)
      {
         isHover = false;
      }
   }

   public void Enter()
   {
      isHover = true;
   }

   public void Exit()
   {
      isHover = false;
   }
}