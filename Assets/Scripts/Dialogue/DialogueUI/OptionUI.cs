using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    private Button _button;

    private TextMeshProUGUI _textMeshProUGUI;
    
    private DialoguePiece currentPiece;
    private string _nextPieceID;
    private bool _takeQuest;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _textMeshProUGUI = transform.GetComponentInChildren<TextMeshProUGUI>(true);
        _button.onClick.AddListener(OnOptionBtnClick);
    }
    
    /// <summary>
    /// 更新option选项框
    /// </summary>
    /// <param name="piece">对话片段</param>
    /// <param name="option">每个片段对话对应的几个选项</param>
    public void UpdateOption(DialoguePiece piece,DialogueOption option)
    {
        currentPiece = piece;
        _textMeshProUGUI.text = option.text;
        _nextPieceID = option.targetID;
        _takeQuest = option.takeQuest;
    }
    
    private void OnOptionBtnClick()
    {
        //判断有没有quest  
        if (currentPiece.questData!=null)
        {
            var newTask = new QuestManager.QuestTask
            {
                //你再不实例化一个试试看呢
                questData = Instantiate(currentPiece.questData)
            };
            //等效写法
            /*var newTask1 = new QuestManager.QuestTask();
            newTask1.questData = currentPiece.quest;*/
            
            //在每个对话片段可能有任务，但选项里不是每个选项都有任务，得对应的勾选takeQuest的选项来将quest加到QuestManager
            //如果该选项是执行任务的，将任务加进任务列表
            if (_takeQuest)
            {
                //判断某个任务是否已经存在
                if (QuestManager.Instance.HavaQuestTask(newTask.questData))
                {
                    //判段是否完成任务给予奖励
                    if (QuestManager.Instance.GetQuestTask(newTask.questData).IsComplete)
                    {
                        newTask.questData.GiveRewards();
                        QuestManager.Instance.GetQuestTask(newTask.questData).IsFinished = true;
                    }
                }
                else
                {
                    //添加任务到列表
                    QuestManager.Instance.questTasks.Add(newTask);
                    //设置任务的状态为开始状态
                    //一定要从QuestManager里去拿和当前任务匹配的任务，不然改的是newTask临时变量
                    QuestManager.Instance.GetQuestTask(newTask.questData).IsStarted = true;
                    //添加任务时检测包裹中是否已经有任务所需的东西
                    foreach (var questItem in newTask.questData.GetQuestRequireNames())
                    {
                        InventoryManager.Instance.CheckQuestRequireItemInInventory(questItem);
                    }
                }
            }
        }
        //如果有下一个对话内容则更新内容，如果没有就关闭对话框
        if (_nextPieceID=="")
        {
            DialogueUIManager.Instance.dialoguePanel.SetActive(false);
        }
        else
        {
            DialogueUIManager.Instance.UpdateDialogueUI(DialogueUIManager.Instance.currentData.dialogueIndex[_nextPieceID]);
        }
    }
    
}
