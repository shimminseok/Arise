using System;
using System.Collections;
using System.Collections.Generic;
using EnemyStates;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyController : BaseController<EnemyController, EnemyState>, IPoolObject, IAttackable, IDamageable
{
    [SerializeField] private string poolID;
    [SerializeField] private int poolSize;
    [SerializeField] private MonsterSO m_MonsterSo;
    public StatBase     AttackStat     { get; private set; }
    public IDamageable  Target         { get; private set; }
    public bool         IsDead         { get; private set; }
    public Collider     Collider       { get; private set; }
    public Vector3      TargetPosition { get; private set; }
    public NavMeshAgent Agent          { get; private set; }
    public Animator     Animator       { get; private set; }

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolID;
    public int        PoolSize   => poolSize;
    public MonsterSO  MonsterSo  => m_MonsterSo;
    private HPBarUI _healthBarUI;
    private AttackPoint _assignedPoint;

    protected override void Awake()
    {
        base.Awake();
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<CapsuleCollider>();
    }

    protected override void Start()
    {
        base.Start();
        AttackStat = StatManager.GetStat<CalculatedStat>(StatType.AttackPow);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    protected override IState<EnemyController, EnemyState> GetState(EnemyState state)
    {
        return state switch
        {
            EnemyState.Idle   => new IdleState(),
            EnemyState.Move   => new MoveState(),
            EnemyState.Attack => new AttackState(StatManager.GetValue(StatType.AttackSpd), StatManager.GetValue(StatType.AttackRange)),
            EnemyState.Die    => new DeadState(),
            _                 => null
        };
    }

    public void Initialized(Vector3 startPos, Vector3 targetPos)
    {
        Agent.Warp(startPos);
        TargetPosition = targetPos;
        IsDead = false;
        OnSpawnFromPool();
        Agent.enabled = true;
        Collider.enabled = true;
    }

    public void OnSpawnFromPool()
    {
        Target = CommandCenter.Instance;
        StatManager.Initialize(m_MonsterSo, this);
    }

    public void OnReturnToPool()
    {
        Target = null;
        transform.position = Vector3.zero;
    }

    public override void FindTarget()
    {
        if (Target != null && Target.IsDead)
            return;
    }

    public override void Movement()
    {
        if (Agent.isOnNavMesh)
        {
            //Debug.Log("Update Movement");
            Agent.speed = StatManager.GetValue(StatType.MoveSpeed);
            Agent.SetDestination(TargetPosition);
        }
    }

    public void StopMovement()
    {
        if (Agent.enabled)
        {
            Agent.ResetPath();
            Agent.isStopped = true;
            Agent.velocity = Vector3.zero;
        }
    }

    public void AssignAttackPoint()
    {
        _assignedPoint = CommandCenter.Instance.GetAvailablePoint();
        if (_assignedPoint != null)
        {
            TargetPosition = _assignedPoint.Collider.ClosestPoint(transform.position);
        }
    }

    public void SetTargetPosition(Vector3 dis)
    {
        TargetPosition = dis;
    }

    public float GetTargetDistance()
    {
        if (Target != null && !Target.IsDead)
        {
            float distance = Utility.GetSqrDistanceBetween(Collider, Target.Collider);
            
            return distance;
        }

        return Mathf.Infinity;
    }

    public bool IsTargetInAttackRange()
    {
        float attackRange    = StatManager.GetValue(StatType.AttackRange);
        float sqrAttackRange = attackRange * attackRange;
        return GetTargetDistance() <= sqrAttackRange;
    }


    public void Attack()
    {
        Target?.TakeDamage(this);
    }


    public void TakeDamage(IAttackable attacker)
    {
        if (IsDead)
            return;
        
        if (_healthBarUI == null)
        {
            _healthBarUI = HealthBarManager.Instance.SpawnHealthBar(this);
            StatManager.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged += _healthBarUI.UpdateHealthBarWrapper;
        }

        float finalDam = attacker.AttackStat.Value * (100 / (100 + StatManager.GetValue(StatType.Defense)));
        StatManager.Consume(StatType.CurHp, StatModifierType.Base, finalDam);

        float curHp = StatManager.GetValue(StatType.CurHp);
        if (curHp <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        if (IsDead)
            return;
        IsDead = true;
        Target = null;
        StatusEffectManager.RemoveAllEffects();
        QuestManager.Instance.UpdateProgress(QuestType.KillEnemies, 1);



        
        
        if (_healthBarUI != null)
        {
            _healthBarUI.UnLink();
            StatManager.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged -= _healthBarUI.UpdateHealthBarWrapper;
        }
        _assignedPoint?.Release();
        _healthBarUI = null;
        Agent.enabled = false;
        Collider.enabled = false;
        ChangeState(EnemyState.Idle);
        
        if (TutorialEnemyManager.Instance != null)
        {
            TutorialEnemyManager.Instance.OnEnemyKilled(this);
        }
        else if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.MonsterDead(this);
        }
    }
}