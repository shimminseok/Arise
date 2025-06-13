using System;
using System.Collections;
using System.Collections.Generic;
using EnemyStates;
using UnityEngine;


public class EnemyController : BaseController<EnemyController, EnemyState>, IPoolObject, IAttackable, IDamageable
{
    [SerializeField] private string poolID;
    [SerializeField] private int poolSize;
    [SerializeField] private MonsterSO m_MonsterSo;

    public StatBase    AttackStat     { get; private set; }
    public IDamageable Target         { get; private set; }
    public bool        IsDead         { get; private set; }
    public Vector3     TargetPosition { get; private set; }
    public Transform   Transform      => transform;
    public GameObject  GameObject     => gameObject;
    public string      PoolID         => poolID;
    public int         PoolSize       => poolSize;

    private HPBarUI healthBarUI;

    protected override void Awake()
    {
        base.Awake();
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
            EnemyState.Attack => new AttackState(),
            EnemyState.Die    => new DeadState(),
            _                 => null
        };
    }

    public void OnSpawnFromPool()
    {
        Target = null;
        IsDead = false;
        Agent.Warp(EnemyManager.Instance.StartPoint);
        TargetPosition = EnemyManager.Instance.Endpoint;
        StatManager.Initialize(m_MonsterSo);
    }

    public void OnReturnToPool()
    {
        Agent.ResetPath();
        transform.position = Vector3.zero;
    }

    public override void FindTarget()
    {
        if (Target != null && Target.IsDead)
            return;
    }

    public override void Movement()
    {
        if (Target != null && Agent.isOnNavMesh)
        {
        }

        Agent.speed = StatManager.GetValue(StatType.MoveSpeed);
        Agent.SetDestination(TargetPosition);
    }

    public void SetTargetPosition(Vector3 position)
    {
        TargetPosition = position;
    }

    public void Movement(Vector3 direction)
    {
    }


    public void Attack()
    {
        Target?.TakeDamage(this);
    }

    public void TakeDamage(IAttackable attacker)
    {
        if (healthBarUI == null)
        {
            healthBarUI = HealthBarManager.Instance.SpawnHealthBar(this);
            StatManager.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged += healthBarUI.UpdateHealthBarWrapper;
        }


        StatManager.Consume(StatType.CurHp, attacker.AttackStat.Value);

        float curHp = StatManager.GetValue(StatType.CurHp);
        if (curHp <= 0)
        {
            Daed();
        }
    }

    public void Daed()
    {
        IsDead = true;
        Target = null;
        StatusEffectManager.RemoveAllEffects();
        EnemyManager.Instance.MonsterDead(this);
        ChangeState(EnemyState.Idle);
        QuestManager.Instance.UpdateProgress(QuestType.KillEnemies, 1);
        healthBarUI.UnLink();
        StatManager.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged -= healthBarUI.UpdateHealthBarWrapper;
        healthBarUI = null;
    }
}