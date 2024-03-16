using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    private Button _questNameButton;
    private TextMeshProUGUI _questNameTxt;
    [HideInInspector]
    public QuestData_SO currentQuestData;

    private void Awake()
    {
        _questNameButton = GetComponent<Button>();
        _questNameTxt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _questNameButton.onClick.AddListener(SetupQuestContent);
    }
    
    /// <summary>
    /// 设置任务内容 包括描述、需求、奖励
    /// </summary>
    private void SetupQuestContent()
    {
        QusetUIManager.Instance.SetupQuestDescription(currentQuestData);
        QusetUIManager.Instance.InitQuestRequireList(currentQuestData);
        QusetUIManager.Instance.InitRewardList(currentQuestData);
    }

    /// <summary>
    /// 设置任务按钮的ui
    /// </summary>
    /// <param name="taskQuestData"></param>
    public void SetupQuestNameButton(QuestData_SO taskQuestData)
    {
        currentQuestData = taskQuestData;
        _questNameTxt.text = currentQuestData.questName;
        //如果已经完成任务，在任务后面添加已完成标志
        if (currentQuestData.isQuestComplete)
        {
            _questNameTxt.text = currentQuestData.questName + "(已完成)";
        }
    }
}
