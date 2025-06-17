using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShape : IAreaShape
{
    private float _radius;
    private float _offset;
    private Transform _origin;

    public CircleShape(float radius, float offset)
    {
        _radius = radius;
        _offset = offset;
    }
    
    public void SpawnAreaCollider(GameObject targetObject, Skill ownerSkill, float duration, Transform origin)
    {
        SphereCollider col = targetObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = _radius;
        col.center = origin.transform.forward * _offset;

        var trigger = targetObject.AddComponent<SkillAreaTrigger>();
        trigger.Initialize(ownerSkill, duration);

        _origin = origin;
    }
    
    public void DrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_origin.position + _origin.forward * _offset, _radius);
    }
}
