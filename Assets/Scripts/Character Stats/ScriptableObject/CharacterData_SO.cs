using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Data",menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
     [Header("Stats Info")] 
     [Tooltip("最大血量")]
     public int maxHealth;
     [Tooltip("当前血量")]
     public int currentHealth;
     [Tooltip("基础防御力")]
     public int baseDefence;
     [Tooltip("当前防御力")]
     public int currentDefence;
     [Tooltip("额外生命值")]
     public int extraLife;
     [Tooltip("每五秒回血")]
     public int everyFiveSecondsRecovery;

     [Header("Kill")] 
     public int killPoint;

     [Header("Level")] 
     [Tooltip("当前等级")]
     public int currentLevel;
     [Tooltip("最大等级，设定是10级")]
     public int maxLevel;
     [Tooltip("当前等级升级需要多少经验值")]
     public int baseExp;
     [Tooltip("当前经验值")]
     public int currentExp;
     [Tooltip("升级后提供的提升加成的比例")]
     public float levelBuff;

     private float LevelMultiplier
     {
          get
          {
               return 1 + (currentLevel - 1) * levelBuff;
          }
     }

     /// <summary>
     /// 更新经验值
     /// </summary>
     /// <param name="point">每个怪物上的经验值</param>
     public void UpdateExp(int point)
     {
          currentExp += point;
          if (currentExp>=baseExp)
          {
               LevelUp();
          }
     }

     private void LevelUp()
     {
          GameManager.Instance.playerStats.baseCharacterData.UpdateCharacterData();
          GameManager.Instance.playerStats.characterData.UpdateCharacterData();
          //升级攻击数据,变的是base的数据
          GameManager.Instance.playerStats.baseAttackData.ImproveAttackData(LevelMultiplier);
          //更新装备数据
          GameManager.Instance.playerStats.ApplyEquipmentData();
          Debug.Log("升级了，当前等级是"+currentLevel);
     }

     /// <summary>
     /// 升级变化更新玩家基础数据
     /// </summary>
     private void UpdateCharacterData()
     {
          currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
          //每次升级的变化是级数乘以倍率
          baseExp = (int)(baseExp * LevelMultiplier);
          currentExp = 0;
          maxHealth = (int)(maxHealth * LevelMultiplier);
          currentHealth = maxHealth;
          baseDefence = (int)(baseDefence * LevelMultiplier);
          currentDefence = baseDefence;
     }
     /// <summary>
     /// 应用武器的防御数据 
     /// </summary>
     /// <param name="weaponData">武器的data_so</param>
     public void ApplyWeaponDefenseData(WeaponData_SO weaponData)
     {
          Debug.Log("调用了装备副武器");
          currentDefence += weaponData.defense;
          extraLife = weaponData.extraLife;
          maxHealth += extraLife;
          currentHealth += extraLife;
          everyFiveSecondsRecovery += weaponData.everyFiveSecondsRecovery;
     }
     /// <summary>
     /// 取消应用武器的防御数据
     /// </summary>
     /// <param name="weaponData">武器的data_so</param>
     public void ApplyBaseDefenseData(CharacterData_SO characterBaseData)
     {
          currentDefence = characterBaseData.baseDefence;
          maxHealth -=extraLife;
          currentHealth -= extraLife;
          extraLife = characterBaseData.extraLife;
          everyFiveSecondsRecovery = characterBaseData.everyFiveSecondsRecovery;
     }
}
