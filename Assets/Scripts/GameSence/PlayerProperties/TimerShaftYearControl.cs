using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.PlayerProperties
{
    /// <summary>
    /// 时间轴中，只显示年份与学期的节点
    /// </summary>
    public class TimerShaftYearControl : MonoBehaviour
    {
        private Unit.Date thisDate;

        public Unit.Date ThisDate
        {
            set
            {
                thisDate = value;
                text.text =
                    $"二\n零\n{Equation.CaToCh[thisDate.year % 100 / 10]}\n{Equation.CaToCh[thisDate.year % 10]}\n年\n{(thisDate.Semester == 0 ? "上" : "下")}\n学\n期";
            }
        }

        [SerializeField] private Text text;
    }
}