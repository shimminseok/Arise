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
        var results   = new List<IDamageable>();
        var colliders = new Collider[1];
        int size = Physics.OverlapSphereNonAlloc(owner.transform.position, owner.StatManager.GetValue(StatType.AttackRange), colliders,
            LayerMask.GetMask("Enemy")
        );

        for (int i = 0; i < size; i++)
        {
            if (colliders[i].TryGetComponent<IDamageable>(out var d) && !d.IsDead)
            {
                results.Add(d);
            }
        }

        return results;
    }
}