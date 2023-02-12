using System;
using System.Collections;
using System.Collections.Generic;
using PhotographyGame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WorldGame;
using Random = UnityEngine.Random;

/// <summary>
/// 摄影小游戏控制器
/// </summary>
public class PhotographyGameControl : MonoBehaviour, IInit
{
    [SerializeField] private GameObject ui;
    [SerializeField] private PhotographyResultControl photographyResultControl;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private GameObject[] markersParent;//背景父物体集.0：篮球场  1：公园  2：放学
    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private Image background;
    private int backgroundNumber;//随机得到的数字，用来保存当前游戏背景的序号
    [SerializeField] private Sprite[] sprites;//背景集.0：篮球场  1：公园  2：放学
    [SerializeField] private Text takeNumber;
    [SerializeField] private Image picture;

    /// <summary>
    /// 鼠标图片
    /// </summary>
    [SerializeField] private Texture2D mouse;

    private StudentUnit studentUnit;

    /// <summary>
    /// 指针移动的速度
    /// </summary>
    private float pointerSpeed = 1.0f;

    /// <summary>
    /// 指针移动边界
    /// </summary>
    private float criticalValue;

    private bool isMove;

    /// <summary>
    /// 是否在游玩状态（镜头移动、镜头调焦、可拍照）
    /// </summary>
    private bool IsMove
    {
        get => isMove;
        set
        {
            isMove = value;
            Cursor.SetCursor(isMove ? mouse : null, Vector2.zero, CursorMode.Auto);
        }
    }

    private bool isHaveTarget;
    private Vector2 target;
    private Vector2 current;
    private Vector2 currentVelocity;

    private float scrollbarTarget;
    private float currentTargetVelocity;
    private int blur;

    /// <summary>
    /// 回合数
    /// </summary>
    private int photoNumber;

    private List<Achievement> achievements;


    public void Init(StudentUnit _studentUnit, int _number)
    {
        //初始化游戏参数
        gameObject.SetActive(true);
        studentUnit = _studentUnit;
        photoNumber = _number;
        takeNumber.text = "第1回合";
        ui.SetActive(true);
        //初始化背景图
        backgroundNumber = Random.Range(0, 3);
        //Debug.Log("摄影序号"+backgroundNumber);
        background.sprite = sprites[backgroundNumber];
        //初始化指针移动的速度、指针移动边界
        if (studentUnit != null)
        {
            CountDifficulty(studentUnit);
        }

        //清空记分板
        achievements ??= new List<Achievement>();
        achievements.Clear();

        //初始化shader参数
        blur = Shader.PropertyToID("_Blur");

        //开始播放音乐
        GameManager.Instance.bgmAudioControl.StopPlayAll();
        GameManager.Instance.bgmAudioControl.PlaySound("口哨");
        //开始“游戏”
        IsMove = true;
    }

    /// <summary>
    /// 根据学生的属性，计算游戏的难度
    /// </summary>
    /// <param name="_studentUnit"></param>
    private void CountDifficulty(StudentUnit _studentUnit)
    {
        Grade grade = _studentUnit.interestGrade.Find(x => x.gradeID == "19");
        switch (grade.score / 100)
        {
            case 0:
                pointerSpeed = 1f;
                criticalValue = 0f;
                break;
            case 1:
                pointerSpeed = 0.95f;
                criticalValue = 0.05f;
                break;
            case 2:
                pointerSpeed = 0.9f;
                criticalValue = 0.1f;
                break;
            case 3:
                pointerSpeed = 0.85f;
                criticalValue = 0.15f;
                break;
            case 4:
                pointerSpeed = 0.8f;
                criticalValue = 0.2f;
                break;
            case 5:
                pointerSpeed = 0.6f;
                criticalValue = 0.25f;
                break;
            default:
                pointerSpeed = 0.5f;
                criticalValue = 0.25f;
                break;
        }
    }

    // private void Start()
    // {
    //     Init(new StudentUnit(), 3);
    // }

