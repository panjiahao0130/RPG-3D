using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [System.Serializable]
    public class QuestTask
    {
        public QuestData_SO questData;
        public bool IsStarted { get => questData.isQuestStarted; set => questData.isQuestStarted = value; }
        public bool IsComplete { get => questData.isQuestComplete; set => questData.isQuestComplete = value; }
        public bool IsFinished { get => questData.isQuestFinished; set => questData.isQuestFinished = value; }
    }

    public List<QuestTask> questTasks = new List<QuestTask>();

    private void Start()
    {
        LoadQuest();
    }

    public void SaveQuest()
    {
        //由于List不是object类型，没法直接通过SaveManager进行保存，通过保存任务数量 再循环遍历每个任务
        //先保存task的数量到playerPrefs
        PlayerPrefs.SetInt("QuestTaskCount",questTasks.Count);
        for (int i = 0; i < questTasks.Count; i++)
        {
            SaveManager.Instance.SaveQuestData(questTasks[i].questData,"Task"+i);
        }
    }

    public void LoadQuest()
    {
        var questData = PlayerPrefs.GetInt("QuestTaskCount");
        for (int i = 0; i < questData; i++)
        {
            var newQuest = ScriptableObject.CreateInstance<QuestData_SO>();
            SaveManager.Instance.LoadQuestData(newQuest,"Task"+i);
            questTasks.Add(new QuestTask{questData = newQuest});
        }
    }

    /// <summary>
    /// 更新任务进度 
    /// </summary>
    /// <param name="requireName">任务所需的东西</param>
    /// <param name="amount">更新的数量</param>
    public void UpdateQuestProgress(string requireName, int amount)
    {
        foreach (var task in questTasks)
        {
            if (task.IsFinished)
            {
                continue;
            }
            //遍历任务列表
            //与所需的东西匹配的任务
            var matchTask = task.questData.questRequires.Find(q => q.requireName == requireName);
            //不为空的matchTask，当前数量增加
            if (matchTask!=null)
            {
                matchTask.currentAmount += amount;
            }
            task.questData.CheckQuestProgress();
        }
    }

    /// <summary>
    /// 判断是否已经有了某个任务
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool HavaQuestTask(QuestData_SO data)
    {
        if (data!=null)
        {
            return questTasks.Any(q => q.questData.questName == data.questName);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 拿到QuestTask
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public QuestTask GetQuestTask(QuestData_SO data)
    {
        return questTasks.Find(q => q.questData.questName == data.questName); 
    }
}
