using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene,DifferentScene
    }
    [Header("Transition Info")]
    //要传送去的场景名
    public string toSceneName;

    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;
    private bool canTrans;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&canTrans)
        {
            
            SceneController.Instance.TransitionToDestination(this);
        }
    }


    //当角色站在门前面canTrans设为true 才能传送
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrans = true;
        }
    }
    //当角色站在门前面canTrans设为false 不能传送
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrans = false;
        }
    }
}
