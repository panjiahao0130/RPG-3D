using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    /// <summary>
    /// 消耗品
    /// </summary>
    Consumed,
    /// <summary>
    /// 武器
    /// </summary>
    MainWeapon,
    /// <summary>
    /// 副武器
    /// </summary>
    SecondaryWeapon,
}
[CreateAssetMenu(fileName = "New Item",menuName="Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    /// <summary>
    /// 这个数量是该物体的数量，不是在包裹中的数量
    /// </summary>
    public int itemAmount;
    public Sprite itemIcon;
    [TextArea]
    public string description="";
    //是否可堆叠的
    public bool stackable;
    [Header("Usable Item")] 
    public UsableItemData_SO usableItemData;
    [Header("Weapon")] 
    //武器的预制体
    //public GameObject weaponPrefab;

    //武器数据
    public WeaponData_SO weaponData;
    
    //装备武器的动画
    //public AnimatorOverrideController weaponAnimator;

}
