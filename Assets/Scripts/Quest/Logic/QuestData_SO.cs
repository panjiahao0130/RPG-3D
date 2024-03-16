using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;

[CreateAssetMenu(fileName = "New Quest",menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire
    {
        //需要的物品名称
        public string requireName;
        public int requireAmount;
        public int currentAmount;
    }
    public string questName;
    [TextArea]
    public string questDescription;
    /// <summary>
    /// 任务是否开启 不能改这 得改task任务列表中的
    /// </summary>
    public bool isQuestStarted;
    /// <summary>
    /// 任务是否完成
    /// </summary>
    public bool isQuestComplete;
    /// <summary>
    /// 任务结束
    /// </summary>
    public bool isQuestFinished;

    public List<QuestRequire> questRequires = new List<QuestRequire>();
    public List<InventoryItem> questRewards = new List<InventoryItem>();
    
    /// <summary>
    /// 检查任务进度，是否已经完成
    /// </summary>
    public void CheckQuestProgress()
    {
        //已完成的任务列表 是一个接口类型
        var finishedRequires = questRequires.Where(q => q.requireAmount <= q.currentAmount);
        isQuestComplete = finishedRequires.Count() == questRequires.Count;
        if (isQuestComplete)
        {
            Debug.Log("任务完成");
        }
    }

    /// <summary>
    /// 获取任务所需物品的名字
    /// </summary>
    /// <returns></returns>
    public List<string> GetQuestRequireNames()
    {
        List<string> questRequireNames = new List<string>();
        foreach (var questRequire in questRequires)
        {
            questRequireNames.Add(questRequire.requireName);
        }
        return questRequireNames;
    }

    /// <summary>
    /// 给奖励
    /// </summary>
    public void GiveRewards()
    {
        foreach (var reward in questRewards)
        {
            //分两种情况
            //奖励是负数的是作为提交到任务的物品
            if (reward.amount<0)
            {
                //需要提交的数量 转成正数
                int requireAmount= Mathf.Abs(reward.amount);
                //先从背包扣，背包不够的部分从actionBar扣
                //背包不为空
                if (InventoryManager.Instance.CheckItemInBag(reward.itemData)!=null)
                {
                    //背包内的物品小于等于所需数量
                    if (InventoryManager.Instance.CheckItemInBag(reward.itemData).amount<=requireAmount)
                    {
                        requireAmount -= InventoryManager.Instance.CheckItemInBag(reward.itemData).amount;
                        InventoryManager.Instance.CheckItemInBag(reward.itemData).amount =0;
                        if (InventoryManager.Instance.CheckItemInActionBar(reward.itemData)!=null)
                        {
                            InventoryManager.Instance.CheckItemInActionBar(reward.itemData).amount -= requireAmount;
                        }
                    }
                    //背包内的物品数量大于所需数量 直接扣除
                    else
                    {
                        InventoryManager.Instance.CheckItemInBag(reward.itemData).amount -= requireAmount;
                    }
                }
                //背包为空 直接从ActionBar扣除 
                else
                {
                    InventoryManager.Instance.CheckItemInActionBar(reward.itemData).amount -= requireAmount;
                }
            
            }
            //奖励是正数的，添加到背包
            else
            {
                InventoryManager.Instance.bagData.AddItem(reward.itemData,reward.amount);
            }
        }
        //刷新页面
        InventoryManager.Instance.bagUI.RefreshUI();
        InventoryManager.Instance.actionBarUI.RefreshUI();
        
    }

}

