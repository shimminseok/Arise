using System;
using UnityEngine;


public enum StatusEffectType
{
    InstantBuff,
    OverTimeBuff,
    InstantDebuff,
    OverTimeDebuff,
    TimedModifierBuff,
    PeriodicDamageDebuff,
    Recover,
    RecoverOverTime,
    Damege,
}


[Serializable]
public class StatusEffectData
{
    public StatusEffectType EffectType;
    public StatData Stat;
    public float Duration;
    public float TickInterval;
}