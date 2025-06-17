using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTickType
{
    Instant,
    OverTime
}

public enum EffectCategory
{
    Buff,
    Debuff
}

public class StatusEffectManager : MonoBehaviour
{
    private List<StatusEffect> activeEffects = new List<StatusEffect>();
    private StatManager statManager;

    private void Start()
    {
        statManager = GetComponent<StatManager>();
    }

    public void ApplyEffect(StatusEffect effect)
    {

        if (!effect.IsStackable)
        {
            var existing = activeEffects.Find(x =>
                x.EffectType == effect.EffectType &&
                x.StatType == effect.StatType &&
                x.ModifierType == effect.ModifierType);
            if (existing != null)
            {
                if (Mathf.Abs(effect.Value) >= Mathf.Abs(existing.Value))
                {
                    RemoveEffect(existing);
                }
                else
                {
                    return;
                }
            }
        }
        Coroutine co = StartCoroutine(effect.Apply(this));
        effect.CoroutineRef = co;
        activeEffects.Add(effect);
    }

    public void ModifyBuffStat(StatType statType, StatModifierType modifierType, float value)
    {
        switch (modifierType)
        {
            case StatModifierType.BuffFlat:
                statManager.ApplyStatEffect(statType, StatModifierType.BuffFlat, value);
                break;
            case StatModifierType.BuffPercent:
                statManager.ApplyStatEffect(statType, StatModifierType.BuffPercent, value);
                break;
        }
    }

    public void RecoverEffect(StatType statType, StatModifierType modifierType, float value)
    {
        statManager.Recover(statType, modifierType, value);
    }

    public void ConsumeEffect(StatType statType, StatModifierType modifierType, float value)
    {
        statManager.Consume(statType, modifierType, value);
    }

    public void RemoveEffect(StatusEffect effect)
    {
        activeEffects.Remove(effect);
        if (effect.CoroutineRef != null)
        {
            StopCoroutine(effect.CoroutineRef);
        }

        effect.OnEffectRemoved(this);
    }
    public void RemoveAllEffects()
    {
        foreach (StatusEffect effect in activeEffects)
        {
            if (effect.CoroutineRef != null)
            {
                StopCoroutine(effect.CoroutineRef);
            }

            effect.OnEffectRemoved(this);
        }

        activeEffects.Clear();
    }
}