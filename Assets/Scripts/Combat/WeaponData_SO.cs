using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon",menuName = "Weapon/Weapon Data")]
public class WeaponData_SO : ScriptableObject
{
    
    [Tooltip("武器种类：单手剑，双手剑,盾")]
    public WeaponType weaponType;

    [Tooltip("武器是主要武器还是副手武器")]
    public WeaponPrimaryOrSecondary weaponPrimaryOrSecondary;

    [Tooltip("需要在手上生成的武器预制体，用_eq后缀的预制体")]
    public GameObject weaponPrefab;
    
    [Tooltip("每个武器带的动画")]
    public AnimatorOverrideController weaponAnimator;
    [Header("Attack Info")] 
    [Tooltip("普通攻击范围")]
    public float attackRange;
    [Tooltip("技能攻击间隔")]
    public float skillRange;
    [Tooltip("攻击间隔")]
    public float coolDown;
    [Tooltip("最小攻击")]
    public int minDamage;
    [Tooltip("最大攻击")]
    public int maxDamage;
    [Tooltip("暴击伤害 1.1表示有110%的暴击伤害，武器也直接填大于1的数就行，应用到角色时会减1的")]
    public float criticalMultiplier;
    [Tooltip("暴击率")]
    public float criticalChange;   

    [Header("Defend Info")] 
    [Tooltip("增加的防御力")]
    public int defense;
    [Tooltip("提供的额外生命")]
    public int extraLife;
    [Tooltip("每五秒恢复")]
    public int everyFiveSecondsRecovery;
    //todo 写每五秒恢复的逻辑
    
}
