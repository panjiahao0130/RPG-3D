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
          currentLevel = Mathf.Clamp(currentLevel + 1,0,maxLevel);
          //每次升级的变化是级数乘以倍率
          baseExp=(int)(baseExp * LevelMultiplier);
          currentExp = 0;
          maxHealth = (int)(maxHealth * LevelMultiplier);
          currentHealth = maxHealth;
          //todo 升级变攻击力 记得改template
          
          Debug.Log("升级了，当前等级是"+currentLevel);
     }
}
