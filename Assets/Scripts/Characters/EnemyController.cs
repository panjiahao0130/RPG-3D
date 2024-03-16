using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyStates
{
    [Description("警卫 站桩类型")]
    Guard,
    [Description("巡逻类型")]
    Patrol,
    [Description("追赶")]
    Chase,
    [Description("死亡")]
    Dead,
}
[RequireComponent(typeof(NavMeshAgent),typeof(CharacterStats),typeof(Collider))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    private NavMeshAgent agent;
    private Animator anim;
    private Collider coll;
    private EnemyStates enemyStates;
    protected CharacterStats characterStats;
    [Header("Basic Setting")]
    //怪物的名字 为了方便在任务需求上显示中文名
    public string enemyName;
    public float sightRadius;
    public bool isGuard;
    private float _speed;
    public float lookAtTime;
    private float remainLookAtTime;
    protected GameObject attackTarget;
    private float lastAttackTime;

    [Header("Patrol State")] 
    public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 origPosition;
    private Quaternion origRotation;
    
    //bool值配合动画
    private bool isWalk;
    private bool isChase;
    private bool isFollow;
    private bool isDead;
    private bool PlayerDead;
    
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        characterStats = GetComponent<CharacterStats>();
        _speed = agent.speed;
        origPosition = transform.position;
        origRotation = transform.rotation;
        remainLookAtTime = lookAtTime;
    }

    private void Start()
    {
        
        if (isGuard)
        {
            enemyStates = EnemyStates.Guard;
        }
        else
        {
            enemyStates = EnemyStates.Patrol;
            GetNewWayPoint();
        }
        GameManager.Instance.AddObserver(this);
    }

    /*//加载场景的时候注册观察者
    private void OnEnable()
    {
        GameManager.Instance.AddObserver(this);
    }*/

    private void OnDisable()
    {
        if (!GameManager.IsInitialized) return;
        GameManager.Instance.RemoveObserver(this);
        //掉落物品
        if (GetComponent<LootItemSpawner>()&&isDead)
        {
            GetComponent<LootItemSpawner>().SpawnLootItem();
        }

        if (QuestManager.IsInitialized&&isDead)
        {
            QuestManager.Instance.UpdateQuestProgress(enemyName,1);
        }
    }

    private void Update()
    {
        if (characterStats.CurrentHealth==0)
        {
            isDead = true;
        }
        if (!PlayerDead)
        {
            SwitchEnemyStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }
    }

    void SwitchAnimation()
    {
        anim.SetBool("Walk",isWalk);
        anim.SetBool("Chase",isChase);
        anim.SetBool("Follow",isFollow);
        anim.SetBool("Critical",characterStats.isCritical);
        anim.SetBool("Death",isDead);
        
    }

    private void SwitchEnemyStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.Dead;
        }
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.Chase;
            //todo 可以加一个警觉时间
            remainLookAtTime = lookAtTime;
        }
        switch (enemyStates)
        {
            case EnemyStates.Guard:
                agent.speed = _speed * 0.5f;
                isChase = false;
                if (transform.position!=origPosition)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = origPosition;
                    if (Vector3.SqrMagnitude(origPosition-transform.position)<=agent.stoppingDistance)
                    {
                        isWalk = false;
                        transform.rotation= Quaternion.Lerp(transform.rotation, origRotation, 0.02f);
                    }
                }
                break;
            case EnemyStates.Patrol:
                agent.speed = _speed * 0.5f;
                isChase = false;
                //判断是否到了目标点
                if (Vector3.Distance(wayPoint,transform.position)<=agent.stoppingDistance)
                {
                    isWalk = false;
                    remainLookAtTime -= Time.deltaTime;
                    if (remainLookAtTime<0)
                    {
                        GetNewWayPoint();
                    }
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                
                break;
            case EnemyStates.Chase:
                agent.speed = _speed;
                isWalk = false;
                isChase = true;
                if (!FoundPlayer())
                {
                    // 超出范围拉回
                    isFollow = false;
                    agent.destination = transform.position;
                    remainLookAtTime -= Time.deltaTime;
                    if (remainLookAtTime<0)
                    {
                        if (isGuard)
                        {
                            enemyStates = EnemyStates.Guard;
                        }
                        else
                        {
                            enemyStates = EnemyStates.Patrol;
                        }
                    }
                }
                else
                {
                    //在追击范围不停追击
                    agent.isStopped = false;
                    isFollow = true;
                    
                    agent.destination = attackTarget.transform.position;
                }
                if (TargetInAttackRange()||TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime<0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;
                        //暴击判断
                        characterStats.isCritical = Random.value <= characterStats.attackData.criticalChange; 
                        Attack();
                    }
                    
                }
                break;
            case EnemyStates.Dead:
                coll.enabled = false;
                agent.radius = 0;
                Destroy(gameObject,2f);
                break;
        }
    }

    private void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if (TargetInAttackRange())
        {
            //todo 有个bug，玩家接近进入攻击范围后，立刻离开范围 会再攻击一次 把怪砍死了还会对角色造成一次伤害  近战和远程攻击执行顺序
            anim.SetTrigger("Attack");
        }
        if (TargetInSkillRange())
        {
            anim.SetTrigger("Skill");
        }
    }

    bool TargetInAttackRange()
    {
        if (attackTarget!=null)
        {
            return Vector3.Distance(transform.position, attackTarget.transform.position) <=
                   characterStats.attackData.attackRange+agent.radius;
        }
        else
        {
            return false;
        }
    }

    bool TargetInSkillRange()
    {
        if (attackTarget!=null)
        {
            return Vector3.Distance(transform.position, attackTarget.transform.position) <=
                   characterStats.attackData.skillRange+agent.radius;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 在半径范围内寻找角色
    /// </summary>
    /// <returns></returns>
    bool FoundPlayer()
    {
        var colliders= Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var item in colliders)
        {
            if (item.CompareTag("Player"))
            {
                attackTarget = item.gameObject;
                return true;
            }
        }

        attackTarget = null;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.blue;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
    }

    private void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);
        Vector3 randomPoint = new Vector3(origPosition.x + randomX, transform.position.y, origPosition.z + randomZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }

    //---------Animation Event---------
    void Hit()
    {
        if (attackTarget!=null&&transform.IsFaceTarget(attackTarget.transform))
        {
            //获取目标的CharacterStats组件
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats,targetStats);
        }
        
    }

    public void EndNotify()
    {
        //播放获胜动画
        anim.SetBool("Win",true);
        PlayerDead = true;
        //停止其他动画
        isChase = false;
        isWalk = false;
        attackTarget = null;
        
    }
}
