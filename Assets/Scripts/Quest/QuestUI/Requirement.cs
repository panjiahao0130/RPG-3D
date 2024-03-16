using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Requirement : MonoBehaviour
{
    private TextMeshProUGUI _requirementNameTxt;

    private TextMeshProUGUI _progress;

    private void Awake()
    {
        _requirementNameTxt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _progress = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 设置任务需求的UI
    /// </summary>
    /// <param name="requireName">需求的名称</param>
    /// <param name="requireAmount">需要的数量</param>
    /// <param name="currentAmount">当前已经有的数量</param>
    public void SetupRequirement(string requireName,int requireAmount,int currentAmount)
    {
        _requirementNameTxt.text = requireName;
        _progress.text = currentAmount + "/" + requireAmount;
    }
    /// <summary>
    /// 设置已完成的任务的UI
    /// </summary>
    /// <param name="quest"></param>
    public void SetupRequirement(QuestData_SO quest)
    {
        _requirementNameTxt.text = quest.questName;
        _progress.text = "(已完成)";
        _progress.color = new Color(0, 1, 6 / 255f);
        _requirementNameTxt.color = new Color(0, 1, 6 / 255f);
    }
}
