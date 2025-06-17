using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSkillSO", menuName = "Scriptable Objects/Skills/Projectile Skill SO")]
public class ProjectileSkillSO : AreaSkillSO
{
    [Header("Projectile Info")]
    public float Speed;
    public float Range;
    
    public override Skill CreateSkillInstance(GameObject owner)
    {
        Debug.Log("스킬 생성");
        var skillObj = new GameObject($"ProjectileSkill_{SkillName}");
        var projSkill = skillObj.AddComponent<ProjectileSkill>();
        
        Owner = owner.transform;
        projSkill.Initialize(this);
        return projSkill;
    }
}
