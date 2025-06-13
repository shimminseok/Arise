using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
public class CommandCenter : MonoBehaviour, IDamageable
{
    public bool      IsDead    { get; private set; }
    public Transform Transform => transform;

    public StatManager StatManager { get; private set; }

    public void Awake()
    {
        StatManager = GetComponent<StatManager>();
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void TakeDamage(IAttackable attacker)
    {
        //방어력 계산
        float finalDam = attacker.AttackStat.Value;

        StatManager.Consume(StatType.CurHp, finalDam);

        if (StatManager.GetValue(StatType.CurHp) <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        IsDead = true;
    }
}