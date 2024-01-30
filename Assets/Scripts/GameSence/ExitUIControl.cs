using UnityEngine;

namespace GameSence
{
    /// <summary>
    /// 关闭面板时，所用到的控制器
    /// </summary>
    public class ExitUIControl : MonoBehaviour
    {
        public void OnExit()
        {
            GetComponent<Animator>().Play("ExitPanel");
        }

        public void OnScheduleEExit()
        {
            GetComponent<Animator>().SetBool("isExit", true);
        }

        public void Exit()
        {
            gameObject.SetActive(false);
        }

        public void OnExitSettingPanel()
        {
            GetComponent<Animator>().Play("GameSceneSettingExitPanel");
        }
    }
}