using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillManager : SceneOnlySingleton<PassiveSkillManager>
{
    [SerializeField] private StatusEffectManager _playerStatus;
    [SerializeField] private IntegerEventChannelSO _OnGainGold;
    private List<PassiveSkillSO> _allPassives;

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
                _OnGainGold.Raise(passive.Effect.Value);
                break;
            default:
                var effect = BuffFactory.CreateBuff(passive.ID, passive.Effect.StatusEffectData);
                _playerStatus.ApplyEffect(effect);
                break;
        }
    }
}
