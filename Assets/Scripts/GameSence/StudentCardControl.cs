using UnityEngine;
using UnityEngine.UI;

public class StudentCardControl : MonoBehaviour
{
    private StudentUnit studentUnit;
    [Header("学生ID")]
    [SerializeField] private GameObject[] headPortraits;
    /// <summary>
    /// 属性按钮
    /// </summary>
    [SerializeField] private Button propertiesButton;
    /// <summary>
    /// 房间按钮
    /// </summary>
    [SerializeField] private Button roomButton;
    private StudentPropertiesControl studentPropertiesControl;
    private StudentRoomControl studentRoomControl;

    private StudentsManager studentsManager;

    private void Start()
    {
        propertiesButton.onClick.AddListener(OpenStudentPropertyPanel);
        roomButton.onClick.AddListener(OpenStudentRoom);
    }

    public void Init(StudentUnit unit)
    {
        studentsManager ??= StudentsManager.Instance;
        studentPropertiesControl ??= studentsManager.studentPropertiesControl;
        studentRoomControl ??= studentsManager.studentRoomControl;
        studentUnit = unit;
        UIUpdate();
    }
    /// <summary>
    /// 刷新界面
    /// </summary>
    private void UIUpdate()
    {
        gameObject.SetActive(true);
        foreach (var obj in headPortraits)
        {
            obj.SetActive(obj.CompareTag(studentUnit.id));
        }
    }

    private void OpenStudentPropertyPanel()
    {
        studentPropertiesControl.IntoPanel(studentUnit);
    }

    private void OpenStudentRoom()
    {
        studentRoomControl.Init(studentUnit);
    }
}