using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private ItemUI currentItemUI;
    private SlotHolder currentHolder;
    private SlotHolder targetHolder;
    private Transform _transform;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
        _transform = transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //记录原始位置
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        
        _transform.SetParent(InventoryManager.Instance.dragCanvas.transform,true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentItemUI.transform.position = eventData.position;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        //放下物品 交换数据
        //拖拽松开鼠标是否指向某个UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //拖拽松开鼠标在slot范围内
            if (InventoryManager.Instance.CheckInAnyInventoryUI(eventData.position))
            {
                //拖拽松开鼠标所指的区域获取SlotHolder组件
                //GetComponentInParent会从当前物体开始找组件
                targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                //判断目标holder是否为我的原holder
                if (targetHolder!=InventoryManager.Instance.currentDrag.originalHolder)
                    switch (targetHolder.slotType)
                    {
                        case SlotType.Bag:
                            if (currentHolder.slotType == SlotType.MainWeapon || currentHolder.slotType == SlotType.SecondaryWeapon)
                            {
                                SwapItem();
                                var itemData = currentHolder._itemUI.GetItemData();
                                if (currentHolder.slotType == SlotType.MainWeapon)
                                {
                                    if (itemData == null)
                                    {
                                        GameManager.Instance.playerStats.UnEquipMainWeapon();
                                        
                                    }
                                    else
                                    {
                                        GameManager.Instance.playerStats.ChangeMainWeapon(itemData);
                                    }
                                }
                                else if (currentHolder.slotType == SlotType.SecondaryWeapon)
                                {
                                    if (itemData == null)
                                    {
                                        GameManager.Instance.playerStats.UnEquipSecondaryWeapon();
                                    }
                                    else
                                    {
                                        GameManager.Instance.playerStats.ChangeSecondaryWeapon(itemData);
                                    }
                                }
                            }
                            else
                            {
                                SwapItem();
                            }
                            break;
                        case SlotType.MainWeapon:
                            if (currentItemUI.inventoryData.inventoryItems[currentItemUI.Index].itemData.itemType==ItemType.MainWeapon)
                            {
                                SwapItem();
                                GameManager.Instance.playerStats.ChangeMainWeapon(targetHolder._itemUI.GetItemData());
                            }
                            break;
                        case SlotType.SecondaryWeapon:
                            if (currentItemUI.inventoryData.inventoryItems[currentItemUI.Index].itemData.itemType==ItemType.SecondaryWeapon)
                            {
                                if (InventoryManager.Instance.equipmentData.inventoryItems[0].itemData==null)
                                {
                                    break;
                                }
                                //todo 写这
                                if (InventoryManager.Instance.equipmentData.inventoryItems[0].itemData.weaponData.weaponType==WeaponType.TwoHandSword)
                                {
                                    break;
                                }
                                SwapItem();
                                GameManager.Instance.playerStats.ChangeSecondaryWeapon(targetHolder._itemUI.GetItemData());
                            }
                            break;
                        case SlotType.Action:
                            if (currentItemUI.inventoryData.inventoryItems[currentItemUI.Index].itemData.itemType==ItemType.Consumed)
                            {
                                SwapItem();
                            }
                            break;
                    }
                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }
        }
        _transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
        RectTransform t=_transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;

    }

    private void SwapItem()
    {
        var targetItem = targetHolder._itemUI.inventoryData.inventoryItems[targetHolder._itemUI.Index];
        var tempItem = currentHolder._itemUI.inventoryData.inventoryItems[currentHolder._itemUI.Index];
        //判断两个item的类型是否相同
        bool isSameItem = targetItem.itemData == tempItem.itemData;
        //两个item类型相同且可堆叠
        if (isSameItem&&targetItem.itemData.stackable)
        {
            targetItem.amount += targetItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        //其他情况 进行交换位置
        else
        {
            currentHolder._itemUI.inventoryData.inventoryItems[currentHolder._itemUI.Index] = targetItem;
            targetHolder._itemUI.inventoryData.inventoryItems[targetHolder._itemUI.Index] = tempItem;
        }
    }
}
