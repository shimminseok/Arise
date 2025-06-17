using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public int StatusEffectID;
    public StatusEffectType EffectType;
    public StatType StatType;
    public StatModifierType ModifierType;
    public float Value;
    public float Duration;
    public float TickInterval = 1f;
    public Coroutine CoroutineRef;

    public bool IsStackable;

    public abstract IEnumerator Apply(StatusEffectManager manager);

    public virtual void OnEffectRemoved(StatusEffectManager effect)
    {
    }
}

//즉발 버프(패시브)
public class InstantBuff : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        manager.ModifyBuffStat(StatType, ModifierType, Value);
        yield return null;
        manager.RemoveEffect(this);

    }
}

//지속시간동안 증가 버프
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

        manager.RemoveEffect(this);
    }
}

// 즉발 디버프(패시브 디버프)
public class InstantDebuff : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        manager.ModifyBuffStat(StatType, ModifierType, -Value);
        yield return null;
        manager.RemoveEffect(this);
    }
    
}

//지속시간 디버프(지속시간동안 깍는 버프)
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

        manager.RemoveEffect(this);
    }
}

//기존 버프
public class TimedModifierBuff : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        // 스탯 증가
        manager.ModifyBuffStat(StatType, ModifierType, Value);

        yield return new WaitForSeconds(Duration);

        // 시간 지나면 원래대로 복구
        manager.RemoveEffect(this);
    }

    public override void OnEffectRemoved(StatusEffectManager manager)
    {
        manager.ModifyBuffStat(StatType, ModifierType, -Value);
    }
}

//즉시 회복
public class RecoverEffect : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        manager.RecoverEffect(StatType, ModifierType, Value);
        yield return null;
        manager.RemoveEffect(this);
    }
}
//지속 시간 회복
public class RecoverOverTime : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        float elapsed = 0f;
        while (elapsed < Duration)
        {
            manager.RecoverEffect(StatType, ModifierType, Value);
            yield return new WaitForSeconds(TickInterval);
            elapsed += TickInterval;
        }

        manager.RemoveEffect(this);
    }
}

//틱 데미지(독뎀, 화상뎀 등등)
public class PeriodicDamageDebuff : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        float elapsed = 0f;
        while (elapsed < Duration)
        {
            manager.ConsumeEffect(StatType, ModifierType, Value);
            yield return new WaitForSeconds(TickInterval);
            elapsed += TickInterval;
        }

        manager.RemoveEffect(this);
    }
}
// 데미지
public class DamageDebuff : StatusEffect
{
    public override IEnumerator Apply(StatusEffectManager manager)
    {
        manager.ConsumeEffect(StatType, ModifierType, Value);
        yield return null;
        manager.RemoveEffect(this);
    }
}

