using System;
using UnityEngine;
using UnityEngine.UI;

namespace World
{
    /// <summary>
    /// 朋克城控制器
    /// </summary>
    public class CoexControl : MonoBehaviour
    {
        public Text taskContentText;
        public Text rewardText;

        private void OnEnable()
        {
            AudioControl.Instance.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
            OnTaskPanel();
        }
        /// <summary>
        /// 点击了返回按钮
        /// </summary>
        public void OnReturn()
        {
            AudioControl.Instance.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
            gameObject.SetActive(false);
        }

        //增加任务面板功能，可以显示、接取任务。任务内容随机生成，内容主要是指定天数内某个属性达到某个数值，任务奖励是奖励属性数值
        public void OnTaskPanel()
        {
            //显示任务列表
            //生成随机任务
            int days = UnityEngine.Random.Range(1, 7);
            int attribute = UnityEngine.Random.Range(1, 4);
            int value = UnityEngine.Random.Range(10, 21);
            string taskContent = $"在{days}天内将{attribute}属性提升至{value}";

            //显示任务内容
            taskContentText.text = $"任务内容：{taskContent}";


            //奖励属性数值
            rewardText.text = $"奖励{attribute}属性{value}点";
        }
        
        //接取任务
        private void OnFetchTask()
        {
        
        }
    }
}