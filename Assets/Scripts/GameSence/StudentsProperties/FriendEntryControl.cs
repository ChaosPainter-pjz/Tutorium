using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 人际关系列表的词条
/// </summary>
public class FriendEntryControl : MonoBehaviour
{
    [SerializeField] private Image headPortrait;
    [SerializeField] private Text fullName;
    [SerializeField] private Text friendshipValue;
    [SerializeField] private Text messageLogging;
    private StudentsList studentsList;
    private NpcList npcList;
    public Relationship relationship;
    public void UpdateUI(Relationship relationship)
    {
        gameObject.SetActive(true);
        this.relationship = relationship;
        if (studentsList==null)
        {
            studentsList = GameManager.Instance.StudentsList;
            npcList = GameManager.Instance.NpcList;
        }
        friendshipValue.text = relationship.value.ToString();
        messageLogging.text = relationship.messageLogging;
        StudentsList.Row row = studentsList.Find_id(relationship.id);
        if (row!=null)
        {
            fullName.text = row.name;
        }else
        {
            NpcList.Row findID = npcList.Find_id(relationship.id);
            if (findID!=null)
            {
                fullName.text = findID.name;
            }
        }
        //加载头像
        foreach (var sprite in ResourceManager.Instance.npcHeadPortraits)
        {
            if (sprite.name != relationship.id) continue;
            headPortrait.sprite = sprite;
            return;
        }
    }
}