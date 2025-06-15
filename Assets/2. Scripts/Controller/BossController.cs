using System;
using System.Collections;
using System.Collections.Generic;
using BossStates;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
public class BossController : BaseController<BossController, BossState>, IPoolObject, IAttackable, IDamageable
{
    [SerializeField] private string poolID;
    [SerializeField] private int poolSize;
    [SerializeField] private BossSO m_BossSo;
    public StatBase     AttackStat     { get; private set; }
    public IDamageable  Target         { get; private set; }
    public bool         IsDead         { get; private set; }
    public Collider     Collider       { get; private set; }
    public Vector3      TargetPosition { get; private set; }
    public NavMeshAgent Agent          { get; private set; }

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolID;
    public int        PoolSize   => poolSize;

    private HPBarUI _healthBarUI;
    private AttackPoint _assignedPoint;

    protected override void Awake()
    {
        base.Awake();
        Agent = GetComponent<NavMeshAgent>();
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


    protected override IState<BossController,BossState> GetState(BossState state)
    {
        return state switch
        {
            BossState.Idle   => new IdleState(),
            BossState.Move   => new MoveState(),
            BossState.Attack => new AttackState(StatManager.GetValue(StatType.AttackPow), StatManager.GetValue(StatType.AttackRange)),
            BossState.Die    => new DeadState(),
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
        StatManager.Initialize(m_BossSo);
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
        /*  형변환 안되고 hpbar 관리 새로 짜주거나 basecontroller로 바꾸기
        if (_healthBarUI == null)
        {
            _healthBarUI = HealthBarManager.Instance.SpawnHealthBar(this);
            StatManager.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged += _healthBarUI.UpdateHealthBarWrapper;
        }
        */

        //TODO 방어력 계산
        float finalDam = attacker.AttackStat.Value;
        StatManager.Consume(StatType.CurHp, finalDam);

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
        BossManager.Instance.BossDead(this);
        ChangeState(BossState.Idle);
        QuestManager.Instance.UpdateProgress(QuestType.KillEnemies, 1);
        _healthBarUI.UnLink();
        StatManager.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged -= _healthBarUI.UpdateHealthBarWrapper;
        _assignedPoint?.Release();
        _healthBarUI = null;
    }
}