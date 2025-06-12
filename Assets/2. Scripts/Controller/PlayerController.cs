using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerStates;
using UnityEngine;

public class PlayerController : BaseController<PlayerController, PlayerState>, IAttackable, IDamageable
{
    public StatBase         AttackStat       { get; private set; }
    public IDamageable      Target           { get; private set; }
    public bool             IsDead           { get; private set; }
    public Transform        Transform        => transform;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// 플레이어의 State를 생성해주는 팩토리 입니다.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    protected override IState<PlayerController, PlayerState> GetState(PlayerState state)
    {
        return state switch
        {
            // PlayerState.Idle   => new IdleState(),
            // PlayerState.Move   => new MoveState(),
            // PlayerState.Attack => new AttackState(StatManager.GetValue(StatType.AttackSpd), StatManager.GetValue(StatType.AttackRange)),
            _                  => null
        };
    }

    public override void Movement()
    {

    }

    public void Attack()
    {
        Target?.TakeDamage(this);
    }

    public override void FindTarget()
    {

    }

    public void TakeDamage(IAttackable attacker)
    {
        if (Target == null)
            Target = attacker as IDamageable;
        
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
        print($"플레이어 사망");
    }
}