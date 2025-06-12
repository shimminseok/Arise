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
public class TowerController : BaseController<TowerController, TowerState>, IPoolObject
{
    [SerializeField] private string poolId;
    [SerializeField] private int poolSize;
    public GameObject GameObject => gameObject;
    public string     PoolID     => poolId;
    public int        PoolSize   => poolSize;

    public BuildingData BuildingData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        BuildingData = GetComponent<BuildingData>();
    }

    protected override void Start()
    {
        base.Start();
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

    public override void Movement()
    {
    }

    public void InitFromPool()
    {
    }
}