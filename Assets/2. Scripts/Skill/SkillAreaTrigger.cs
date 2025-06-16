using System.Collections.Generic;
using UnityEngine;

public class SkillAreaTrigger : MonoBehaviour
{
    private Skill _ownerSkill;
    private HashSet<IDamageable> _hitTargets = new HashSet<IDamageable>();

    public void Initialize(Skill ownerSkill, float duration)
    {
        _ownerSkill = ownerSkill;
        Destroy(this, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_ownerSkill == null) return;

        if (other.TryGetComponent<IDamageable>(out var target))
        {
            if (_hitTargets.Contains(target)) return;
            _hitTargets.Add(target);
            _ownerSkill.ApplyEffects(target);
        }
    }
}