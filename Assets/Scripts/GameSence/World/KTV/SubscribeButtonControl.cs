using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace World.KTV
{
    /// <summary>
    /// KTV中的预约按钮控制器
    /// </summary>
    public class SubscribeButtonControl : MonoBehaviour
    {
        [SerializeField] private Text text;
        private int number;
        public int price;
        //private Toggle toggle;
        //private Toggle Toggle => toggle ??= GetComponent<Toggle>();
        private Button button;
        private Button Button=>button??=GetComponent<Button>();
        public UnityAction<int,int> dele;
        public void UpdateUI(bool canBuy,int number,UnityAction<int,int> dele)
        {
            if (this.dele==null)
            {
                this.dele = dele;
                Button.onClick.AddListener(OnButton);
            }

            this.number = number;

            if (canBuy)
            {
                Button.interactable = true;
                text.text = "预约";
            }
            else
            {
                Button.interactable = false;
                text.text = "已满";

            }
        }
        /// <summary>
        /// 点击按钮事件
        /// </summary>
        private void OnButton()
        {
            dele(number,price);
        }

    }
}