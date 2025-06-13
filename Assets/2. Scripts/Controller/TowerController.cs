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
            TowerState.Attack => new AttackState(),
            _                 => null
        };
    }

    public override void FindTarget()
    {
    }


    public void InitFromPool()
    {
        StatManager.Initialize(towerSO);
    }


    public void Attack()
    {
    }
}