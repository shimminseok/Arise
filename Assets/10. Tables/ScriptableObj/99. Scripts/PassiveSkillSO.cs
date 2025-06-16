using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public PassiveStatType statType;
    public float value;
}

[CreateAssetMenu(fileName = "PassiveSkillSO", menuName = "Scriptable Objects/Passive Skill/Passive Skill SO")]
public class PassiveSkillSO : ScriptableObject
{
    public int ID;
    public string SkillName;
    [TextArea] public string Description;
    public PassiveEffect Effect;
}
