using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum RockStates{HitPlayer,HitEnemy,HitNothing}
public class Rock : MonoBehaviour
{
    [HideInInspector]
    public RockStates _rockStates;
    private Rigidbody rg;
    [Header("Basic Setting")] 
    //施加的力
    public float force;

    public int damage;
    private GameObject target;
    private Vector3 direction;
    public GameObject particlePrefab;
    

    private void Start()
    {
        rg = GetComponent<Rigidbody>();
        rg.velocity=Vector3.one;
        _rockStates = RockStates.HitPlayer;
        FlyToTarget();
    }

    private void FixedUpdate()
    {
        if (rg.velocity.sqrMagnitude < 1f)
        {
            _rockStates = RockStates.HitNothing;
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void FlyToTarget()
    {
        direction = (target.transform.position - transform.position).normalized;
        rg.AddForce(force*direction,ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (_rockStates)
        {
            case RockStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    var targetAgent = other.gameObject.GetComponent<NavMeshAgent>();
                    var targetStats = other.gameObject.GetComponent<CharacterStats>();
                    targetAgent.isStopped = true;
                    targetAgent.velocity = direction * force;
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    targetStats.TakeDamage(targetStats,damage);
                    _rockStates = RockStates.HitNothing;
                }
                break;
            case RockStates.HitEnemy:
                if (other.gameObject.GetComponent<EnemyController>())
                {
                    var targetStats = other.gameObject.GetComponent<CharacterStats>();
                    targetStats.TakeDamage(targetStats,damage);
                    Destroy(gameObject);
                    //生成石头炸开的特效
                    Instantiate(particlePrefab, transform.position, Quaternion.identity);
                }
                break;
            case RockStates.HitNothing:
                break;
        }
    }
}
