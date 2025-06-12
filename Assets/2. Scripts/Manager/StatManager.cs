using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatManager : MonoBehaviour
{
    public Dictionary<StatType, StatBase> Stats { get; private set; } = new Dictionary<StatType, StatBase>();


    /// <summary>
    /// 플레이어의 스탯을 초기화 시켜주는 메서드
    /// </summary>
    /// <param name="player"></param>
    public void Initialize(PlayerSO player)
    {
        foreach (var stat in player.PlayerStats)
        {
            Stats[stat.StatType] = BaseStatFactory(stat.StatType, stat.Value);
        }
    }

    /// <summary>
    /// 몬스터의 스탯을 초기화 시켜주는 메서드
    /// </summary>
    /// <param name="monster"></param>
    public void Initialize(MonsterSO monster)
    {
        foreach (StatData monsterStat in monster.Stats)
        {
            Stats[monsterStat.StatType] = BaseStatFactory(monsterStat.StatType, monsterStat.Value);
        }
    }

    /// <summary>
    /// 타워의 스탯을 초기화 시켜주는 메서드
    /// </summary>
    /// <param name="tower"></param>
    public void Initialize(TowerSO tower)
    {
        foreach (StatData towerStat in tower.Stats)
        {
            Stats[towerStat.StatType] = BaseStatFactory(towerStat.StatType, towerStat.Value);
        }
    }

    /// <summary>
    /// Stat을 생성해주는 팩토리
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private StatBase BaseStatFactory(StatType type, float value)
    {
        return type switch
        {
            StatType.CurHp => new ResourceStat(type, value),
            StatType.CurMp => new ResourceStat(type, value),
            ///////////////////////////////////////////////////////////////////////////////////
            _ => new CalculatedStat(type, value),
        };
    }

    public T GetStat<T>(StatType type) where T : StatBase
    {
        return Stats[type] as T;
    }

    public float GetValue(StatType type)
    {
        return Stats[type].GetCurrent();
    }

    public void Recover(StatType statType, float value)
    {
        if (Stats[statType] is ResourceStat res)
        {
            if (res.CurrentValue < res.MaxValue)
            {
                res.Recover(value);
                Debug.Log($"Recover : {statType} : {value} RemainValue: {res.CurrentValue}");
            }
        }
    }

    public void Consume(StatType statType, float value)
    {
        if (Stats[statType] is ResourceStat res)
        {
            if (res.CurrentValue > 0)
            {
                res.Consume(value);
                Debug.Log($"Consume {statType} : {value}, RemainValue: {res.CurrentValue}");
            }
        }
    }

    /// <summary>
    /// 증가되는 스탯에 따라 해당 스탯을 증감시켜주는 메서드
    /// </summary>
    /// <param name="type"></param>
    /// <param name="valueType"></param>
    /// <param name="value"></param>
    public void ApplyStatEffect(StatType type, StatModifierType valueType, float value)
    {
        if (Stats[type] is not CalculatedStat stat) return;

        switch (valueType)
        {
            case StatModifierType.Base:        stat.ModifyBaseValue(value); break;
            case StatModifierType.BuffFlat:    stat.ModifyBuffFlat(value); break;
            case StatModifierType.BuffPercent: stat.ModifyBuffPercent(value); break;
            case StatModifierType.Equipment:   stat.ModifyEquipmentValue(value); break;
        }

        switch (type)
        {
            case StatType.MaxHp:
                SyncCurrentWithMax(StatType.CurHp, stat);
                break;
            case StatType.MaxMp:
                SyncCurrentWithMax(StatType.CurMp, stat);
                break;
        }

        Debug.Log($"Stat : {type} Modify Value {value}, FinalValue : {stat.Value}");
    }

    /// <summary>
    /// ResourceStat의 Max값을 동기화 시켜주는 메서드
    /// </summary>
    /// <param name="curStatType"></param>
    /// <param name="stat"></param>
    private void SyncCurrentWithMax(StatType curStatType, CalculatedStat stat)
    {
        if (Stats.TryGetValue(curStatType, out var res) && res is ResourceStat curStat)
        {
            curStat.SetMax(stat.FinalValue);
        }
    }
}