using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SplashAttackSO", menuName = "TowerAttackType/SplashAttackSO", order = 0)]
public class SplashAttackSO : AttackTypeSO
{
    public override void Attack(TowerController owner)
    {
        var targets = FindTargets(owner);
        if (targets.Count == 0)
            return;

        FireProjectile(owner, targets[0]);
    }

    protected override List<IDamageable> FindTargets(TowerController owner)
    {
        List<IDamageable> targets = new List<IDamageable>();

        for (int i = 0; i < owner.TargetResults.Length; i++)
        {
            if (owner.TargetResults[i] == null)
                continue;
            if (owner.TargetResults[i].TryGetComponent<IDamageable>(out var d) && !d.IsDead)
            {
                targets.Add(d);
            }
        }

        return targets;
    }
}