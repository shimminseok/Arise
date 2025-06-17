using System.Collections.Generic;
using UnityEngine;

public class SkillAreaTrigger : MonoBehaviour
{
    private Skill _ownerSkill;
    private HashSet<IDamageable> _hitTargets = new HashSet<IDamageable>();
    private float _angle = 360f;
    private Transform _origin;
    private bool _isAngleType;
    
    public void Initialize(Skill ownerSkill, float duration)
    {
        _ownerSkill = ownerSkill;
        Destroy(this, duration);
    }
    
    public void Initialize(Skill ownerSkill, float duration, float angle, Transform origin)
    {
        _ownerSkill = ownerSkill;
        _angle = angle;
        _origin = origin;
        _isAngleType = true;
        Destroy(this, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_ownerSkill == null) return;
        
        if (_isAngleType && !IsInAngle(other.transform)) return;

        if (other.TryGetComponent<IDamageable>(out var target))
        {
            if (_hitTargets.Contains(target)) return;
            _hitTargets.Add(target);
            _ownerSkill.ApplyEffects(target);
        }
    }
    
    private bool IsInAngle(Transform target)
    {
        Vector3 toTarget = (target.position - _origin.position).normalized;
        float dot = Vector3.Dot(_origin.forward, toTarget);
        float angleToTarget = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return angleToTarget <= _angle / 2f;
    }
}