using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AreaSkillSO", menuName = "Scriptable Objects/Skills/Area Skill SO")]
public class AreaSkillSO : SkillSO
{
    public enum Shape { Circle, Rect, Cone }
    
    [Header("Area Skill Info")]
    public Shape ShapeType;
    public float Radius;
    public float Angle;
    
    public Vector3 RectSize;
    
    public Transform Owner;
    public float Offset;

    public override Skill CreateSkillInstance(GameObject owner)
    {
        var skillObj = new GameObject($"AreaSkill_{SkillName}");
        var areaSkill = skillObj.AddComponent<AreaSkill>();
        
        Owner = owner.transform;
        areaSkill.Initialize(this);
        return areaSkill;
    }
}
