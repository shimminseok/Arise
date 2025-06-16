using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    MaxHp,
    CurHp,

    AttackPow,
    AttackSpd,
    AttackRange,

    MoveSpeed,
    Defense,

    MaxMp,
    CurMp,
    
    RunMultiplier,
    LookSensitivity,
}

public enum StatModifierType
{
    Base,
    BuffFlat,
    BuffPercent,
    Equipment,

    BasePercent
}