using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    private GameObject attackTarget;

    private float lastAttackTime;

    private CharacterStats characterStats;
    private bool isDead;

    private float stopDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        stopDistance = agent.stoppingDistance;
    }

    private void Start()
    {
        //实例化出角色的时候加载角色数据
        SaveManager.Instance.LoadPlayerData();
    }

    private void OnEnable()
    {
        //注册角色数据
        GameManager.Instance.RegisterPlayer(characterStats);
        //当OnMouseClicked声明是UnityEvent时用addlistener 
        //MouseManager.Instance.OnMouseClicked.AddListener(MoveToTarget);
        MouseManager.Instance.OnMouseClicked += MoveToTarget; //和unityEvent的 AddListener一样的
        MouseManager.Instance.OnEnemyClicked += EventAttack;
    }

    private void OnDisable()
    {
        if (!MouseManager.IsInitialized) return;
        MouseManager.Instance.OnMouseClicked -= MoveToTarget; //和unityEvent的 AddListener一样的
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }

    private void Update()
    {
        isDead = characterStats.CurrentHealth == 0;
        if (isDead)
        {
            //玩家死亡向所有敌人广播
            GameManager.Instance.NotifyObservers();
        }
        lastAttackTime -= Time.deltaTime;
        SwitchAnimation(); 
    }

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed",agent.velocity.sqrMagnitude);
        anim.SetBool("Death",isDead);
    }

    /// <summary>
    /// 移动到目标点
    /// </summary>
    /// <param name="target"></param>
    private void MoveToTarget(Vector3 target)
    {
        if (isDead) return;
        StopAllCoroutines();
        agent.isStopped = false;
        //点击目标的时候停止距离是初始的stoppingDistance
        agent.stoppingDistance = stopDistance;
        //移动到目标点
        agent.destination = target;
    }
    private void EventAttack(GameObject target)
    {
        if (isDead) return;
        if (target!=null)
        {
            attackTarget = target;
            //判断是否暴击
            characterStats.isCritical = Random.value <= characterStats.attackData.criticalChange;
            Coroutine coroutine= StartCoroutine(MoveToAttackTarget()); 
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        //当攻击的时候，停止距离是武器的攻击距离
        agent.stoppingDistance = characterStats.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        //当play与目标点的距离大于武器的攻击距离时，一直冲向对方
        while (Vector3.Distance(transform.position,attackTarget.transform.position)>characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position; 
            yield return null;
        }
        agent.isStopped = true;
        if (lastAttackTime<0)
        {
            anim.SetBool("Critical",characterStats.isCritical);
            anim.SetTrigger("Attack");
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }
    //---------Animation Event---------
    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            //这会报空
            attackTarget.GetComponent<Rock>()._rockStates = RockStates.HitEnemy;
            attackTarget.GetComponent<Rigidbody>().velocity=Vector3.one;
            attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward*20,ForceMode.Impulse);
        }
        else
        {
            //获取目标的CharacterStats组件
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats,targetStats);
        }
        
    }
}
