using System.Collections.Generic;
using Basic;
using Basic.CSV2Table;
using GameSence.StudentRoom;
using GameSence.StudentsProperties;
using Unit;
using UnityEngine;

namespace GameSence
{
    /// <summary>
    /// 主界面底部学生卡片栏目的总管理器
    /// </summary>
    public class StudentsManager : MonoInstance<StudentsManager>
    {
        [SerializeField] private GameManager.GameManager gameManager;

        /// <summary>
        ///主界面底部学生卡片
        /// </summary>
        private List<StudentCardControl> studentCardControls;

        /// <summary>
        /// 学生属性面板控制器
        /// </summary>
        public StudentPropertiesControl studentPropertiesControl;

        /// <summary>
        /// 房间
        /// </summary>
        public StudentRoomControl studentRoomControl;

        [SerializeField] private GameObject studentCard;
        [SerializeField] private Transform studentCardParent;
        private List<StudentUnit> studentUnits;

        public override void Awake()
        {
            base.Awake();
            gameManager.InitGameEvent += Init;
        }

        private void Init()
        {
            studentUnits = gameManager.saveObject.SaveData.studentUnits;
            studentCardControls ??= new List<StudentCardControl>();
            UIUpdate();
        }

        /// <summary>
        /// 生成卡片、隐藏多余卡片，刷新卡片
        /// </summary>
        private void UIUpdate()
        {
            studentCardParent.gameObject.SetActive(studentUnits.Count != 0);
            while (studentUnits.Count > studentCardControls.Count)
            {
                var control = Instantiate(studentCard, studentCardParent).GetComponent<StudentCardControl>();
                studentCardControls.Add(control);
            }

            foreach (var control in studentCardControls) control.gameObject.SetActive(false);

            for (var i = 0; i < studentUnits.Count; i++) studentCardControls[i].Init(studentUnits[i]);
        }

        // private void Update()
        // {
        //     UpdateUI();
        // }

        /// <summary>
        /// 添加指定id的学生
        /// </summary>
        public void AddStudent(string id)
        {
            var unit = studentUnits.Find(x => x.id == id);

            if (unit == null)
            {
                var row = gameManager.StudentsList.Find_id(id);
                unit = new StudentUnit
                {
                    id = row.id,
                    fullName = row.name,
                    gender = row.gender switch { "男" => Gender.Man, "女" => Gender.Woman, _ => Gender.None },
                    birthday = new Unit.Date(int.Parse(row.year) + gameManager.saveObject.SaveData.gameDate.year,
                        int.Parse(row.semester), int.Parse(row.week), int.Parse(row.whatDay)),
                    enrollmentYear = gameManager.saveObject.SaveData.gameDate.year,
                    school = row.School,
                    Trust = int.Parse(row.initialTrust),
                    personalData = row.description
                };
                for (var i = 0; i < unit.properties.Count; i++) unit.properties[i].score = row.Properties[i];

                for (var i = 0; i < unit.mainGrade.Count; i++) unit.mainGrade[i].score = row.MainGrade[i];

                for (var i = 0; i < unit.interestGrade.Count; i++) unit.interestGrade[i].score = row.InterestGrade[i];

                studentUnits.Add(unit);
            }
            else
            {
                Debug.Log("重复添加同一学生，id：" + id);
            }

            UIUpdate();
        }
    }
}