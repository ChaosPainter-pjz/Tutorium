using UnityEngine;
using UnityEngine.UI;

namespace WorldGame
{
    /// <summary>
    /// 小游戏选人界面，小卡片下显示学生属性的词条控制器
    /// </summary>
    public class ScoreCardControl : MonoBehaviour
    {
        [SerializeField] private Text scoreName;
        [SerializeField] private Text value;

        public void UpdateUI(string _scoreName, string _value)
        {
            gameObject.SetActive(true);
            scoreName.text = _scoreName;
            value.text = _value;
        }
    }
}