    private void Update()
    {
        MoveBackground();
        MoveScrollbar();
        // Cursor.SetCursor(IsMove ? mouse : null, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// 移动背景图
    /// </summary>
    private void MoveBackground()
    {
        if (IsMove)
        {
            if (isHaveTarget)
            {
                backgroundRectTransform.anchoredPosition = Vector2.SmoothDamp(backgroundRectTransform.anchoredPosition, this.target, ref currentVelocity, 1.8f);
            }
            else
            {
                current = backgroundRectTransform.anchoredPosition;
                float x = (current.x - Screen.width) / 2f;
                float y = (current.y - Screen.height) / 2f;
                target = new Vector2(Random.Range(-x, x), Random.Range(-y, y));
                isHaveTarget = true;
            }

            if (Vector3.Distance(this.target, backgroundRectTransform.anchoredPosition) < 10f)
            {
                isHaveTarget = false;
            }
        }
    }

    /// <summary>
    /// 移动滑块
    /// </summary>
    private void MoveScrollbar()
    {
        if (IsMove)
        {
            if (Mathf.Abs(scrollbarTarget - scrollbar.value) < 0.1f)
            {
                ChangeScrollbarTarget();
            }
            else
            {
                scrollbar.value = Mathf.SmoothDamp(scrollbar.value, scrollbarTarget, ref currentTargetVelocity, 1f, 0.3f, Time.deltaTime * pointerSpeed);
                background.material.SetFloat(blur, Mathf.Abs(scrollbar.value - 0.5f) * 2);
            }

            //Debug.Log(Mathf.Abs( scrollbarTarget - scrollbar.value ));
        }
    }

    /// <summary>
    /// 变更滑块的目标点
    /// </summary>
    private void ChangeScrollbarTarget()
    {
        scrollbarTarget = Random.Range(criticalValue, 1f - criticalValue);
    }

    /// <summary>
    /// 点击拍照
    /// </summary>
    public void OnEnter()
    {
        if (IsMove == false)
            return;
        IsMove = false;
        float score = Mathf.Pow(1 - Mathf.Abs(scrollbar.value - 0.5f) * 2, 3) * 100;
        //加上标记物的分
        score += Marker(markersParent[backgroundNumber]);
        Achievement achievement = new Achievement() {Value = score};
        //Debug.Log(achievement.Value);
        achievements.Add(achievement);
        StartCoroutine(TakePhotos());

        if (achievements.Count < photoNumber)
        {
            takeNumber.text = $"第{achievements.Count + 1}回合";
        }
        else
        {
            takeNumber.text = "结束";
        }
    }
    /// <summary>
    /// 计算标记物给出的附加分
    /// </summary>
    /// <param name="parentObj">标记物集合的父物体</param>
    /// <returns></returns>
    private int Marker(GameObject parentObj)
    {
        int score = 0;
        RectTransform[] transforms = parentObj.GetComponentsInChildren<RectTransform>();
        for (int i = 1; i < transforms.Length; i++)
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transforms[i].position);
            if (screenPoint.x > 0 && screenPoint.x < Screen.width && screenPoint.y > 0 && screenPoint.y < Screen.height)
            {
                score += int.Parse(transforms[i].name);
                //Debug.Log(transforms[i], transforms[i]);
            }
        }

        //Debug.Log("额外加分" + score);
        return score;
    }

    /// <summary>
    /// 拍照
    /// </summary>
    private IEnumerator TakePhotos()
    {
        ui.SetActive(false);

        //yield return null;
        yield return new WaitForEndOfFrame();
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        achievements[achievements.Count - 1].Texture2D = texture;
        picture.sprite = achievements[achievements.Count - 1].Sprite;
        picture.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);
        picture.gameObject.SetActive(false);
        if (achievements.Count >= photoNumber)
        {
            photographyResultControl.gameObject.SetActive(true);
            photographyResultControl.Init(achievements, studentUnit);
        }
        else
        {
            scrollbar.value = Random.Range(criticalValue, 1f - criticalValue);
            ChangeScrollbarTarget();
            ui.SetActive(true);
            IsMove = true;
        }
    }

    /// <summary>
    /// 摄影游戏成绩单位
    /// </summary>
    public class Achievement
    {
        public float Value;
        public Texture2D Texture2D;

        public int IntValue => Convert.ToInt16(Value);
        public Sprite Sprite => Sprite.Create(Texture2D, new Rect(0, 0, Texture2D.width, Texture2D.height), Vector2.zero);

        /// <summary>
        /// 返回该成绩的评价
        /// </summary>
        public int Appraise
        {
            get
            {
                if (Value < 60)
                {
                    return 0;
                }

                if (Value < 90)
                {
                    return 1;
                }

                return 2;
            }
        }
    }
}