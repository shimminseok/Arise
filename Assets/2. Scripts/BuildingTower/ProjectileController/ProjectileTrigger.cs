using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

public class ProjectileTrigger : MonoBehaviour
{
    private ProjectileController _projectileController;
    private IDamageable _target;
    private float _splashRadius;

    public void SetTarget(ProjectileController owner, float splashRadius)
    {
        _projectileController = owner;
        _target = _projectileController.Target;
        _splashRadius = splashRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (damageable == _target && _target != null && !_target.IsDead)
            {
                if (_splashRadius > 0)
                {
                    ApplySplashDamage(damageable);
                }
                else
                {
                    damageable.TakeDamage(_projectileController.Attacker);
                }
                ObjectPoolManager.Instance.ReturnObject(_projectileController.gameObject);
                foreach (StatusEffectSO statusEffect in _projectileController.OwnerTower.StatusEffects)
                {
                    foreach (StatusEffectData effect in statusEffect.StatusEffects)
                    {
                        if (_target.Collider.TryGetComponent(out StatusEffectManager statusEffectManager))
                        {
                            statusEffectManager.ApplyEffect(BuffFactory.CreateBuff(statusEffect.ID, effect));
                        }
                    }
                }
            }
        }
    }

    private void ApplySplashDamage(IDamageable centerTarget)
    {
        Vector3 center  = centerTarget.Collider.bounds.center;
        var     results = new Collider[20];
        int     size    = Physics.OverlapSphereNonAlloc(center, _projectileController.SplashRadius, results, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < size; i++)
        {
            if (results[i].TryGetComponent<IDamageable>(out var splashTarget) && !splashTarget.IsDead)
            {
                splashTarget.TakeDamage(_projectileController.Attacker);

            }
        }
    }
}