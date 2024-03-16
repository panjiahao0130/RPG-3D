using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue",menuName = "Dialogue/Dialogue Data")]
public class DialogueData_SO:ScriptableObject
{
    //dialoguePieces显示在编辑器中
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();

    public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();
    
    /// <summary>
    /// 拿当前对话片段中包含的任务
    /// </summary>
    /// <returns></returns>
    public QuestData_SO GetQuest()
    {
        QuestData_SO currentQuest = null;
        foreach (var piece in dialoguePieces)
        {
            if (piece.questData!=null)
            {
                currentQuest = piece.questData;
            }
        }
        return currentQuest;
    }
    
    /// <summary>
    /// 当编辑器发生改变时调用
    /// </summary>
#if UNITY_EDITOR
    private void OnValidate()
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            //字典里没有这个键就添加
            dialogueIndex.TryAdd(piece.pieceID, piece);
            /*if (!dialogueIndex.ContainsKey(piece.ID))
            {
                dialogueIndex.Add(piece.ID,piece);
            }*/
        }
    }
#else
    void Awake()//保证在打包执行的游戏里第一时间获得对话的所有字典匹配 
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            dialogueIndex.TryAdd(piece.ID, piece);
        }
    }
#endif

    
}
