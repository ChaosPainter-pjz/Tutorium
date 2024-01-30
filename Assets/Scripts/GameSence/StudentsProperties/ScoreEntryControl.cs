using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.StudentsProperties
{
    /// <summary>
    /// 学生学科分数词条的控制器
    /// </summary>
    public class ScoreEntryControl : MonoBehaviour
    {
        [SerializeField] private Text scoreName;
        [SerializeField] private Text scoreNumber;
        public Grade grade;

        public bool UIUpdate(Grade grade)
        {
            this.grade = grade;
            scoreName.text = grade.name;
            scoreNumber.text = grade.score.ToString();
            gameObject.SetActive(grade.score != 0);
            return grade.score != 0;
        }
    }
}