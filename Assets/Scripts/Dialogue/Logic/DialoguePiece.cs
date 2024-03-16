using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class DialoguePiece
{
    public string pieceID;
    public Sprite image;
    [TextArea]
    public string text;
    /// <summary>
    /// 每个有任务的对话片段的任务数据
    /// </summary>
    public QuestData_SO questData;
    public List<DialogueOption> dialogueOptions = new List<DialogueOption>();
}
