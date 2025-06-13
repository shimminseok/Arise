using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrigger : MonoBehaviour
{
    ProjectileController projectileController;

    private IDamageable _target;

    public void SetTarget(ProjectileController owner)
    {
        projectileController = owner;
        _target = projectileController.Target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (damageable == _target && _target != null && !_target.IsDead)
            {
                Debug.Log("대미지!");
                ObjectPoolManager.Instance.ReturnObject(projectileController.gameObject);
                damageable.TakeDamage(projectileController.Attacker);
            }
        }
    }
}