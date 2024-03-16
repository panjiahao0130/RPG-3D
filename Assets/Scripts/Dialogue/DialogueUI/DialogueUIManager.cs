using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUIManager : Singleton<DialogueUIManager>
{

    [Header("Basic Elements")]
    public Image icon;

    public TextMeshProUGUI mainText;
    public Button nextButton;
    //对话页面的gameObject
    public GameObject dialoguePanel;

    [Header("Option")]
    //选项option的RectTransform
    public RectTransform optionPanel;
    //选项option的预制体
    public OptionUI optionPrefab;
    
    
    //[Header("Data")]
    [HideInInspector]
    public DialogueData_SO currentData;

    private int currentIndex = 0;
    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    /// <summary>
    /// 继续对话
    /// </summary>
    private void ContinueDialogue()
    {
        if (currentIndex<currentData.dialoguePieces.Count)
        {
            UpdateDialogueUI(currentData.dialoguePieces[currentIndex]);
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
        
    }

    /// <summary>
    /// 根据DialogueController传数据，不同的Npc的对话不一样，用的是同一个DialogueUI，所以是挂载在不同NPC上的不同的Controller传dialogueData
    /// </summary>
    /// <param name="dialogueData"></param>
    public void UpdateDialogueData(DialogueData_SO dialogueData)
    {
        currentData = dialogueData;
        currentIndex = 0;
    }
    
    /// <summary>
    /// 更新对话框，包括对话内容和选项内容
    /// </summary>
    /// <param name="piece"></param>
    public void UpdateDialogueUI(DialoguePiece piece)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        currentIndex++;
        if (piece.image!=null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
        {
            icon.enabled = false;
        }

        mainText.text = "";
        //mainText.DOColor(new Color(1.0f, 150.0f, 150.0f), 2);
        DOTween.To(()=>string.Empty,value=>mainText.text=value,piece.text,1f).SetEase(Ease.Linear);

        if (piece.dialogueOptions.Count==0&&currentData.dialoguePieces.Count>0)
        {
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
            nextButton.interactable = true;
        }
        else
        {
            //会是对话框的比例不对
            //nextButton.gameObject.SetActive(false);
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
            nextButton.interactable = false;
        }
        dialoguePanel.SetActive(true); 
        CreateOptions(piece);
    }

    /// <summary>
    /// 创建选项框
    /// </summary>
    /// <param name="piece"></param>
    private void CreateOptions(DialoguePiece piece)
    {
        //如果optionPanel下面已经有optionBtn，先全销毁了 因为点击某个option之后下一个对话可能也是有option的
        if (optionPanel.childCount>0)
        {
            for (int i = 0; i <optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }
        
        for (int i = 0; i < piece.dialogueOptions.Count; i++)
        {
            //实例化Option
            var option = Instantiate(optionPrefab, optionPanel);
            //更新Option的UI
            option.UpdateOption(piece,piece.dialogueOptions[i]);
        }
    }
}
