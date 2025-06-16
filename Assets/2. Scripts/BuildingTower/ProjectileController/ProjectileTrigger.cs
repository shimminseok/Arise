using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrigger : MonoBehaviour
{
    private ProjectileController _projectileController;
    private IDamageable _target;

    public void SetTarget(ProjectileController owner)
    {
        _projectileController = owner;
        _target = _projectileController.Target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (damageable == _target && _target != null && !_target.IsDead)
            {
                ObjectPoolManager.Instance.ReturnObject(_projectileController.gameObject);
                damageable.TakeDamage(_projectileController.Attacker);
                
            }
        }
    }
}