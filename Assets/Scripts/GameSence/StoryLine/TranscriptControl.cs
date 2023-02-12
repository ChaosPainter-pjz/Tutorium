using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WorldGame;

/// <summary>
/// 成绩单页面控制器
/// </summary>
public class TranscriptControl : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject timeStream;
    [SerializeField] private Text headlineTime;
    [SerializeField] private Text studentName;
    [SerializeField] private TranscriptGradeCardControl[] transcriptGradeCardControls;
    [SerializeField] private Text overall;
    [SerializeField] private Text teacher;

    [SerializeField] private Text self;

    [SerializeField] private Text player;
    [SerializeField] private GameObject enterbutton;
    private SaveData saveData;
    private StudentUnit thisStudent;
    private int studentNumber = 0;
    // void Start()
    // {
    //     saveData = GameManager.Instance.saveObject.SaveData;
    //     if (saveData.studentUnits==null||saveData.studentUnits.Count==0)
    //     {
    //         //return;
    //     }
    //     UpdateUI(saveData.studentUnits[0]);
    //     FallSemester();
    // }

    private void OnEnable()
    {
        saveData ??= GameManager.Instance.saveObject.SaveData;
        studentNumber = 0;
        UpdateUI(saveData.studentUnits[0]);
    }

    public void UpdateUI(StudentUnit _thisStudent)
    {
        thisStudent = _thisStudent;
        Date date = saveData.gameDate;
        if (thisStudent!=null)
        {
            headlineTime.text = date.ToString(Date.ToStringType.YearAndSemester) + "-成绩单";
            studentName.text = thisStudent.fullName;
            int overallInt = 0;//总分


            if (date.year - thisStudent.enrollmentYear==0)//第一年
            {
                for (int i = 0; i < transcriptGradeCardControls.Length; i++)
                {
                    int score = thisStudent.mainGrade[i].GradeScore;
                    overallInt += score;
                    transcriptGradeCardControls[i].UpdateUI(thisStudent.mainGrade[i]);
                }
            }
            else
            {
                foreach (var control in transcriptGradeCardControls)
                {
                    control.gameObject.SetActive(false);
                }

                for (int i = 0; i < 3; i++)
                {
                    overallInt += thisStudent.mainGrade[i].GradeScore;
                    transcriptGradeCardControls[i].UpdateUI(thisStudent.mainGrade[i]);
                }

                for (int i = 0; i < 3; i++)
                {
                    overallInt += thisStudent.NCEESelect[i].GradeScore;
                    transcriptGradeCardControls[i+3].UpdateUI(thisStudent.NCEESelect[i]);
                }
            }
            overall.text = "0分";
            teacher.text = "系统一时语塞，找不到形容这次成绩的骚话";
            self.text = "老板，没有文案我不知道写什么啊";
            //寻找符合年度、学期数、分数的row集合
            var rowsOverall = GameManager.Instance.StudentMarkList
                .FindAll_Year((date.year-saveData.InitYear+1).ToString())
                .FindAll(x => x.semester == date.Semester.ToString())
                .FindAll(x=>int.Parse(x.Total)<overallInt);
            int maxAll = -1;
            int maxTeacher = -1;
            int maxSelf = -1;
            foreach (var row in rowsOverall)
            {
                int total = int.Parse(row.Total);
                switch (row.Type)
                {
                    case "All":
                        if (total>maxAll)
                        {
                            maxAll = total;
                            overall.text = row.Content;
                        }
                        break;
                    case "Teacher":
                        if (total>maxTeacher)
                        {
                            maxTeacher = total;
                            teacher.text = row.Content;
                        }
                        break;
                    case "Self":
                        if (total>maxSelf)
                        {
                            maxSelf = total;
                            self.text = row.Content;
                        }
                        break;
                }
            }

            player.text = "";
        }
    }


    /// <summary>
    /// 发送动画结束事件调用
    /// </summary>
    public void OnEnterDelegate()
    {
        if (studentNumber>=saveData.studentUnits.Count)
        {
            gameObject.SetActive(false);
            studentNumber = 0;
        }
        else
        {
            UpdateUI(saveData.studentUnits[studentNumber]);
        }
        enterbutton.SetActive(true);
    }
    /// <summary>
    /// 点击发送按钮
    /// </summary>
    public void OnEnter()
    {
        enterbutton.SetActive(false);
        studentNumber++;
        HintManager.Instance.AddHint(new Hint("成绩单已发出",$"{thisStudent.fullName}的成绩单已发出到学生的家庭地址"));

        if (studentNumber<saveData.studentUnits.Count)
        {
            animator.SetBool("again",true);
        }
        else
        {
            animator.SetBool("again",false);
        }
        animator.SetTrigger("send");
        //Invoke(nameof(OnEnterDelegate),1.25f);
    }
    // /// <summary>
    // /// 时间流
    // /// </summary>
    // private void TimeStream()
    // {
    //     enterbutton.SetActive(true);
    //     gameObject.SetActive(false);
    //     timeStream.SetActive(true);
    //
    // }
}