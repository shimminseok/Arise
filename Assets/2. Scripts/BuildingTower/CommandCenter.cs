using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
public class CommandCenter : SceneOnlySingleton<CommandCenter>, IDamageable
{
    [SerializeField] private List<AttackPoint> attackPoints;
    [SerializeField] private CommandCenterSO commandCenterSo;
    [SerializeField] private BoxCollider m_Collider;
    public bool        IsDead      { get; private set; }
    public StatManager StatManager { get; private set; }
    public Collider    Collider    => m_Collider;

    protected override void Awake()
    {
        base.Awake();
        StatManager = GetComponent<StatManager>();
        m_Collider = GetComponent<BoxCollider>();
        StatManager.Initialize(commandCenterSo, this);
    }

    public void TakeDamage(IAttackable attacker)
    {
        //TODO 방어력 계산
        float finalDam = attacker.AttackStat.Value;

        StatManager.Consume(StatType.CurHp, StatModifierType.Base, finalDam);

        if (StatManager.GetValue(StatType.CurHp) <= 0)
        {
            Dead();
        }
    }

    public AttackPoint GetAvailablePoint()
    {
        foreach (AttackPoint point in attackPoints)
        {
            if (point.TryReserve())
            {
                return point;
            }
        }

        return null;
    }

    public void Dead()
    {
        IsDead = true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}