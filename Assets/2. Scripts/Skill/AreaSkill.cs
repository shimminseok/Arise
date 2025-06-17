using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSkill : Skill
{
    protected float _offset;
    protected IAreaShape shape;

    public void Initialize(AreaSkillSO data)
    {
        base.Initialize(data);

        switch (data.ShapeType)
        {
            case AreaSkillSO.Shape.Circle:
                shape = new CircleShape(data.Radius, data.Offset);
                break;
            case AreaSkillSO.Shape.Rect:
                shape = new RectShape(data.RectSize, data.Owner.transform.rotation, data.Offset);
                break;
            case AreaSkillSO.Shape.Cone:
                shape = new ConeShape(data.Radius, data.Angle, data.Offset);
                break;
        }

        _offset = data.Offset;
    }
    
    protected override void Apply(Transform origin)
    {
        transform.position = origin.position;
        Instantiate(SkillData.SkillPrefab, origin.position + origin.forward * _offset, origin.rotation * Quaternion.Euler(0, -90, 0), transform);
        shape.SpawnAreaCollider(gameObject, this, _duration, origin);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (shape != null)
            shape.DrawGizmos();
    }
#endif
}
