using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    MaxHp,
    CurHp,

    AttackPow,
    AttackSpd,

    MoveSpeed,
    Defense,

    MaxMp,
    CurMp,
}

public enum StatModifierType
{
    Base,
    BuffFlat,
    BuffPercent,
    Equipment
}