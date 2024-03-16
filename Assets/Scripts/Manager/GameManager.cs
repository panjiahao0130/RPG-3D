using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public CharacterStats playerStats;

    private CinemachineFreeLook fowllowCamera;

    //观察者列表
    private List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    private bool isGameOver = false;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 注册角色数据
    /// </summary>
    /// <param name="player">player的数据</param>
    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;
        fowllowCamera = FindObjectOfType<CinemachineFreeLook>();
        if (fowllowCamera!=null)
        {
            fowllowCamera.Follow = playerStats.transform.GetChild(2);
            fowllowCamera.LookAt = playerStats.transform.GetChild(2); 
        }
    }

    /// <summary>
    /// 添加观察者
    /// </summary>
    /// <param name="observer"></param>
    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    /// <summary>
    /// 移除观察者
    /// </summary>
    /// <param name="observer"></param>
    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    /// <summary>
    /// 通知所有观察者
    /// </summary>
    public void NotifyObservers()
    {
        isGameOver = true;
        if (isGameOver)
        {
            isGameOver = false;
            foreach (var observer in endGameObservers)
            {
                observer.EndNotify();
            }
        }
    }

    /// <summary>
    /// 找场景中DestinationTag为Enter的传送门位置
    /// </summary>
    /// <returns>返回Enter点的Transform</returns>
    public Transform GetEntrance()
    {
        
        foreach (var destination in FindObjectsOfType<TransitionDestination>())
        {
            if (destination.destinationTag==TransitionDestination.DestinationTag.Enter)
            {
                return destination.transform;
            }
        }
        return null;
    }
}
