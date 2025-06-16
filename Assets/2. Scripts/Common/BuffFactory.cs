public static class BuffFactory
{
    public static StatusEffect CreateBuff(int id, StatusEffectData data)
    {
        StatusEffect effect = data.EffectType switch
        {
            StatusEffectType.InstantBuff          => new InstantBuff(),
            StatusEffectType.OverTimeBuff         => new OverTimeBuff(),
            StatusEffectType.InstantDebuff        => new InstantDebuff(),
            StatusEffectType.OverTimeDebuff       => new OverTimeDebuff(),
            StatusEffectType.TimedModifierBuff    => new TimedModifierBuff(),
            StatusEffectType.Recover              => new RecoverEffect(),
            StatusEffectType.RecoverOverTime      => new RecoverOverTime(),
            StatusEffectType.PeriodicDamageDebuff => new PeriodicDamageDebuff(),
            StatusEffectType.Damege               => new DamageDebuff(),

            _ => null
        };
        if (effect == null)
            return null;
        effect.StatusEffectID = id;
        effect.EffectType = data.EffectType;
        effect.StatType = data.Stat.StatType;
        effect.Duration = data.Duration;
        effect.ModifierType = data.Stat.ModifierType;
        effect.Value = data.Stat.Value;
        effect.TickInterval = data.TickInterval;
        effect.IsStackable = data.IsStackable;
        return effect;
    }
}