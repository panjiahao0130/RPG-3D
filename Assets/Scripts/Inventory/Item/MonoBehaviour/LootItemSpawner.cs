using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemSpawner : MonoBehaviour
{
    //可序列化的
    [System.Serializable]
    public class StyledLootItem
    {
        public GameObject item;
        [Range(0,1)]
        public float weight;
    }

    public List<StyledLootItem> styledLootItems;

    public void SpawnLootItem()
    {
        float currentValue = Random.value;
        for (int i = 0; i < styledLootItems.Count; i++)
        {
            if (currentValue<=styledLootItems[i].weight)
            {
                GameObject obj = Instantiate(styledLootItems[i].item);
                obj.transform.position = transform.position + Vector3.up * 3;
            }
            //break决定能不能多掉落
            //break 
        }
        
    }
}
