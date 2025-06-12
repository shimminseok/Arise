using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public StatType StatType;
    public StatModifierType ModifierType;
    public float Value;
    public float Duration;
    public float TickInterval = 1f;
    public Coroutine CoroutineRef;

    public abstract IEnumerator Apply(StatusEffectManager manager);

    public virtual void OnEffectRemoved(StatusEffectManager effect)
    {
    }
}

//즉발 버프
public class InstantBuff : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        manager.ModifyBuffStat(StatType, ModifierType, Value);
        yield return null;
    }
}

//지속시간 버프
public class OverTimeBuff : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        float elapsed = 0f;
        while (elapsed < Duration)
        {
            manager.ModifyBuffStat(StatType, ModifierType, Value);
            yield return new WaitForSeconds(TickInterval);
            elapsed += TickInterval;
        }
    }
}

// 즉발 디버프
public class InstantDebuff : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        manager.ModifyBuffStat(StatType, ModifierType, -Value);
        yield return null;
    }
}

//지속시간 디버프
public class OverTimeDebuff : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        float elapsed = 0f;
        while (elapsed < Duration)
        {
            manager.ModifyBuffStat(StatType, ModifierType, -Value);
            yield return new WaitForSeconds(TickInterval);
            elapsed += TickInterval;
        }
    }
}

public class TimedModifierBuff : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        // 스탯 증가
        manager.ModifyBuffStat(StatType, ModifierType, Value);

        yield return new WaitForSeconds(Duration);

        // 시간 지나면 원래대로 복구
        manager.ModifyBuffStat(StatType, ModifierType, -Value);
    }

    public override void OnEffectRemoved(StatusEffectManager manager)
    {
        manager.ModifyBuffStat(StatType, ModifierType, -Value);
    }
}

public class RecoverEffect : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        manager.RecoverEffect(StatType, Value);
        yield return null;
    }
}

public class RecoverOverTime : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        float elapsed = 0f;
        while (elapsed < Duration)
        {
            manager.RecoverEffect(StatType, Value);
            yield return new WaitForSeconds(TickInterval);
            elapsed += TickInterval;
        }
    }
}

public class PeriodicDamageDebuff : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        float elapsed = 0f;
        while (elapsed < Duration)
        {
            manager.ConsumeEffect(StatType, Value);
            yield return new WaitForSeconds(TickInterval);
            elapsed += TickInterval;
        }
    }
}