using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack",menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
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

    /// <summary>
    /// 应用武器的攻击数据 
    /// </summary>
    /// <param name="weaponData">武器的data_so</param>
    public void ApplyWeaponAttackData(WeaponData_SO weaponData)
    {
        //攻击范围和攻击间隔直接拿武器的范围、攻击间隔作为范围和攻击间隔
        attackRange = weaponData.attackRange;
        skillRange = weaponData.skillRange;
        coolDown = weaponData.coolDown;
        //攻击伤害、暴击率、暴击伤害通过基础伤害加上武器伤害、暴击率、暴击伤害
        minDamage += weaponData.minDamage;
        maxDamage += weaponData.maxDamage;
        criticalMultiplier += (weaponData.criticalMultiplier - 1);
        criticalChange += weaponData.criticalChange;
    }
    
    /// <summary>
    /// 应用不拿武器的攻击数值
    /// </summary>
    /// <param name="baseData"></param>
    public void ApplyBaseAttackData(AttackData_SO baseData)
    {
        //攻击范围和攻击间隔直接拿武器的范围、攻击间隔作为范围和攻击间隔
        attackRange = baseData.attackRange;
        skillRange = baseData.skillRange;
        coolDown = baseData.coolDown;
        //攻击伤害、暴击率、暴击伤害通过基础伤害加上武器伤害、暴击率、暴击伤害
        minDamage = baseData.minDamage;
        maxDamage = baseData.maxDamage;
        criticalMultiplier = baseData.criticalMultiplier ;
        criticalChange = baseData.criticalChange;
    }

    public void ImproveAttackData(float levelMultiplier)
    {
        minDamage = (int)(minDamage * levelMultiplier);
        maxDamage = (int)(maxDamage * levelMultiplier);
    }
}
