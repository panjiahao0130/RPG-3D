using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    private GameObject player;
    private NavMeshAgent _agent;
    public GameObject playerPrefab;
    //public SceneFader sceneFader;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameManager.Instance.AddObserver(this);
    }

    /// <summary>
    /// 通过传送门传到指定的场景
    /// </summary>
    /// <param name="transitionPoint"></param>
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            //根据不同TransitionType来进行同场景传送或者不同场景传送
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name,transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.toSceneName,transitionPoint.destinationTag));
                break;
        }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        
        if (SceneManager.GetActiveScene().name!=sceneName)
        {
            //todo 可以加一个场景切换的 淡入淡出效果
            //放判断里面是切换场景的时候才保存数据，同场景传送不保存数据
            SaveManager.Instance.SavePlayerData();
            //保存背包数据
            SaveManager.Instance.SaveInventoryData();
            //保存任务数据
            QuestManager.Instance.SaveQuest();
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position,
                GetDestination(destinationTag).transform.rotation);
            //这里不放load是因为PlayerController里的start里有load方法
            
            //这里放save是使得切换场景后能保存当前场景的数据  数据中包括当前场景名称
            SaveManager.Instance.SavePlayerData();
            
            //切换场景后装备武器到手上，切换武器动画
            var equipmentsData = InventoryManager.Instance.equipmentData.inventoryItems;
            foreach (var item in equipmentsData)
            {
                if (item.itemData!=null)
                {
                    GameManager.Instance.playerStats.EquipMainWeapon(item.itemData);

                }
            }
        }
        else
        {
            player = GameManager.Instance.playerStats.gameObject;
            _agent = player.GetComponent<NavMeshAgent>();
            _agent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position,GetDestination(destinationTag).transform.rotation);
            _agent.enabled = true;
            yield return null;
        }
    }

    /// <summary>
    /// 根据场景上的传送门上的destinationTag来获取传送点的位置
    /// </summary>
    /// <param name="destinationTag">返回TransitionDestination组件 即能得到传送点的位置</param>
    /// <returns></returns>
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var transitionDestinations = FindObjectsOfType<TransitionDestination>();
        foreach (var item in transitionDestinations)
        {
            if (item.destinationTag==destinationTag)
            {
                return item;
            }
        }
        return null;
    }

    //切换到第一个游戏场景，是作为newGame进入
    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("Game"));
    }

    //切换到退出游戏或者死亡时的那个场景 作为continue进入
    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }
    
    /// <summary>
    /// mainMenu加载场景 进入enter点 每个场景至少有一个enter点 为了能够继续游戏进入
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator LoadLevel(string sceneName)
    {
        
        if (sceneName!="")
        {
            yield return StartCoroutine(SceneFader.Instance.FadeOut(2.5f));
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position,
                GameManager.Instance.GetEntrance().rotation);
            
            //切换场景后装备武器到手上，切换武器动画
            var equipmentsData = InventoryManager.Instance.equipmentData.inventoryItems;
            foreach (var item in equipmentsData)
            {
                if (item.itemData!=null)
                {
                    GameManager.Instance.playerStats.EquipMainWeapon(item.itemData);

                }
            }
            //切换场景后保存角色数据 数据中包括当前场景名称
            SaveManager.Instance.SavePlayerData();
            SaveManager.Instance.SaveInventoryData();
            QuestManager.Instance.SaveQuest();
            yield return StartCoroutine(SceneFader.Instance.FadeIn(2.5f));
        }
        
    }

    public void TransitionToMain()
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        
        yield return StartCoroutine(SceneFader.Instance.FadeOut(2.5f));
        yield return SceneManager.LoadSceneAsync("Main");
        yield return StartCoroutine(SceneFader.Instance.FadeIn(2.5f));
    }

    public void EndNotify()
    {
        StartCoroutine(LoadMainMenu());
    }
}
