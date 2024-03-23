using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 武器种类分类
/// </summary>
public enum WeaponType
{
    /// <summary>
    /// 单手剑
    /// </summary>
    OneHandedSword,
    /// <summary>
    /// 双手剑
    /// </summary>
    TwoHandSword,
    /// <summary>
    /// 盾牌类型
    /// </summary>
    Shield,
    
}

/// <summary>
/// 武器的主次分类
/// </summary>
public enum WeaponPrimaryOrSecondary
{
    /// <summary>
    /// 主要武器
    /// </summary>
    Primary,
    /// <summary>
    /// 副手武器
    /// </summary>
    Secondary,
    //todo 直接在item判断主副武器的逻辑改为在武器数据这根据主副武器来执行相应逻辑
}


