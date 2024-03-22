using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包系统管理器
/// </summary>
public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }
    
    [Header("Inventory Data")]//仓库数据
    [HideInInspector]
    public InventoryData_SO bagData;
    public InventoryData_SO bagTemplate;
    [HideInInspector]
    public InventoryData_SO actionBarData;
    public InventoryData_SO actionBarTemplate;
    [HideInInspector]
    public InventoryData_SO equipmentData;
    public InventoryData_SO equipmentTemplate;

    [Header("Containers")] //容器UI
    public ContainerUI bagUI;
    public ContainerUI actionBarUI;
    public ContainerUI equipmentUI;

    [Header("DragCanvas")] 
    public Canvas dragCanvas;

    [Header("UI Panel")] 
    public GameObject bagPanel;
    public GameObject statsPanel;

    [Header("Stats Text")] 
    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI attackTxt;
    public TextMeshProUGUI defenseTxt;
    
    [Header("ItemToolTip")] 
    public ItemTooltip tooltip;

    //拖动时候的那个item
    public DragData currentDrag;

    protected override void Awake()
    {
        base.Awake();
        if (bagTemplate!=null) bagData = Instantiate(bagTemplate);
        if (actionBarTemplate!=null) actionBarData = Instantiate(actionBarTemplate);
        if (equipmentTemplate!=null) equipmentData = Instantiate(equipmentTemplate);
    }

    private void Start()
    {
        //每个场景加载的时候读取数据
        SaveManager.Instance.LoadInventoryData();
        bagUI.RefreshUI();
        actionBarUI.RefreshUI();
        equipmentUI.RefreshUI();
    }

    private bool isOpen = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bagPanel.SetActive(isOpen);
            statsPanel.SetActive(isOpen);
            isOpen = !isOpen;
        }

        if (GameManager.Instance.playerStats!=null)
        {
            UpdateStatsText(GameManager.Instance.playerStats.CurrentHealth,GameManager.Instance.playerStats.attackData.minDamage,GameManager.Instance.playerStats.attackData.maxDamage,GameManager.Instance.playerStats.CurrentDefence);
        }
    }

    private void UpdateStatsText(int health,int min,int max,int defense)
    {
        healthTxt.text = health.ToString();
        attackTxt.text = min + " - " + max;
        defenseTxt.text = defense.ToString();
    }

    #region 检查拖拽物品是否在每一个 Slot范围内
    /// <summary>
    /// 检查鼠标是否在某一种类的容器槽位UI之中
    /// </summary>
    /// <param name="position">鼠标所在位置</param>
    /// <param name="slotHolders">传入不同slotHolders</param>
    /// <returns></returns>
    private bool CheckInSomeInventoryUI(Vector3 position, SlotHolder[] slotHolders)
    {
        foreach (var slotHolder in slotHolders)
        {
            RectTransform t = (RectTransform)slotHolder.transform;
            //此 RectTransform 是否包含从给定摄像机观察到的屏幕点？
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 指针是否在任一的槽位区域内
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckInAnyInventoryUI(Vector3 position)
    {
        return CheckInSomeInventoryUI(position, bagUI.slotHolders) || 
               CheckInSomeInventoryUI(position, actionBarUI.slotHolders) || 
               CheckInSomeInventoryUI(position, equipmentUI.slotHolders);
    }
    #endregion

    #region 检测包裹中是否有任务物品

    /// <summary>
    /// 检测背包中是否已经存在任务所需的物品并更新进度
    /// </summary>
    /// <param name="questItemName">任务所需物品的名字</param>
    public void CheckQuestRequireItemInInventory(string questItemName)
    {
        //检测背包
        foreach (var item in bagData.inventoryItems)
        {
            if (item.itemData!=null)
            {
                if (item.itemData.itemName==questItemName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName,item.amount);
                }
            }
        }
        //检测Actionbar
        foreach (var item in actionBarData.inventoryItems)
        {
            if (item.itemData!=null)
            {
                if (item.itemData.itemName==questItemName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName,item.amount);
                }
            }
            
        }
    }
    #endregion

    /// <summary>
    /// 检测背包中是否有指定物品 返回指定物品
    /// </summary>
    /// <param name="questItem">负数的那个questReward </param>
    /// <returns></returns>
    public InventoryItem CheckItemInBag(ItemData_SO questItem)
    {
        return bagData.inventoryItems.Find(q => q.itemData == questItem);
    }
    
    /// <summary>
    /// 检测ActionBar中是否有指定物品 返回指定物品
    /// </summary>
    /// <param name="questItem">负数的那个questReward </param>
    /// <returns></returns>
    public InventoryItem CheckItemInActionBar(ItemData_SO questItem)
    {
        return actionBarData.inventoryItems.Find(q => q.itemData == questItem);
    }
    
}
