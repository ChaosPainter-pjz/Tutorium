using UnityEngine;
[System.Serializable]
public class TimerShaftStudentNode
{
    [SerializeField]
    public string studentID;
    [SerializeField]
    public string text;

    public TimerShaftStudentNode(string studentID, string text)
    {
        this.studentID = studentID;
        this.text = text;
    }
}