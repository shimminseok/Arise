using System;
using System.Collections;
using System.Collections.Generic;
using EnemyStates;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class EnemyController : BaseController<EnemyController, EnemyState>, IPoolObject, IAttackable, IDamageable
{
    [SerializeField] private string poolID;
    [SerializeField] private int poolSize;
    [SerializeField] private MonsterSO m_MonsterSo;
    public StatBase    AttackStat     { get; private set; }
    public IDamageable Target         { get; private set; }
    public bool        IsDead         { get; private set; }
    public Collider    Collider       { get; private set; }
    public Vector3     TargetPosition { get; private set; }
    public GameObject  GameObject     => gameObject;
    public string      PoolID         => poolID;
    public int         PoolSize       => poolSize;

    private HPBarUI _healthBarUI;
    private AttackPoint _assignedPoint;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        AttackStat = StatManager.GetStat<CalculatedStat>(StatType.AttackPow);
        Collider = GetComponent<CapsuleCollider>();
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
            EnemyState.Attack => new AttackState(StatManager.GetValue(StatType.AttackPow), StatManager.GetValue(StatType.AttackRange)),
            EnemyState.Die    => new DeadState(),
            _                 => null
        };
    }

    public void Initialized(Vector3 startPos, Vector3 targetPos)
    {
        Agent.Warp(startPos);
        TargetPosition = targetPos;
        OnSpawnFromPool();
    }

    public void OnSpawnFromPool()
    {
        Target = CommandCenter.Instance;
        IsDead = false;
        StatManager.Initialize(m_MonsterSo);
    }

    public void OnReturnToPool()
    {
        Agent.ResetPath();
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
            Agent.speed = StatManager.GetValue(StatType.MoveSpeed);
            Agent.SetDestination(TargetPosition);
        }
    }

    public void AssignAttackPoint()
    {
        _assignedPoint = CommandCenter.Instance.GetAvailablePoint();
        if (_assignedPoint != null)
        {
            TargetPosition = _assignedPoint.transform.position;
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
        if (_healthBarUI == null)
        {
            _healthBarUI = HealthBarManager.Instance.SpawnHealthBar(this);
            StatManager.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged += _healthBarUI.UpdateHealthBarWrapper;
        }


        StatManager.Consume(StatType.CurHp, attacker.AttackStat.Value);

        float curHp = StatManager.GetValue(StatType.CurHp);
        if (curHp <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        IsDead = true;
        Target = null;
        StatusEffectManager.RemoveAllEffects();
        EnemyManager.Instance.MonsterDead(this);
        ChangeState(EnemyState.Idle);
        QuestManager.Instance.UpdateProgress(QuestType.KillEnemies, 1);
        _healthBarUI.UnLink();
        StatManager.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged -= _healthBarUI.UpdateHealthBarWrapper;
        _assignedPoint?.Release();
        _healthBarUI = null;
    }
}