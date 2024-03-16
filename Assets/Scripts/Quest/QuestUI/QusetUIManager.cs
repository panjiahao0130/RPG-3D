using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class QusetUIManager : Singleton<QusetUIManager>
{

    [Header("Elements")]
    public GameObject panel;
    public ItemTooltip tooltip;
    private bool isOpen;
    
    [Header("Quest Name")] 
    public RectTransform questListTransform;

    public QuestNameButton questNameButton;
    [Header("Requirement")] 
    public RectTransform requirementListTransform;

    public Requirement requirement;
    [Header("QuestDescription")] 
    public TextMeshProUGUI questDescriptionTxt;
    
    [Header("RewardItem")] 
    public RectTransform rewardListTransform;
    public ItemUI rewardItemUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;
            //打开任务面板
            panel.gameObject.SetActive(isOpen);
            if (isOpen)
            {
                //将描述清空
                questDescriptionTxt.text = string.Empty;
                //重置任务面板
                ResetQuestUI();
                //初始化任务列表按钮
                InitQuestList();
            }

            //关闭任务的时候tooltip要关掉
            if (!isOpen)
            {
                tooltip.gameObject.SetActive(false);
            }
            
        }
    }
    
    /// <summary>
    /// 重置任务面板
    /// </summary>
    public void ResetQuestUI()
    {
        foreach (Transform item in questListTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in requirementListTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in rewardListTransform)
        {
            Destroy(item.gameObject);
        }
    }
    
    /// <summary>
    /// 初始化任务列表
    /// </summary>
    private void InitQuestList()
    {
        foreach (var task in QuestManager.Instance.questTasks)
        {
            var newTask = Instantiate(questNameButton, questListTransform);
            //设置任务按钮
            newTask.SetupQuestNameButton(task.questData);
        }
    }

    /// <summary>
    /// 设置任务的描述
    /// </summary>
    /// <param name="questData"></param>
    public void SetupQuestDescription(QuestData_SO questData)
    {
        questDescriptionTxt.text = questData.questDescription;
    }

    /// <summary>
    /// 初始化任务需求
    /// </summary>
    /// <param name="questData"></param>
    public void InitQuestRequireList(QuestData_SO questData)
    {
        foreach (Transform item in requirementListTransform)
        {
            Destroy(item.gameObject);
        }
        
        foreach (var require in questData.questRequires)
        {
            var newRequire = Instantiate(requirement, requirementListTransform);
            //设置所需列表UI
            newRequire.SetupRequirement(require.requireName,require.requireAmount,require.currentAmount);
            if (questData.isQuestFinished)
            {
                newRequire.SetupRequirement(questData);
            }
        }
    }

    /// <summary>
    /// 初始化奖励
    /// </summary>
    /// <param name="questData"></param>
    public void InitRewardList(QuestData_SO questData)
    {
        foreach (Transform item in rewardListTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (var reward in questData.questRewards)
        {
            var newReward = Instantiate(rewardItemUI, rewardListTransform);
            //设置奖励列表的UI
            newReward.SetUpItemUI(reward.itemData,reward.amount);
        }
    }
}
