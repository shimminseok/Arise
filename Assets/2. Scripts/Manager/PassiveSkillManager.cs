using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PassiveSkillManager : SceneOnlySingleton<PassiveSkillManager>
{
    [SerializeField] private StatusEffectManager _playerStatus;
    private List<PassiveSkillSO> _allPassives;
    private StatusEffectManager _weaponStatus;

    protected override void Awake()
    {
        base.Awake();
        var table = TableManager.Instance.GetTable<PassiveSkillTable>();
        if (table == null)
        {
            Debug.LogError("PassiveSkillTable이 등록되어 있지 않습니다!!!");
            return;
        }

        _allPassives = new List<PassiveSkillSO>(table.DataDic.Values);
    }

    private void Start()
    {
        // 이렇게 가져오는건 진짜 너무 구린데...
        _weaponStatus = _playerStatus.GetComponent<PlayerController>().WeaponController.GetComponent<StatusEffectManager>();
    }

    public List<PassiveSkillSO> GetThreeRandomChoices()
    {
        var chosen = new List<PassiveSkillSO>();
        var available = new List<PassiveSkillSO>(_allPassives);

        int count = Mathf.Min(3, available.Count);
        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, available.Count);
            chosen.Add(available[idx]);
            available.RemoveAt(idx);
        }

        return chosen;
    }

    public void ApplyPassive(PassiveSkillSO passive)
    {
        Debug.Log($"{passive.Effect.StatType}");
        switch (passive.Effect.StatType)
        {
            case PassiveStatType.GoldGain:
                GoldManager.Instance.AddGold(passive.Effect.Value);
                break;
            case PassiveStatType.MoveSpeed:
                var effect = BuffFactory.CreateBuff(passive.ID, passive.Effect.StatusEffectData);
                _playerStatus.ApplyEffect(effect);
                break;
            default:
                effect = BuffFactory.CreateBuff(passive.ID, passive.Effect.StatusEffectData);
                _weaponStatus.ApplyEffect(effect);
                break;
        }
    }
}
