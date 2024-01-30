using UnityEngine;
using UnityEngine.UI;

namespace GameSence.PlayerProperties
{
    /// <summary>
    /// 时间轴，学生卡片节点控制器
    /// </summary>
    public class StudentNodeControl : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Text text;
        public TimerShaftStudentNode node;

        public void Init(TimerShaftStudentNode node)
        {
            this.node = node;
            foreach (var sprite in PlayerPropertiesManager.Instance.studentHeadPortrait)
            {
                if (sprite.name != this.node.studentID) continue;
                image.sprite = sprite;
                break;
            }

            text.text = this.node.text;
        }
    }
}