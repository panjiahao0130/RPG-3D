using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
    private bool _isAddToWeaponSlot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isAddToWeaponSlot = InventoryManager.Instance.equipmentData.IsAddToEquipmentSlot(itemData);
            if (_isAddToWeaponSlot)
            {
                if (itemData.itemType==ItemType.MainWeapon)
                {
                    GameManager.Instance.playerStats.EquipMainWeapon(itemData);
                }
                else if (itemData.itemType==ItemType.SecondaryWeapon)
                {
                    GameManager.Instance.playerStats.EquipSecondaryWeapon(itemData);
                }
                InventoryManager.Instance.equipmentUI.RefreshUI();
            }
            else
            {
                InventoryManager.Instance.bagData.AddItem(itemData,itemData.itemAmount);
                InventoryManager.Instance.bagUI.RefreshUI();
            }
            //捡到东西的时候更新任务进度，会根据名字是否匹配来更新
            QuestManager.Instance.UpdateQuestProgress(itemData.itemName,itemData.itemAmount);
            Destroy(gameObject);
        }
    }
}
