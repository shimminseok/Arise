using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : AreaSkill
{
    private GameObject _prefab;
    private float _speed;
    private float _range;

    public void Initialize(ProjectileSkillSO data)
    {
        base.Initialize(data);
        _prefab = data.SkillPrefab;
        _speed = data.Speed;
        _range = data.Range;
        shape = new RectShape(data.RectSize, data.Owner.transform.rotation, data.Offset);
    }
    
    protected override void Apply(Transform origin)
    {
        transform.position = origin.position;
        shape.SpawnAreaCollider(gameObject, this, _duration, origin);
        var projectile = Instantiate(_prefab, origin.position + origin.forward * _offset, origin.rotation, transform);
        var mover = gameObject.AddComponent<ProjectileMover>();

        mover.Initialize(_speed, _range, projectile.transform.forward);
    }
}
