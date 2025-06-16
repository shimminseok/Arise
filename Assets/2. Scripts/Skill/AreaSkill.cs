using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSkill : Skill
{
    protected IAreaShape shape;

    public void Initialize(AreaSkillSO data)
    {
        base.Initialize(data);

        switch (data.ShapeType)
        {
            case AreaSkillSO.Shape.Circle:
                shape = new CircleShape(data.Radius);
                break;
            case AreaSkillSO.Shape.Rect:
                shape = new RectShape(data.RectSize, data.Owner.transform.rotation);
                break;
        }
    }
    
    protected override void Apply(Transform origin)
    {
        transform.position = origin.position;
        shape.SpawnAreaCollider(gameObject, this, _duration);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (shape != null)
            shape.DrawGizmos(transform);
    }
#endif
}
