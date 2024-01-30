using System.Collections.Generic;
using System.Linq;
using GameSence.Hint;
using Unit;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameSence.StudentsProperties.分班
{
    /// <summary>
    /// 分班面板，单个学生卡片的控制器
    /// </summary>
    public class DivideStudentCardControl : MonoBehaviour
    {
        [SerializeField] private TrustControl trustControl;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<Button> buttons;
        [SerializeField] private Text studentName;
        [SerializeField] private Text approveText;
        private StudentUnit studentUnit;

        // private void Start()
        // {
        //     Init(GameManager.Instance.saveObject.SaveData.studentUnits[0]);
        // }

        public void Init(StudentUnit unit)
        {
            gameObject.SetActive(true);
            studentUnit = unit;
            UpdateUI();
        }


        public void UpdateUI()
        {
            studentName.text = studentUnit.fullName;
            for (var i = 0; i < buttons.Count; i++)
            {
                buttons[i].interactable = studentUnit.NCEESelect.All(grade => grade.gradeID != buttons[i].name);
                var grade = studentUnit.mainGrade[i + 3];
                buttons[i].GetComponentInChildren<Text>().text = $"{grade.name}{grade.score}";
            }

            trustControl.InitTrust(studentUnit);
            //下面还要计算认同率
            var ncee = studentUnit.NCEESelect;
            var noNcee = new List<Grade>();
            for (var i = 3; i < studentUnit.mainGrade.Count; i++)
                if (studentUnit.NCEESelect.All(grade => grade.gradeID != studentUnit.mainGrade[i].gradeID))
                    noNcee.Add(studentUnit.mainGrade[i]);

            var nceeAllScore = ncee.Sum(grade => grade.GradeScore);
            var noNceeAllScore = noNcee.Sum(grade => grade.GradeScore);
            approveText.text = ((nceeAllScore + 0f) / (nceeAllScore + noNceeAllScore + 0f)).ToString("P0");
        }

        public void OnButton(Button button)
        {
            while (studentUnit.NCEESelect.Count >= 3) studentUnit.NCEESelect.RemoveAt(0);

            studentUnit.NCEESelect.Add(studentUnit.mainGrade[int.Parse(button.name)]);
            // if (studentUnit.NCEESelect.Any(grade => grade.gradeID ==button.name))//这时这个toggle没有点亮
            // {
            //     int index = studentUnit.NCEESelect.FindIndex(grade => grade.gradeID == button.name);
            //     studentUnit.NCEESelect.RemoveAt(index);
            // }
            // else//这时这个toggle已经点亮
            // {
            //     while (studentUnit.NCEESelect.Count>=3)
            //     {
            //         studentUnit.NCEESelect.RemoveAt(0);
            //     }
            //
            //     studentUnit.NCEESelect.Add(studentUnit.mainGrade[int.Parse(button.name)]);
            // }
            audioSource.Play();
            UpdateUI();
        }

        /// <summary>
        /// 学生选择学科
        /// </summary>
        public void ChooseSubject()
        {
            var ncee = studentUnit.NCEESelect;
            var noNcee = new List<Grade>();
            var allNcee = new List<Grade>();
            for (var i = 3; i < studentUnit.mainGrade.Count; i++)
                if (studentUnit.NCEESelect.All(grade => grade.gradeID != studentUnit.mainGrade[i].gradeID))
                    noNcee.Add(studentUnit.mainGrade[i]);

            for (var i = 3; i < studentUnit.mainGrade.Count; i++) allNcee.Add(studentUnit.mainGrade[i]);

            float nceeAllScore = ncee.Sum(grade => grade.GradeScore);
            float noNceeAllScore = noNcee.Sum(grade => grade.GradeScore);
            var random = Random.Range(0f, 1f);
            if (nceeAllScore / (nceeAllScore + noNceeAllScore) <= random) //学生自主选择
            {
                var maxGrade = allNcee[0];
                foreach (var grade in allNcee)
                    if (grade.GradeScore > maxGrade.GradeScore)
                        maxGrade = grade;
                var nMaxGrade = new Grade() { score = 0 };
                foreach (var grade in ncee)
                    if (grade.GradeScore > nMaxGrade.GradeScore && grade.gradeID != maxGrade.gradeID)
                        nMaxGrade = grade;

                Grade sGrade = null;
                switch (studentUnit.id)
                {
                    case "1":
                        SelfGrade("历史");
                        break;
                    case "2":
                        SelfGrade("物理");
                        break;
                    case "3":
                        SelfGrade("地理");
                        break;
                    case "4":
                        SelfGrade("政治");
                        break;
                }

                //根据学生本身意愿设置志愿
                void SelfGrade(string gradeName)
                {
                    if (maxGrade.name != gradeName && nMaxGrade.name != gradeName)
                        sGrade = allNcee.Find(grade => grade.name == gradeName);
                }

                //意愿提前实现，这里根据老师推荐来确认第三学科
                if (sGrade == null)
                    foreach (var grade in ncee)
                        if (grade.gradeID != nMaxGrade.gradeID && grade.gradeID != maxGrade.gradeID)
                            sGrade = grade;

                //老师推荐的也提前实现，那就随意选一个
                if (sGrade == null)
                    for (var i = 0; i < allNcee.Count; i++)
                    {
                        var grade = allNcee[i];
                        if (grade.gradeID != nMaxGrade.gradeID && grade.gradeID != maxGrade.gradeID)
                        {
                            if (sGrade == null)
                            {
                                sGrade = grade;
                            }
                            else
                            {
                                Random.InitState(i);
                                if (Random.Range(0, 3) == 0) sGrade = grade;
                            }
                        }
                    }

                ncee[0] = maxGrade;
                ncee[1] = nMaxGrade;
                ncee[2] = sGrade;
            }

            HintManager.Instance.AddHint(new Hint.Hint("分班结果",
                $"{studentUnit.fullName}结果深思熟虑，终于分班完成，TA选择了{ncee[0].name}+{ncee[1].name}+{ncee[2].name}"));
        }
    }
}