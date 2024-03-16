using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Golem : EnemyController
{
    [Header("Skill")] 
    public float kickForce = 25f;

    public Transform  handPos;
    public GameObject rockPrefab;
    
    //Animation Event
    void Kickoff()
    {
        if (attackTarget!=null&&transform.IsFaceTarget(attackTarget.transform))
        {
            //获取目标的CharacterStats组件
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = kickForce * direction;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
            
            targetStats.TakeDamage(characterStats,targetStats);
        }
    }

    void ThowRock()
    {
        var rock = Instantiate(rockPrefab, handPos.position+Vector3.up, Quaternion.identity);
        if (attackTarget==null)
        {
            attackTarget = FindObjectOfType<PlayerController>().gameObject;
        }
        rock.GetComponent<Rock>().SetTarget(attackTarget);
        
        
    }
}
