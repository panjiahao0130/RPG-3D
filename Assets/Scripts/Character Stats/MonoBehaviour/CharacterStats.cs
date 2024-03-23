using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// 攻击时更新生命值栏
    /// </summary>
    public event Action<int, int> updateHealthBarOnAttack;
    //角色数据 一直会变
    public CharacterData_SO characterData;
    //角色基础数据，不包括装备效果
    public CharacterData_SO baseCharacterData;
    //模板数据 怪物要设置这个模板数据 不然所有的每个种类怪物共用一个数据 会一刀死
    public CharacterData_SO templateData;
    
    //攻击数据,一直在变 然后保存在json里
    public AttackData_SO attackData; 
    //角色攻击数据，没有武器的状态
    public AttackData_SO baseAttackData;
    //基础攻击数据模板
    public AttackData_SO templateAttackData;
    
    //基础的动画
    private RuntimeAnimatorController baseAnimator;

    private void Awake()
    {
        if (templateData!=null)
        {
            baseCharacterData = Instantiate(templateData);
            characterData = Instantiate(baseCharacterData);
        }

        if (templateAttackData!=null)
        {
            baseAttackData = Instantiate(templateAttackData);
            attackData = Instantiate(baseAttackData);
        }
        //runtimeAnimatorController就是一开始的animator controller里的动画
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }

    [HideInInspector]
    //是否暴击
    public bool isCritical;

    [Header("Weapon")]
    //武器槽位 生成的位置
    [FormerlySerializedAs("weaponSlot")] public Transform mainWeaponSlot;

    [FormerlySerializedAs("shieldSlot")] public Transform secondaryWeaponSlot;
    #region ReadData from data_SO
    public int MaxHealth
    {
        get { if (characterData!=null) return characterData.maxHealth; return 0; }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (characterData!=null) return characterData.currentHealth; return 0; }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { if (characterData!=null) return characterData.baseDefence; return 0; }
        set { characterData.currentDefence = value; }
    }
    public int CurrentDefence
    {
        get { if (characterData!=null) return characterData.currentDefence; return 0; }
        set { characterData.currentDefence = value; }
    } 
    public int EveryFiveSecondsRecovery
    {
        get { if (characterData!=null) return characterData.everyFiveSecondsRecovery; return 0; }
        set { characterData.everyFiveSecondsRecovery = value; }
    }
    public int ExtraLife
    {
        get { if (characterData!=null) return characterData.extraLife; return 0; }
        set { characterData.extraLife = value; }
    }
    #endregion

    #region Character Combat

    /// <summary>
    /// 受到伤害 受到伤害 受到伤害  调用者是受伤的那一方
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    public void TakeDamage(CharacterStats attacker, CharacterStats defender)
    {
        //用Mathf.Max()防止伤害为负数 最低为0
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.CurrentDefence,0);
        //当前血量 和伤害同理
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        if (attacker.isCritical)
        {
            defender.GetComponent<Animator>().SetTrigger("Hit");
        }
        // 经验update
        updateHealthBarOnAttack?.Invoke(CurrentHealth,MaxHealth);
        //update UI
        if (CurrentHealth<=0)
        {
             attacker.characterData.UpdateExp(characterData.killPoint);
             attacker.baseCharacterData.UpdateExp(characterData.killPoint);
        }
        
    }
    /// <summary>
    /// 用石头攻击时执行的
    /// </summary>
    /// <param name="defender">被攻击者</param>
    /// <param name="damage">伤害值</param>
    public void TakeDamage(CharacterStats defender, int damage)
    {
        int currentDamage = Mathf.Max(damage - defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        updateHealthBarOnAttack?.Invoke(CurrentHealth,MaxHealth);
        if (CurrentHealth<=0)
        {
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killPoint);
            GameManager.Instance.playerStats.baseCharacterData.UpdateExp(characterData.killPoint);
        }
    }

    private int CurrentDamage()
    {
        float coreDamage = Random.Range(attackData.minDamage,attackData.maxDamage);//maxDamage要+1 因为不包含最大值
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            //Debug.Log("暴击"+coreDamage);
        }

        return (int)coreDamage;
    }

    #endregion

    #region 穿戴装备（主武器 副武器）

    /// <summary>
    /// 装备主武器 实例化武器到手上，更新武器数据，切换动画
    /// </summary>
    /// <param name="weaponData"></param>
    public void EquipMainWeapon(WeaponData_SO weaponData)
    {
        if (weaponData!=null)
        {
            //实例化主武器武器
            Instantiate(weaponData.weaponPrefab, mainWeaponSlot);
            // 先调基础的数据
            attackData.ApplyBaseAttackData(baseAttackData);
            //再调装备武器的数据 
            attackData.ApplyWeaponAttackData(weaponData);
            //切换成装备武器的动画
            GetComponent<Animator>().runtimeAnimatorController = weaponData.weaponAnimator;
            //根据武器类型再执行一定逻辑，如装备双手武器的同时会卸载副手武器
            switch (weaponData.weaponType)
            {
                case WeaponType.OneHandedSword:
                    break;
                case WeaponType.TwoHandSword:
                    UnEquipSecondaryWeaponPassively();
                    break;
                case WeaponType.Shield:
                    break;
            }
            
        }
    }

    /// <summary>
    /// 卸载主武器
    /// </summary>
    public void UnEquipMainWeapon()
    {
        //如果武器位置有武器，先将武器销毁
        if (mainWeaponSlot.transform.childCount!=0)
        {
            for (int i = 0; i < mainWeaponSlot.transform.childCount; i++)
            {
                Destroy(mainWeaponSlot.transform.GetChild(i).gameObject);
            }
        }
        attackData.ApplyBaseAttackData(baseAttackData);
        //切换成空手的动画
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
        //被动卸载掉副手武器
        UnEquipSecondaryWeaponPassively();
    }

    

    /// <summary>
    /// 装备副手武器 实例化武器到手上，更新武器数据
    /// </summary>
    /// <param name="secondaryWeapon"></param>
    public void EquipSecondaryWeapon(WeaponData_SO secondaryWeapon)
    {
        if (secondaryWeapon!=null)
        {
            Instantiate(secondaryWeapon.weaponPrefab, secondaryWeaponSlot);
            //先调用基础数据
            characterData.ApplyBaseDefenseData(baseCharacterData);
            //再调用武器数据
            characterData.ApplyWeaponDefenseData(secondaryWeapon);
        }
    }
    
    /// <summary>
    /// 卸载副武器
    /// </summary>
    public void UnEquipSecondaryWeapon()
    {
        //如果盾牌位置有盾牌，先将盾牌摧毁
        if (secondaryWeaponSlot.transform.childCount!=0)
        {
            for (int i = 0; i < secondaryWeaponSlot.transform.childCount; i++)
            {
                Destroy(secondaryWeaponSlot.transform.GetChild(i).gameObject);
            }
        }
        //应用角色基础数据
        characterData.ApplyBaseDefenseData(baseCharacterData);
    }

    public void ChangeMainWeapon(ItemData_SO mainWeapon)
    {
        UnEquipMainWeapon();
        EquipMainWeapon(mainWeapon.weaponData);
    }
    
    public void ChangeSecondaryWeapon(ItemData_SO secondaryWeapon)
    {
        UnEquipSecondaryWeapon();
        EquipSecondaryWeapon(secondaryWeapon.weaponData);
    }
    
    /// <summary>
    /// 被动卸载副手武器，先判断副手武器数据库有没有数据，没有则无效果
    /// </summary>
    private void UnEquipSecondaryWeaponPassively()
    {
        var secondaryEquipment = InventoryManager.Instance.equipmentData.inventoryItems[1].itemData;
        if (secondaryEquipment != null)
        {
            //卸载主武器的时候同时卸载副武器
            //将副武器数据添加到背包处
            InventoryManager.Instance.bagData.AddItem(secondaryEquipment, secondaryEquipment.itemAmount);
            InventoryManager.Instance.equipmentData.inventoryItems[1].itemData = null;
            //刷新装备栏UI和背包UI
            InventoryManager.Instance.equipmentUI.RefreshUI();
            InventoryManager.Instance.bagUI.RefreshUI();
            //卸载副武器
            GameManager.Instance.playerStats.UnEquipSecondaryWeapon();
        }
    }
    
    #endregion

    #region Apply Data Change

    /// <summary>
    /// 应用血量
    /// </summary>
    /// <param name="amount"></param>
    public void ApplyHealth(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }

    /// <summary>
    /// 同步装备数据，包括生命值等基础数据和攻击数据等
    /// </summary>
    public void ApplyEquipmentData()
    {
        attackData.ApplyBaseAttackData(baseAttackData);
        characterData.ApplyBaseDefenseData(baseCharacterData);
        var mainWeaponData = InventoryManager.Instance.equipmentData.inventoryItems[0].itemData;
        if (mainWeaponData!=null)
        {
            attackData.ApplyWeaponAttackData(mainWeaponData.weaponData);
        }
        var secondaryWeaponData = InventoryManager.Instance.equipmentData.inventoryItems[1].itemData;
        if (secondaryWeaponData!=null)
        {
            characterData.ApplyWeaponDefenseData(secondaryWeaponData.weaponData);
        }
    }

    #endregion
    
}
