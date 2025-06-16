using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShape : IAreaShape
{
    private float _radius;

    public CircleShape(float radius)
    {
        _radius = radius;
    }
    
    public void SpawnAreaCollider(GameObject targetObject, Skill ownerSkill, float duration)
    {
        SphereCollider col = targetObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = _radius;

        var trigger = targetObject.AddComponent<SkillAreaTrigger>();
        trigger.Initialize(ownerSkill, duration);
    }
    
    public void DrawGizmos(Transform origin)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin.position, _radius);
    }
}
