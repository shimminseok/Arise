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
        var results = new Collider[owner.TowerSO.ProjectileCount];
        int size = Physics.OverlapSphereNonAlloc(
            owner.transform.position,
            owner.StatManager.GetValue(StatType.AttackRange),
            results,
            LayerMask.GetMask("Enemy")
        );

        var targets = new List<IDamageable>();
        for (int i = 0; i < size && targets.Count < owner.TowerSO.ProjectileCount; i++)
        {
            if (results[i].TryGetComponent<IDamageable>(out var damageable))
                targets.Add(damageable);
        }

        return targets;
    }
}