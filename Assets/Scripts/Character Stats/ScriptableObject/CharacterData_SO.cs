using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Data",menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
     [Header("Stats Info")] 
     public int maxHealth;
     public int currentHealth;
     public int baseDefence;
     public int currentDefence;

     [Header("Kill")] 
     public int killPoint;

     [Header("Level")] 
     public int currentLevel;
     public int maxLevel;
     public int baseExp;
     public int currentExp;
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
          UpdateCharacterData();
          //升级攻击数据,变的是base的数据
          GameManager.Instance.playerStats.baseAttackData.ImproveAttackData(LevelMultiplier);
          //更新attack,从base和武器的加起来
          GameManager.Instance.playerStats.UpdateAttackData();
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
}
