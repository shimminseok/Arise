using System.Collections.Generic;
using UnityEngine;

public abstract class AttackTypeSO : ScriptableObject
{
    public abstract void Attack(TowerController owner);

    protected void FireProjectile(TowerController owner, IDamageable target)
    {
        GameObject projectile = ObjectPoolManager.Instance.GetObject(owner.ProjectilePoolId);
        if (projectile.TryGetComponent(out ProjectileController proj))
        {
            proj.transform.position = owner.FireTransform.position;
            proj.SetTarget(owner, target);
        }
    }

    protected abstract List<IDamageable> FindTargets(TowerController owner);
}