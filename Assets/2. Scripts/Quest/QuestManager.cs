using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SceneOnlySingleton<QuestManager>
{
    [SerializeField] private List<QuestData> _questDatabase = new();

    private Dictionary<string, QuestData> _questLookup = new();
    private Dictionary<string, QuestProgress> _progressLookup = new();

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        LoadFromTable();
    }

    private void LoadFromTable()
    {
        QuestTable questTable = TableManager.Instance.GetTable<QuestTable>();
        foreach (var pair in questTable.DataDic)
        {
            _questLookup[pair.Key] = pair.Value;

            if (!_progressLookup.ContainsKey(pair.Key))
            {
                QuestProgress progress = new QuestProgress
                {
                    QuestId = pair.Key,
                    CurrentValue = 0,
                    IsCompleted = false,
                    RewardClaimed = false
                };
                _progressLookup[pair.Key] = progress;
            }
        }
    }

    public void UpdateProgress(QuestType type, int amount = 1)
    {
        foreach (var pair in _questLookup)
        {
            QuestData data = pair.Value;
            QuestProgress progress = _progressLookup[pair.Key];

            if (data.Condition.Type != type || progress.IsCompleted)
                continue;

            progress.CurrentValue += amount;

            if (progress.CurrentValue >= data.Condition.TargetValue)
            {
                progress.CurrentValue = data.Condition.TargetValue;
                progress.IsCompleted = true;
                Debug.Log($"퀘스트 완료: {data.Title}");
            }
        }
    }

    public void ClaimReward(string questId)
    {
        if (!_questLookup.ContainsKey(questId) || !_progressLookup.ContainsKey(questId))
            return;

        QuestProgress progress = _progressLookup[questId];
        if (!progress.IsCompleted || progress.RewardClaimed) return;

        progress.RewardClaimed = true;
        int reward = _questLookup[questId].RewardGold;

        Debug.Log($"보상 수령 완료: {reward} 골드 지급");
    }

    public IEnumerable<(QuestData Data, QuestProgress Progress)> GetAllProgress()
    {
        foreach (var pair in _questLookup)
        {
            yield return (pair.Value, _progressLookup[pair.Key]);
        }
    }
}