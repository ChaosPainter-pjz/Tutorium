using System.Collections.Generic;
using Basic;
using Basic.CSV2Table;
using Unit;
using UnityEngine;

namespace GameSence.Schedule
{
    /// <summary>
    /// 选择课程面板的管理器
    /// </summary>
    public class SelectManager : MonoInstance<SelectManager>
    {
        [SerializeField] private GameManager.GameManager gameManager;
        [SerializeField] private ScheduleManager scheduleManager;
        [SerializeField] private ParticularsPanelControl particularsPanelControl;

        [SerializeField] private Camera uiCamera;

        //拖拽物控制器
        [SerializeField] private DragControl dragControl;
        [SerializeField] private AudioSource audioSourceUp;
        [SerializeField] private AudioSource audioSourceDown;
        [SerializeField] public Animator animator;
        private static readonly int IsOpen = Animator.StringToHash("isOpen");
        [Header("选择面板")] [SerializeField] private Transform mainGradeParent;
        [SerializeField] private GameObject mainGradePrefab;
        private List<SelectCardControl> mainGradeList;
        [SerializeField] private Transform interestGradeParent;
        [SerializeField] private GameObject interestGradePrefab;
        private List<SelectCardControl> interestGradeList;
        [SerializeField] private Transform otherGradeParent;
        [SerializeField] private GameObject otherGradePrefab;
        private List<SelectCardControl> otherGradeList;

        private List<StudentCourse> masterStudentCourse;

        public CourseList.Row row;

        //正在拖拽？
        private bool isDrag = false;
        private Vector3 pos; //控件初始位置
        private Vector2 mousePos; //鼠标初始位置
        [SerializeField] private RectTransform canvasRec; //控件所在画布


        public override void Awake()
        {
            base.Awake();
            gameManager.InitGameEvent += Init;
        }

        /// <summary>
        /// 指针在card上按下
        /// </summary>
        /// <param name="row"></param>
        /// <param name="position">被按下的按钮的位置</param>
        public void OnPointerDown(CourseList.Row row, Vector3 position)
        {
            this.row = row;
            isDrag = true;
            dragControl.UpdateUI(row);
            dragControl.gameObject.SetActive(true);
            dragControl.transform.position = position;
            //控件所在画布空间的初始位置
            pos = dragControl.GetComponent<RectTransform>().anchoredPosition;
            //将屏幕空间鼠标位置eventData.position转换为鼠标在画布空间的鼠标位置
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRec, Input.mousePosition, uiCamera,
                out mousePos);
            audioSourceUp.Play();
        }

        /// <summary>
        /// 指针被松开
        /// </summary>
        public void OnPointerUp()
        {
            isDrag = false;
            scheduleManager.LockNearestCard(dragControl.transform.position);
            dragControl.gameObject.SetActive(false);
            audioSourceDown.Play();
        }

        private void Update()
        {
            if (isDrag)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 newVec;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRec, Input.mousePosition, uiCamera,
                        out newVec);
                    //鼠标移动在画布空间的位置增量
                    var offset = new Vector3(newVec.x - mousePos.x, newVec.y - mousePos.y, 0);
                    //原始位置增加位置增量即为现在位置
                    var dragTransform = dragControl.transform;
                    ((RectTransform)dragTransform).anchoredPosition = pos + offset;
                    scheduleManager.LightNearestCard(dragTransform.position);
                }
                else
                {
                    OnPointerUp();
                }
            }
        }

        /// 刷新Toggle的显示
        public void UpdateUI()
        {
            CourseUIUpdate();
        }

        /// <summary>
        /// 根据指定的CourseList.Row（课程），结合存档中的教研技能列表。返回符合技能加成的加成数据
        /// </summary>
        private void Init()
        {
            masterStudentCourse = gameManager.saveObject.SaveData.playerUnit.masterStudentCourse;
            mainGradeList = new List<SelectCardControl>();
            interestGradeList = new List<SelectCardControl>();
            otherGradeList = new List<SelectCardControl>();
            particularsPanelControl.Init();
            InitCard();
        }

        /// <summary>
        /// 初始化可选课程的toggle
        /// </summary>
        private void InitCard()
        {
            void InstantiateToggle(CourseList.Row row, GameObject prefab, Transform parent)
            {
                var control = Instantiate(prefab, parent).GetComponent<SelectCardControl>();
                control.Init(row);
                mainGradeList.Add(control);
            }

            foreach (var row in gameManager.CourseList.GetRowList())
                switch (row.Type)
                {
                    case "M":
                        InstantiateToggle(row, mainGradePrefab, mainGradeParent);
                        break;
                    case "I":
                        InstantiateToggle(row, interestGradePrefab, interestGradeParent);
                        break;
                    case "O":
                        InstantiateToggle(row, otherGradePrefab, otherGradeParent);
                        break;
                    case "N":
                        break;
                    default:
                        Debug.Log("未知的课程类型" + row.Type);
                        break;
                }
        }

        /// 刷新toggle的显示状态，让新解锁的课程可以选择
        private void CourseUIUpdate()
        {
            //显示拥有的课程
            foreach (var masterCourse in masterStudentCourse)
            {
                var mainControl = mainGradeList.Find(x => x.row.Id == masterCourse.id);
                if (mainControl)
                {
                    mainControl.gameObject.SetActive(true);
                    continue;
                }

                var interestControl = interestGradeList.Find(x => x.row.Id == masterCourse.id);
                if (interestControl)
                {
                    interestControl.gameObject.SetActive(true);
                    continue;
                }

                var otherControl = otherGradeList.Find(x => x.row.Id == masterCourse.id);
                if (otherControl) otherControl.gameObject.SetActive(true);
            }
        }

        // 点击详情按钮，打开详情面板
        public void OpenParticularsPanel(CourseList.Row row)
        {
            particularsPanelControl.OpenPanel(row);
        }
    }
}