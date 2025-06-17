using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAreaShape
{
    public void SpawnAreaCollider(GameObject targetObject, Skill ownerSkill, float duration, Transform origin);
    public void DrawGizmos();
}
