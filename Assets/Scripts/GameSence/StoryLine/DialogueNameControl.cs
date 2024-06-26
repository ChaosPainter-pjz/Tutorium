using UnityEngine;
using UnityEngine.UI;

namespace GameSence.StoryLine
{
    public class DialogueNameControl : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private Image image;

        public void SetName(string dialogueName)
        {
            text.text = dialogueName == "无名" ? "" : dialogueName;

            if (dialogueName == "无名" || dialogueName == "")
                image.color = new Color(1, 1, 1, 0);
            else
                image.color = new Color(1, 1, 1, 1);
        }
    }
}