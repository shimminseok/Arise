using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum PassiveStatType
{
    GoldGain,
    MoveSpeed,
    AttackPower,
    AttackSpeed,
    AttackRange,
}

[System.Serializable]
public class PassiveEffect
{
    public PassiveStatType StatType;
    public StatusEffectData StatusEffectData;
    public int Value;
}

[CreateAssetMenu(fileName = "PassiveSkillSO", menuName = "Scriptable Objects/Passive Skill/Passive Skill SO")]
public class PassiveSkillSO : ScriptableObject
{
    public int ID;
    public string SkillName;
    [TextArea] public string Description;
    public Sprite Icon;
    public PassiveEffect Effect;
}
