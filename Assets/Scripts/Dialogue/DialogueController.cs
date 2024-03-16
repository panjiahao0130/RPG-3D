using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueController : MonoBehaviour
{
    private bool _canTalk = false;
    public DialogueData_SO currentDialogueData;
    private void Update()
    {
        if (_canTalk&&Input.GetKey(KeyCode.E))
        {
            OpenDialogue();
        }
    }

    private void OpenDialogue()
    {
        //传dialogue的数据
        DialogueUIManager.Instance.UpdateDialogueData(currentDialogueData);
        //打开对话UI，更新对话内容
        DialogueUIManager.Instance.UpdateDialogueUI(currentDialogueData.dialoguePieces[0]);
        //todo 加一个屏幕强制刷新
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&currentDialogueData!=null)
        {
            _canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueUIManager.Instance.dialoguePanel.SetActive(false);
            _canTalk = false;
        }
    }
}
