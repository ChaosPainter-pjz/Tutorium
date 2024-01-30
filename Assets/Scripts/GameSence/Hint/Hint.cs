using System;
using GameSence.Date;
using UnityEngine;

namespace GameSence.Hint
{
    /// <summary>
    /// 浮动消息体
    /// </summary>
    [Serializable]
    public class Hint
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        [SerializeField] public string Headline;

        /// <summary>
        /// 消息内容
        /// </summary>
        [SerializeField] public string Text;

        [SerializeField] public Unit.Date Date;

        public Hint(string _headline, string _text)
        {
            Headline = _headline;
            Text = _text;
            try
            {
                Date = DatetimeManager.Instance.DateTime.Copy();
            }
            catch (Exception)
            {
                Date = new Unit.Date(DatetimeManager.Instance.InitYear, 0, 0, 0);
                Debug.Log("引用游戏时间错误");
            }
        }
    }
}