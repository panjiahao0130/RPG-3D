using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    private string sceneName;

    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TransitionToMain();
        }
    }

    /// <summary>
    /// 保存角色数据
    /// </summary>
    public void SavePlayerData()
    {
        //保存角色生命值等基础属性
        Save(GameManager.Instance.playerStats.characterData,GameManager.Instance.playerStats.characterData.name);
        //保存角色的基础baseAttack
        Save(GameManager.Instance.playerStats.baseAttackData,GameManager.Instance.playerStats.baseAttackData.name);
        //保存角色的当前的Attack
        Save(GameManager.Instance.playerStats.attackData,GameManager.Instance.playerStats.attackData.name);
    }

    /// <summary>
    /// 加载角色数据
    /// </summary>
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData,GameManager.Instance.playerStats.characterData.name);
        //加载角色的基础baseAttack
        Load(GameManager.Instance.playerStats.baseAttackData,GameManager.Instance.playerStats.baseAttackData.name);
        //加载角色的当前的Attack
        Load(GameManager.Instance.playerStats.attackData,GameManager.Instance.playerStats.attackData.name);
    }
    
    /// <summary>
    /// 保存背包（装备、actionBar）数据
    /// </summary>
    public void SaveInventoryData()
    {
        Save(InventoryManager.Instance.bagData,InventoryManager.Instance.bagData.name);
        Save(InventoryManager.Instance.actionBarData,InventoryManager.Instance.actionBarData.name);
        Save(InventoryManager.Instance.equipmentData,InventoryManager.Instance.equipmentData.name);
    }

    /// <summary>
    /// 加载背包（装备、actionBar）数据
    /// </summary>
    public void LoadInventoryData()
    {
        Load(InventoryManager.Instance.bagData,InventoryManager.Instance.bagData.name);
        Load(InventoryManager.Instance.actionBarData,InventoryManager.Instance.actionBarData.name);
        Load(InventoryManager.Instance.equipmentData,InventoryManager.Instance.equipmentData.name);
    }

    public void SaveQuestData(object data,string key)
    {
        Save(data,key);
    }
    public void LoadQuestData(object data,string key)
    {
        Load(data,key);
    }
    
    private void Save(object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key,jsonData);
        //保存当前场景的名字
        PlayerPrefs.SetString(sceneName,SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    private void Load(object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key),data);
        }
    }
}
