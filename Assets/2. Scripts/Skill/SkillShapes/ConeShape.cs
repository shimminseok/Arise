using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeShape : IAreaShape
{
    private float _radius;
    private float _offset;
    private float _angle;
    private Transform _origin;

    public ConeShape(float radius, float angle, float offset)
    {
        _radius = radius;
        _offset = offset;
        _angle = angle;
    }
    
    public void SpawnAreaCollider(GameObject targetObject, Skill ownerSkill, float duration, Transform origin)
    {
        SphereCollider col = targetObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = _radius;
        col.center = origin.transform.forward * _offset;

        var trigger = targetObject.AddComponent<SkillAreaTrigger>();
        trigger.Initialize(ownerSkill, duration, _angle, origin);

        _origin = origin;
    }
    
    public void DrawGizmos()
    {
        if (_origin == null) return;

        Vector3 center = _origin.position + _origin.forward * _offset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, _radius);

        Vector3 leftDir = Quaternion.Euler(0, -_angle / 2f, 0) * _origin.forward;
        Vector3 rightDir = Quaternion.Euler(0, _angle / 2f, 0) * _origin.forward;

        Gizmos.DrawLine(center, center + leftDir * _radius);
        Gizmos.DrawLine(center, center + rightDir * _radius);
    }
}
