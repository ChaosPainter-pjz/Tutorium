using UnityEngine;

namespace Basic
{
    public class DebugCs : MonoBehaviour
    {
        private string Str;
        private Vector2 v2;
        private bool IsShow;


        private void Start()
        {
            IsShow = false;
            Str = "";
            v2 = Vector2.zero;
        }


        private void Update()
        {
//当按下退格键时显示或隐藏控制台
            if (Input.GetKey(KeyCode.Backspace))
                IsShow = !IsShow;
        }


//当脚本启用时注册控制台信息输出的委托
        private void OnEnable()
        {
            Application.logMessageReceived += Application_logMessageReceived;
        }


//当脚本禁用时取消控制台信息输出的委托
        private void OnDisable()
        {
            Application.logMessageReceived -= Application_logMessageReceived;
        }


        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
//输入控制台的信息
            Str += condition + "\n" + stackTrace +
                   "\n---------------------------------------------------------------\n";
        }


        private void OnGUI()
        {
//绘制控制台窗口
            if (IsShow)
            {
                v2 = GUILayout.BeginScrollView(v2, GUILayout.MinWidth(Screen.width - 5), GUILayout.MaxHeight(400));
                GUILayout.TextArea(Str, GUILayout.MinWidth(Screen.width - 100));
                GUILayout.EndScrollView();
            }
        }
    }
}