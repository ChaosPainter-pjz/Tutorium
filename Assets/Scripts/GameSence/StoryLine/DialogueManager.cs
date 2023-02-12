using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoInstance<DialogueManager>
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ResourceManager resourceManager;
    [Header("面板绑定")]
    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private Animator scenesAnimator;
    [SerializeField] private Image scene;
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject[] selectButtons;
    [SerializeField] private Text[] selectTexts;
    [SerializeField] private GameObject inputPanel;
    [SerializeField] private InputField inputField;
    [SerializeField] private Text inputAbbName;
    [Header("肖像")]//每添加NPC，需要手动增加肖像物体，因为摆的位置可能不同，不能自动生成
    [SerializeField] private GameObject[] studentPortraits;
    [SerializeField] private GameObject[] npcPortraits;
    //[SerializeField] private GameObject playerPortraits;

    [Header("对话面板")]
    [SerializeField] private Button panelButton;
    //对象名称、对象名称背景是否显示的控制器
    [SerializeField] private DialogueNameControl fullName;
    [SerializeField] private Text dialogueText;

    PlayerUnit playerUnit;
    private StoryLineManager storyLineManager;
    private StudentsList studentsList;
    private NpcList npcList;
    private LinearPlotList linearPlotList;
    private LinearPlotList.Row currentLinearRow;
    private BranchedPlotList branchedPlotList;
    private BranchedPlotList.Row currentBranchedRow;
    private string ResultsID;
    private string plotID;
    public override void Awake()
    {
        base.Awake();
        gameManager.InitGameEvent += Init;
    }

    private void Init()
    {
        storyLineManager = StoryLineManager.Instance;
        playerUnit = gameManager.saveObject.SaveData.playerUnit;
        studentsList = gameManager.StudentsList;
        npcList = gameManager.NpcList;
        linearPlotList = gameManager.LinearPlotList;
        branchedPlotList = gameManager.BranchedPlotList;
    }
    /// <summary>
    /// 直线式剧情
    /// </summary>
    public bool StartDialogue(LinearPlotList.Row row)
    {

        if (row == null) return false;
        currentLinearRow = row;
        Basis(row.Text,row.RoleID,row.RolePortrait);
        SetPanel(PlotPanelType.Linear);
        return true;
    }
    /// <summary>
    /// 分支式剧情
    /// </summary>
    public bool StartDialogue(BranchedPlotList.Row row)
    {
        if (row == null) return false;
        currentBranchedRow = row;
        Basis(row.Text,row.RoleID,row.RolePortrait);
        SetPanel(PlotPanelType.Branched);
        return true;
    }
    /// <summary>
    /// 重命名玩家名称
    /// </summary>
    private void StartDialogue()
    {
        SetPanel(PlotPanelType.Input);
    }
    /// <summary>
    /// 更改基础信息,名字，文本，肖像
    /// </summary>
    private void Basis(string text,string roleID,string rolePortrait)
    {
        //隐藏肖像
        foreach (var obj in studentPortraits) obj.SetActive(false);
        foreach (var obj in npcPortraits) obj.SetActive(false);
        if (roleID == "Player"||roleID == "player")
        {
            fullName.SetName(gameManager.saveObject.SaveData.playerUnit.fullName);

        }
        else
        {
            var sID = studentsList.Find_id(roleID);
            var nID = npcList.Find_id(roleID);
            if (sID != null)
            {
                fullName.SetName(sID.name);

                if (rolePortrait == "default"||rolePortrait == "")
                {
                    foreach (var obj in studentPortraits)//显示肖像
                    {
                        if (!obj.CompareTag(sID.id)) continue;
                        obj.SetActive(true);
                        break;
                    }
                }
            }else if (nID != null)
            {
                fullName.SetName(nID.name);
                if (rolePortrait == "default"||rolePortrait == "")
                {
                    foreach (var obj in npcPortraits)
                    {
                        if (obj.name != roleID) continue;
                        obj.SetActive(true);
                        break;
                    }
                }
            }
            else
            {
                fullName.SetName("");
            }
        }
        string temporaryText = text.Replace("fullName",playerUnit.fullName);
        temporaryText = temporaryText.Replace("abbName", playerUnit.abbName);
        SetDialogueText(temporaryText);
    }
    /// <summary>
    /// 剧情面板类型
    /// </summary>
    private enum PlotPanelType
    {
        Linear,Branched,Input
    }
    /// <summary>
    /// 设置面板的显示
    /// </summary>
    private void SetPanel(PlotPanelType type)
    {
        switch (type)
        {
            case PlotPanelType.Linear:
                panelButton.enabled = true;
                selectPanel.SetActive(false);
                inputPanel.SetActive(false);
                break;
            case PlotPanelType.Branched:
                panelButton.enabled = false;
                selectPanel.SetActive(true);
                //保存选项指向的剧情是否为空，作为词条是否显示的依据
                bool[] isNextPlot = {
                    currentBranchedRow.NextPlotId_1!="null",
                    currentBranchedRow.NextPlotId_2!="null",
                    currentBranchedRow.NextPlotId_3!="null",
                    currentBranchedRow.NextPlotId_4!="null"};
                //保存选项的文本内容
                string[] branches =
                {
                    currentBranchedRow.Branches_1,
                    currentBranchedRow.Branches_2,
                    currentBranchedRow.Branches_3,
                    currentBranchedRow.Branches_4
                };
                for (int i = 0; i < selectButtons.Length; i++)
                {
                    selectButtons[i].gameObject.SetActive(isNextPlot[i]);
                    selectTexts[i].text = branches[i];
                }
                inputPanel.SetActive(false);
                break;
            case PlotPanelType.Input:
                panelButton.enabled = false;
                selectPanel.SetActive(false);
                inputPanel.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    /// <summary>
    /// 点击面板以继续剧情
    /// </summary>
    private void OnPanel()
    {
        if (plotID == "null" || plotID == "exit") // 结束剧情
        {
            EndPlot();
            return;
        }

        if (!StartDialogue(linearPlotList.Find_PlotId(plotID)))
        {
            if (!StartDialogue(branchedPlotList.Find_PlotId(plotID)))
            {
                EndPlot();
                return;
            }
        }
        if (plotID == "rename")
        {
            StartDialogue();
        }
    }

    public void OnRenameEnter()// 单击起名字的确认按钮
    {
        if (inputField.text == "" && SteamManager.Initialized)
        {
            inputField.text = SteamFriends.GetPersonaName();
            return;
        }

        if (inputField.text == "")
        {
            inputField.text = "佚名";
            return;
        }

        playerUnit.fullName = inputField.text;
        playerUnit.abbName = inputField.text.Substring(0, 1);
        OnClickPanel();
    }
    /// <summary>
    /// 刷新尊称的显示（起名字时）
    /// </summary>
    public void AbbNameUpdate()
    {
        if (inputField.text!="")
        {

            inputAbbName.text = $"尊敬的称呼：{inputField.text.Substring(0, 1)}老师";
        }
        else
        {
            inputAbbName.text = "";
        }
    }
    /// <summary>
    /// 结束此剧情链，尝试下一个剧情
    /// </summary>
    private void EndPlot()
    {
        dialoguePanel.SetActive(false);
        StoryLineManager.Instance.BeganPlot();
    }

    public void OnClickPanel()
    {
        if (isPrint)
        {
            StopAllCoroutines();
            dialogueText.text = "";
            foreach (var s in showLog)
            {
                dialogueText.text += s;
            }
            isPrint = false;
            return;
        }
        storyLineManager.PlotResults(currentLinearRow.ResultsA);
        storyLineManager.PlotResults(currentLinearRow.ResultsB);
        plotID = currentLinearRow.NextPlotId;
        OnPanel();
    }
    /// <summary>
    /// 单击选择项
    /// </summary>
    /// <param name="number"></param>
    public void SelectButton(int number)
    {
        plotID = number switch
        {
            1 => currentBranchedRow.NextPlotId_1,
            2 => currentBranchedRow.NextPlotId_2,
            3 => currentBranchedRow.NextPlotId_3,
            4 => currentBranchedRow.NextPlotId_4,
            _ => null
        };
        OnPanel();
    }
    /// <summary>
    /// 切换场景
    /// </summary>
    /// <param name="sceneID">场景ID</param>
    /// <param name="isSwitch">是否是从一个场景切换到另一个场景，这决定是否出现白光动画</param>
    public void SetScene(string sceneID,bool isSwitch)
    {
        Sprite image = null;
        foreach (Sprite sprite in resourceManager.scenes)
            if (sceneID == sprite.name)
                image = sprite;
        foreach (Sprite sprite in resourceManager.PhotoAlbum)
        {
            if (sceneID == sprite.name)
                image = sprite;
        }
        scene.sprite = image == null ?  resourceManager.scenes[0] : image;
        if (isSwitch)
        {
            scenesAnimator.SetTrigger("switch");
        }

    }

    /// <summary>
    /// 正在逐字显示剧情文本吗
    /// </summary>
    private bool isPrint;
    private List<string> showLog = new List<string>();
    private void SetDialogueText(string text)
    {
        StopAllCoroutines();
        isPrint = true;
        ShowLogInfo(text);
    }
    void ShowLogInfo(string _log)
    {
        dialogueText.text = "";
        showLog.Clear();
        List<List<string>> splitToList = new List<List<string>>();
        //根据尖括号分开成文字和颜色
        string[] logChar = _log.Split('<', '>');
        //结果为{"龙虾的","color=#FFFFFFFF","Text","/color",""}
        foreach (string str in logChar)
        {
            List<string> logcharArr = new List<string>();
            foreach (char cha in str)
            {
                logcharArr.Add(cha.ToString());
            }
            splitToList.Add(logcharArr);
        }
        //结果为{{龙,虾,的},{c,o,l,o,r,=,#,F,F,F,F,F,F,F,F},{T,e,x,t},{/,c,o,l,o,r},{}}
        for (int i = 0; i < logChar.Length; i++)
        {
            //当找到/color这个字段的时候,将上一个所有字符都加上颜色
            Fwb("/color");
            Fwb("/b");
            Fwb("/i");
            Fwb("/size");
            Fwb("/material");

            void Fwb(string strB)
            {
                if (string.Compare(logChar[i], strB, StringComparison.Ordinal) != 0) return;
                List<string> addColorString = new List<string>();
                List<string> addColorStingCache = new List<string>();

                //拿到需要加颜色的字符
                for (int k = 0; k < logChar[i - 1].Length; k++)
                {
                    addColorString.Add(logChar[i - 1][k].ToString());
                }

                //将字符加颜色
                foreach (string t in addColorString)
                {
                    if (t=="")
                        continue;
                    //logChar[i]是"/color"所以logChar[i-1]是需要加颜色的字符，logChar[i-2]是需要的颜色码
                    addColorStingCache.Add($"<{logChar[i - 2]}>{t}<{strB}>");
                }

                splitToList[i - 1] = addColorStingCache;

                //将颜色码的两个数据清除
                splitToList[i - 2].Clear();
                splitToList[i].Clear();
            }
        }

        //重写需要显示的数据
        foreach (List<string> t in splitToList)
        {
            foreach (string t1 in t)
            {
                showLog.Add(t1);
            }
        }
        StartCoroutine(ShowLog());
    }


    IEnumerator ShowLog()
    {
        foreach (var item in showLog)
        {
            dialogueText.text += item;
            yield return new WaitForSeconds(0.05f);
        }
        isPrint = false;
    }

}