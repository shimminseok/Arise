using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectShape : IAreaShape
{
    private Vector3 _size;
    private Quaternion _rotation;
    private float _offset;
    private Transform _origin;

    public RectShape(Vector3 size, Quaternion rotation, float offset)
    {
        _size = size;
        _rotation = rotation;
        _offset = offset;
    }

    public void SpawnAreaCollider(GameObject targetObject, Skill ownerSkill, float duration, Transform origin)
    {
        BoxCollider col = targetObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = _size;
        col.center = origin.transform.forward * _offset;

        var trigger = targetObject.AddComponent<SkillAreaTrigger>();
        trigger.Initialize(ownerSkill, duration);

        _origin = origin;
    }
    
    public void DrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 center = _origin.position + _origin.forward * _offset;
        Gizmos.matrix = Matrix4x4.TRS(center, _rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, _size);
    }
}
