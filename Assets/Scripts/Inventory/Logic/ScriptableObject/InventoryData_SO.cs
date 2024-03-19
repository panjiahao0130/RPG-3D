using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory",menuName="Inventory/Inventory Data")]
[Description("仓库数据管理")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();

    /// <summary>
    /// 添加物品到仓库 包括背包和Action Bar
    /// </summary>
    /// <param name="newItemData">新的物品信息</param>
    /// <param name="amount">数量</param>
    public void AddItem(ItemData_SO newItemData, int amount)
    {
        //先判断物品是否可堆叠 并进行堆叠操作
        if (newItemData.stackable)
        {
            foreach (var item in inventoryItems)
            {
                if (item.itemData==newItemData)
                {
                    //当前物品的数量与新物品的数量相加
                    item.amount += newItemData.itemAmount;
                    return;
                }
            }
        }
        //说明原先仓库里没有
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].itemData==null)
            {
                inventoryItems[i].itemData = newItemData;
                inventoryItems[i].amount = amount;
                return;
            }
        }
    }

    /// <summary>
    /// 添加装备数据到装备仓库，并返回是否添加成功
    /// </summary>
    /// <param name="newItemData"></param>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool IsAddToEquipmentSlot(ItemData_SO newItemData,int amount=1)//武器和盾牌的数量默认都为1
    {
        if (newItemData.itemType==ItemType.MainWeapon)
        {
            if (inventoryItems[0].itemData==null)
            {
                inventoryItems[0].itemData = newItemData;
                inventoryItems[0].amount = amount;
                return true;
            }
        }
        else if (newItemData.itemType==ItemType.SecondaryWeapon)
        {
            if (inventoryItems[0].itemData==null)
            {
                Debug.Log("该武器是副武器，没有主武器无法装备");
                return false;
            }
            if (inventoryItems[1].itemData==null)
            {
                inventoryItems[1].itemData = newItemData;
                inventoryItems[1].amount = amount;
                return true;
            }
        }
        return false;
    }
}
[Serializable]
public class InventoryItem
{
    //包裹中的某个格子的item数据
    public ItemData_SO itemData;
    //该item的数量
    public int amount;
}
