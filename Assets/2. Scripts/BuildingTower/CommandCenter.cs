using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
public class CommandCenter : SceneOnlySingleton<CommandCenter>, IDamageable
{
    [SerializeField] private List<AttackPoint> attackPoints;
    [SerializeField] private CommandCenterSO commandCenterSo;
    [SerializeField] private BoxCollider m_Collider;

    [SerializeField] private VoidEventChannelSO gameOverEvent;
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
        float finalDam = attacker.AttackStat.Value * (100 / (100 + StatManager.GetValue(StatType.Defense)));

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
        if (IsDead)
            return;
        
        IsDead = true;
        gameOverEvent?.Raise();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}