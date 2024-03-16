using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    private DialogueController _dialogueController;
    private QuestData_SO _currentQuest;

    private void Awake()
    {
        _dialogueController = GetComponent<DialogueController>();
    }

    public DialogueData_SO startDialogue;
    public DialogueData_SO progressDialogue;
    public DialogueData_SO completeDialogue;
    public DialogueData_SO finishedDialogue;

    #region 获得任务状态
    public bool IsStarted
    {
        get
        {
            if (QuestManager.Instance.HavaQuestTask(_currentQuest))
            {
                return QuestManager.Instance.GetQuestTask(_currentQuest).IsStarted;
            }
            else return false;
        }
    }
    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HavaQuestTask(_currentQuest))
            {
                return QuestManager.Instance.GetQuestTask(_currentQuest).IsComplete;
            }
            else return false;
        }
    }
    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.HavaQuestTask(_currentQuest))
            {
                return QuestManager.Instance.GetQuestTask(_currentQuest).IsFinished;
            }
            else return false;
        }
    }
    #endregion

    private void Start()
    {
        _dialogueController.currentDialogueData = startDialogue;
        _currentQuest = _dialogueController.currentDialogueData.GetQuest();
    }

    private void Update()
    {
        //根据不同的任务状态，匹配不同的对话片段
        if (IsStarted)
        {
            if (IsComplete)
            {
                _dialogueController.currentDialogueData = completeDialogue;
            }
            else
            {
                _dialogueController.currentDialogueData = progressDialogue;
            }
        }
        if (IsFinished)
        {
            _dialogueController.currentDialogueData = finishedDialogue;
        }
    }
}
