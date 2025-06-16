using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillSO : ScriptableObject
{
    [Header("ID")]
    public int ID;
    
    [Header("Skill Info")]
    public Sprite Icon;
    public string SkillName;
    [TextArea] public string Description;
    public float Cooldown;
    public float Duration;

    [Header("Skill Effects")]
    public List<StatusEffectData> StatusEffects = new List<StatusEffectData>();
    
    public abstract Skill CreateSkillInstance(GameObject owner);
}