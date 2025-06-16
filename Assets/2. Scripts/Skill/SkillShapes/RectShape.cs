using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectShape : IAreaShape
{
    private Vector3 _size;
    private Quaternion _rotation;

    public RectShape(Vector3 size, Quaternion rotation)
    {
        _size = size;
        _rotation = rotation;
    }

    public void SpawnAreaCollider(GameObject targetObject, Skill ownerSkill, float duration)
    {
        BoxCollider col = targetObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = _size;

        var trigger = targetObject.AddComponent<SkillAreaTrigger>();
        trigger.Initialize(ownerSkill, duration);
    }
    
    public void DrawGizmos(Transform origin)
    {
        Gizmos.color = Color.red;

        Vector3 center = origin.position;
        Vector3 halfSize = new Vector3(_size.x / 2, 0f, _size.y / 2);

        Vector3[] corners = new Vector3[4];
        corners[0] = center + _rotation * new Vector3(-halfSize.x, 0, -halfSize.z);
        corners[1] = center + _rotation * new Vector3(-halfSize.x, 0, halfSize.z);
        corners[2] = center + _rotation * new Vector3(halfSize.x, 0, halfSize.z);
        corners[3] = center + _rotation * new Vector3(halfSize.x, 0, -halfSize.z);

        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
        }
    }
}
