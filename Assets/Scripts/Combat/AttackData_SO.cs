using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack",menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Header("Attack Info")]
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public int minDamage;
    public int maxDamage;

    /// <summary>
    /// 暴击伤害
    /// </summary>
    public float criticalMultiplier;

    /// <summary>
    /// 暴击率
    /// </summary>
    public float criticalChange;

    /// <summary>
    /// 应用武器数据
    /// </summary>
    /// <param name="weaponData">武器的data_so</param>
    public void ApplyWeaponData(AttackData_SO weaponData)
    {
        attackRange = weaponData.attackRange;
        skillRange = weaponData.skillRange;
        coolDown = weaponData.coolDown;
        minDamage = weaponData.minDamage;
        maxDamage = weaponData.maxDamage;
        criticalMultiplier = weaponData.criticalMultiplier;
        criticalChange = weaponData.criticalChange;
    }
}
