using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerStates;

public enum TowerState
{
    Idle,
    Attack
}

public class TowerController : BaseController<TowerController, TowerState>
{
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
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
}