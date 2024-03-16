using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amount;
    [HideInInspector]
    public ItemData_SO currentData;
    
    /// <summary>
    /// 每个itemUI上的对应的包裹数据
    /// </summary>
    public InventoryData_SO inventoryData { get; set; }
    public int Index { get; set; } = -1;
    

    /// <summary>
    /// 设置图片UI和数量
    /// </summary>
    /// <param name="item"></param>
    /// <param name="itemAmount"></param>
    public void SetUpItemUI(ItemData_SO item, int itemAmount)
    {
        //当数量为0 将背包的数据置空
        if (itemAmount==0)
        {
            inventoryData.inventoryItems[Index].itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }

        //数量小于0 将item置空 主要是改任务面板那个奖励的显示 小于0的数是用来提交给任务的
        if (itemAmount<0)
        {
            item = null;
        }
        if (item!=null)
        {
                currentData = item;
                icon.sprite = item.itemIcon;
                amount.text = itemAmount.ToString();
                icon.gameObject.SetActive(true);
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
        
    }

    /// <summary>
    /// 获取item的ItemData_SO数据
    /// </summary>
    /// <returns></returns>
    public ItemData_SO GetItemData()
    {
        return inventoryData.inventoryItems[Index].itemData;
    }
    
    /// <summary>
    /// 获取InventoryItem
    /// </summary>
    /// <returns></returns>
    public InventoryItem GetInventoryItem()
    {
        return inventoryData.inventoryItems[Index];
    }
}
