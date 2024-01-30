using System.Collections.Generic;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.StudentsProperties
{
    public class IndicatingPointsControl : MonoBehaviour
    {
        [SerializeField] private Text number;
        [SerializeField] private List<IndicatingControl> indicatingList;
        public List<string> indicatingNow;

        private void Start()
        {
            GetComponentInParent<StudentPropertiesControl>().UIUpdateEvent += UIUpdate;
            UIUpdate();
        }

        private void UIUpdate()
        {
            var studentUnit = GetComponentInParent<StudentPropertiesControl>().studentUnit;
            number.text = (studentUnit.indicatingPoints - studentUnit.indicatingNow.Count).ToString();
        }
    }
}