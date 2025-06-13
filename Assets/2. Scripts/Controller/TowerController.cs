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
public class TowerController : BaseController<TowerController, TowerState>, IPoolObject, IAttackable
{
    [SerializeField] private string poolId;
    [SerializeField] private int poolSize;
    [SerializeField] private TowerSO towerSO;

    public StatBase     AttackStat   { get; private set; }
    public IDamageable  Target       { get; private set; }
    public BuildingData BuildingData { get; private set; }

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolId;
    public int        PoolSize   => poolSize;

    protected override void Awake()
    {
        base.Awake();
        BuildingData = GetComponent<BuildingData>();
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
            return Vector3.Distance(transform.position, Target.Transform.position);
        }

        return Mathf.Infinity;
    }

    public override void FindTarget()
    {
        var results = new Collider[10];
        var size    = Physics.OverlapSphereNonAlloc(transform.position, StatManager.GetValue(StatType.AttackRange) * 10, results, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < size; i++)
        {
            if (results[i].TryGetComponent<IDamageable>(out var damageable))
            {
                //어택 타입에 따라서 다르게 가져오기
                //test
                Target = damageable;
                break;
            }
        }
    }


    public void OnSpawnFromPool()
    {
        StatManager.Initialize(towerSO);
    }

    public void OnReturnToPool()
    {
    }


    public void Attack()
    {
        Debug.Log("Tower Attack");
        Target.TakeDamage(this);
    }
}