using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MultishotTypeSO", menuName = "TowerAttackType/MultishotTypeSO", order = 0)]
public class MultishotAttackSO : AttackTypeSO
{
    public override void Attack(TowerController owner)
    {
        var targets = FindTargets(owner);
        foreach (var target in targets)
        {
            FireProjectile(owner, target);
        }
    }

    protected override List<IDamageable> FindTargets(TowerController owner)
    {
        var results = owner.TargetResults;

        var targets = new List<IDamageable>();
        targets.Add(owner.Target);
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i] == null)
                continue;
            if (results[i].TryGetComponent<IDamageable>(out var damageable) && !damageable.IsDead && !targets.Contains(damageable))
            {
                targets.Add(damageable);
            }
        }

        return targets;
    }
}