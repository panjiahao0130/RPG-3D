using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 槽位类型
/// </summary>
public enum SlotType
{
    /// <summary>
    /// 背包格子
    /// </summary>
    Bag,
    /// <summary>
    /// 主武器格子
    /// </summary>
    MainWeapon,
    /// <summary>
    /// 副武器格子
    /// </summary>
    SecondaryWeapon,
    /// <summary>
    /// 下方使用栏格子
    /// </summary>
    Action
}
public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI _itemUI;

    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.Bag:
                _itemUI.inventoryData = InventoryManager.Instance.bagData; 
                break;
            case SlotType.MainWeapon:
                _itemUI.inventoryData = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.SecondaryWeapon:
                _itemUI.inventoryData = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.Action:
                _itemUI.inventoryData = InventoryManager.Instance.actionBarData;
                break;
        }

        var item = _itemUI.GetInventoryItem();
        _itemUI.SetUpItemUI(item.itemData,item.amount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount%2==0)
        {
            if (_itemUI.GetItemData()!=null)
            {
                if (_itemUI.GetItemData().itemType==ItemType.Consumed)
                {
                    UseItem();
                }
                /*else
                {
                    if (_itemUI.GetItemData().itemType==ItemType.MainWeapon)
                    {
                        GameManager.Instance.playerStats.ChangeMainWeapon(_itemUI.GetItemData());
                    }
                    else
                    {
                        GameManager.Instance.playerStats.ChangeSecondaryWeapon(_itemUI.GetItemData());
                    }
                    var item = InventoryManager.Instance.equipmentData.SwapItem(_itemUI.GetItemData());
                    if (item!=null)
                    {
                        InventoryManager.Instance.bagData.AddItem(item,item.itemAmount);
                    }
                    UpdateItem();
                    InventoryManager.Instance.equipmentUI.RefreshUI();
                }*/
            }
            
        }
    }

    /// <summary>
    /// 使用道具
    /// </summary>
    public void UseItem()
    {
        if (_itemUI.GetItemData().itemType==ItemType.Consumed&&_itemUI.GetInventoryItem().amount>0)
        {
            GameManager.Instance.playerStats.ApplyHealth(_itemUI.GetItemData().usableItemData.healthPoint);
            _itemUI.GetInventoryItem().amount -= 1;
            QuestManager.Instance.UpdateQuestProgress(_itemUI.GetItemData().itemName,-1);
            //如果数量为0 数据直接删了
            /*if (_itemUI.GetInventoryItem().amount == 0)
            {
                _itemUI.GetInventoryItem().itemData = null;
            }*/
            UpdateItem();
        }
        
    }
    private bool isMouseOver = false;
    private IEnumerator DelayShow()
    {
        yield return new WaitForSeconds(0.5f);

        if (isMouseOver)
        {
            if (_itemUI.GetItemData())
            {
                InventoryManager.Instance.tooltip.SetItemTooltips(_itemUI.GetItemData());
                InventoryManager.Instance.tooltip.gameObject.SetActive(true); 
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        StartCoroutine(DelayShow());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        if (InventoryManager.Instance.tooltip.gameObject.activeInHierarchy )
        {
            InventoryManager.Instance.tooltip.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }
}
