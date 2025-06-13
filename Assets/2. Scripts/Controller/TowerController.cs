using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerStates;

public enum TowerState
{
    Idle,
    Attack
}

[RequireComponent(typeof(BuildingData))]
[RequireComponent(typeof(CapsuleCollider))]
public class TowerController : BaseController<TowerController, TowerState>, IPoolObject, IAttackable
{
    [SerializeField] private string poolId;
    [SerializeField] private int poolSize;
    [SerializeField] private TowerSO towerSO;
    [SerializeField] private string ProjectilePoolId;
    [SerializeField] private Transform fireTransform;

    public StatBase     AttackStat   { get; private set; }
    public IDamageable  Target       { get; private set; }
    public BuildingData BuildingData { get; private set; }
    public bool         IsPlaced     { get; private set; }

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolId;
    public int        PoolSize   => poolSize;

    private Collider m_Collider;

    protected override void Awake()
    {
        base.Awake();
        BuildingData = GetComponent<BuildingData>();
        m_Collider = GetComponent<CapsuleCollider>();
    }

    protected override void Start()
    {
        base.Start();
        AttackStat = StatManager.GetStat<CalculatedStat>(StatType.AttackPow);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override IState<TowerController, TowerState> GetState(TowerState state)
    {
        return state switch
        {
            TowerState.Idle   => new IdleState(),
            TowerState.Attack => new AttackState(StatManager.GetValue(StatType.AttackSpd), StatManager.GetValue(StatType.AttackRange)),
            _                 => null
        };
    }

    public float GetTargetDistance()
    {
        if (Target != null && !Target.IsDead)
        {
            float distance = Utility.GetSqrDistanceBetween(m_Collider, Target.Collider);
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

    public override void FindTarget()
    {
        var results = new Collider[10];
        var size    = Physics.OverlapSphereNonAlloc(transform.position, StatManager.GetValue(StatType.AttackRange), results, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < size; i++)
        {
            if (results[i].TryGetComponent<IDamageable>(out var damageable))
            {
                Target = damageable;
                break;
            }
        }
    }


    public void OnSpawnFromPool()
    {
        IsPlaced = false;
        StatManager.Initialize(towerSO);
    }

    public void OnReturnToPool()
    {
    }

    public void OnBuildComplete()
    {
        IsPlaced = true;
    }


    public void Attack()
    {
        GameObject projectile = ObjectPoolManager.Instance.GetObject(ProjectilePoolId);
        if (projectile.TryGetComponent<ProjectileController>(out var projectileController))
        {
            projectileController.transform.position = fireTransform.position;
            projectileController.SetTarget(this, Target);
        }
    }
}