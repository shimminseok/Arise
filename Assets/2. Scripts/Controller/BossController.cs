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
    public StatBase AttackStat { get; private set; }
    public IDamageable Target { get; private set; }
    public bool IsDead { get; private set; }
    public Collider Collider { get; private set; }
    public Vector3 TargetPosition { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolID;
    public int        PoolSize   => poolSize;
    public BossSO     BossSo     => m_BossSo;
    
    
    private HPBarUI _healthBarUI;
    private AttackPoint _assignedPoint;

    //스킬 프리팹 2개
    public GameObject earthQuakeSkill;
    public Animator animator;
    [SerializeField] private string[] BossSkillPoolId;

    public bool istest;

    public bool skillStateFinished;
    private Collider[] results = new Collider[3];
    public StatusEffectSO statusEffectSO;
    protected override void Awake()
    {
        base.Awake();
        Agent = GetComponent<NavMeshAgent>();
        Collider = GetComponent<CapsuleCollider>();
    }

    protected override void Start()
    {
        base.Start();
        AttackStat = StatManager.GetStat<CalculatedStat>(StatType.AttackPow);
        StartCoroutine(SetTestTrueRoutine());
    }

    IEnumerator SetTestTrueRoutine()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(2f,4f);
            yield return new WaitForSeconds(waitTime);

            istest = true;
            yield return null; // 1프레임 후 꺼짐 
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    //          _                 => null  ->알 수 없는 값이 들어오면: _ => null 실행 → null 리턴
    protected override IState<BossController, BossState> GetState(BossState state)
    {
        return state switch
        {
            BossState.Idle   => new IdleState(),
            BossState.Move   => new MoveState(),
            BossState.Attack => new AttackState(StatManager.GetValue(StatType.AttackSpd), StatManager.GetValue(StatType.AttackRange)),
            BossState.Die    => new DeadState(),
            BossState.Skill  => new SkillState(),
            _                => null
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
        StatManager.Initialize(m_BossSo, this);
    }

    public void OnReturnToPool()
    {
        Target = null;
        transform.position = Vector3.zero;
    }

    //20범위내에 Water layer를 가진 오브젝트 10개 삭제
    public override void FindTarget()
    {
        var results = new Collider[10];
        var size = Physics.OverlapSphereNonAlloc(
            transform.position,
             20,
              results, LayerMask.GetMask("Water"));
        for (int i = 0; i < size; i++)
        {
            results[i].transform.gameObject.SetActive(false);
            /*
                        if (results[i].TryGetComponent<IDamageable>(out var damageable))
                        {
                            Target = damageable;
                            break;
                        }
                        */
        }
    }

    public void FindTargetByEnum(BossSkillName bossSkillName)
    {
        switch (bossSkillName)
        {
            case BossSkillName.EarthQuake:
                EarthQuake();
                break;
            case BossSkillName.Dispel:
                Dispel();
                break;
        }


    }

    public void EarthQuake()
    {
        //Physics.OverlapSphereNonAlloc->랜덤으로 값을 받음
        var size = Physics.OverlapSphereNonAlloc(
            transform.position,
             20,
              results, LayerMask.GetMask("Tower"));
        for (int i = 0; i < size; i++)
        {
            results[i].transform.gameObject.SetActive(false);
        }
    }

    public void Dispel()
    {

        var size = Physics.OverlapSphereNonAlloc(
            transform.position,
             20,
              results, LayerMask.GetMask("Tower"));
        for (int i = 0; i < size; i++)
        {
                foreach (var effectData in statusEffectSO.StatusEffects)
                {
                    results[i].GetComponent<StatusEffectManager>().ApplyEffect(BuffFactory.CreateBuff(statusEffectSO.ID, effectData));
                }
        }
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
        float attackRange = StatManager.GetValue(StatType.AttackRange);
        float sqrAttackRange = attackRange * attackRange;
        return GetTargetDistance() <= sqrAttackRange;
    }


    public void Attack()
    {
        Target?.TakeDamage(this);
    }

    public void TakeDamage(IAttackable attacker)
    {
        //*  형변환 안되고 hpbar 관리 새로 짜주거나 basecontroller로 바꾸기
        if (_healthBarUI == null)
        {
            _healthBarUI = HealthBarManager.Instance.SpawnHealthBar(this);
            StatManager.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged += _healthBarUI.UpdateHealthBarWrapper;
        }

        //TODO 방어력 계산
        float finalDam = attacker.AttackStat.Value;
        StatManager.Consume(StatType.CurHp, StatModifierType.Base, finalDam);

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
        Agent.ResetPath();
        Agent.enabled = false;
        Collider.enabled = false;
        StatusEffectManager.RemoveAllEffects();
        EnemyManager.Instance.MonsterDead(this);
        ChangeState(BossState.Idle);
        QuestManager.Instance.UpdateProgress(QuestType.KillEnemies, 1);
        if (_healthBarUI != null)
        {
            _healthBarUI.UnLink();
            StatManager.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged -= _healthBarUI.UpdateHealthBarWrapper;
        }
        _assignedPoint?.Release();
        _healthBarUI = null;
    }

    public void FireSkill(BossSkillName bossSkillName)
    {
        //results

        if (bossSkillName.Equals(BossSkillName.EarthQuake))
        {
            GameObject projectile = ObjectPoolManager.Instance.GetObject(bossSkillName.ToString());
            if (projectile.TryGetComponent<BossSkillController>(out var BossSkillController))
            {
                BossSkillController.transform.position = transform.position;
                BossSkillController.SetTarget(this, Target);
            }
        }
        if (bossSkillName.Equals(BossSkillName.Dispel))
        {
            for (int i = 0; i < results.Length - 1; i++)
            {
                
                GameObject projectile = ObjectPoolManager.Instance.GetObject(bossSkillName.ToString());
                if (projectile.TryGetComponent<BossSkillController>(out var BossSkillController))
                {
                    BossSkillController.transform.position = results[i].transform.position;
                    BossSkillController.SetTarget(this, Target);
                }
            }
        }

    }

}