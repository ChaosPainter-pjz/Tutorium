using System.Collections.Generic;
using System.Linq;
using Basic;
using GameSence.Hint;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.Classroom
{
    public class ClassroomControl : MonoBehaviour
    {
        [SerializeField] private SaveObject saveObject;
        [SerializeField] private Transform mainTransform;
        [SerializeField] private MoneyManager moneyManager;

        /// <summary>
        /// 场景摄像机
        /// </summary>
        [SerializeField] private Camera mainCamera;

        /// <summary>
        /// 摄像机的默认旋转
        /// </summary>
        private Vector3 mainCameraEulerAngles;

        //教室倍率
        private float ClassroomEfficiency => saveObject.SaveData.classroomEfficiency;
        [SerializeField] private Text current;
        [SerializeField] private Text next;
        [SerializeField] private Button upButton;
        [SerializeField] private Text price;
        [SerializeField] private GameObject upObj;
        [SerializeField] private GameObject enterObj;
        [SerializeField] private int level;
        [SerializeField] private GameObject scene;

        private Dictionary<int, string> classroomLevel = new()
        {
            { 0, "1.50" },
            { 1, "1.60" },
            { 2, "1.70" },
            { 3, "1.85" },
            { 4, "2.00" },
            { 5, "2.20" },
            { 6, "2.40" },
            { 7, "2.70" },
            { 8, "3.00" }
        };

        private int[] prices =
        {
            0, 5000, 8000, 10000, 13000, 15000, 18000, 20000, 21000, 22000, 23000, 24000
        };

        private void OnEnable()
        {
            mainTransform.localScale = Vector3.zero;
            mainCameraEulerAngles = mainCamera.transform.eulerAngles;
            scene.SetActive(true);
            foreach (var dic in classroomLevel)
                if (dic.Value == ClassroomEfficiency.ToString("F"))
                {
                    level = dic.Key;
                    break;
                }

            current.text = ClassroomEfficiency.ToString("F");
            if (level == classroomLevel.Keys.Max())
            {
                next.text = "MAX";
                upButton.gameObject.SetActive(false);
                price.text = "0";
            }
            else
            {
                next.text = classroomLevel[level + 1];
                upButton.gameObject.SetActive(true);
                price.text = prices[level + 1].ToString();

                upButton.interactable = moneyManager.Money >= prices[level + 1];
                upObj.SetActive(true);
                enterObj.SetActive(false);
            }

            GameManager.GameManager.Instance.bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType
                .Classroom);
        }

        private void OnDisable()
        {
            mainTransform.localScale = Vector3.one;
            mainCamera.transform.eulerAngles = mainCameraEulerAngles;
            GameManager.GameManager.Instance.bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType.Main);

            scene.SetActive(false);
        }

        public void OnUpButton()
        {
            if (upObj.activeSelf)
            {
                upObj.SetActive(false);
                enterObj.SetActive(true);
            }
            else
            {
                MoneyManager.Instance.Money -= prices[level + 1];
                saveObject.SaveData.classroomEfficiency = float.Parse(classroomLevel[level + 1]);
                HintManager.Instance.AddHint(new Hint.Hint("教室升级", $"教室提供的学习效率提升了，现在是{classroomLevel[level + 1]}倍"));
                OnEnable();
            }
        }

        public void Update()
        {
            //摄像机跟随
            var y = Mathf.Lerp(-2f, 2f, Input.mousePosition.y / Screen.height);
            var x = Mathf.Lerp(-2f, 2f, Input.mousePosition.x / Screen.width);
            //Debug.Log($"x:{x} y:{y} height:{Screen.height} width:{Screen.width}");
            mainCamera.transform.eulerAngles = mainCameraEulerAngles + new Vector3(y, -x, 0);
        }
    }
}