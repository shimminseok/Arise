using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    private Dictionary<string, QuestData> _questLookup = new();
    private Dictionary<string, QuestProgress> _progressLookup = new();

    protected override void Awake()
    {
        base.Awake();
        // 아무 것도 하지 않음. 초기화는 Start에서.
    }

    public bool IsInitialized { get; private set; } = false;
    private IEnumerator Start()
    {
        //  1프레임 딜레이 → TableManager 등록을 기다림
        yield return null;

        // TableManager에서 안전하게 QuestTable 요청
        var questTable = TableManager.Instance.GetTable<QuestTable>();
        Debug.Log("QuestTable 불러오기 성공");

        // 등록된 퀘스트들 초기화
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

        Debug.Log($" 총 퀘스트 수: {_questLookup.Count}");
        IsInitialized = true;
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
        
        var panel = FindObjectOfType<QuestPanelUI>();
        if (panel != null)
        {
            panel.RefreshAll();
        }
    }

    public void ClaimReward(string questId)
    {
        if (!_questLookup.ContainsKey(questId) || !_progressLookup.ContainsKey(questId))
            return;

        var progress = _progressLookup[questId];
        if (!progress.IsCompleted || progress.RewardClaimed) return;

        progress.RewardClaimed = true;
        var data = _questLookup[questId];
        
        GoldManager.Instance.AddGold(data.RewardGold);
        
        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnQuestPanelConfirmed();
        }
    }

    public IEnumerable<(QuestData Data, QuestProgress Progress)> GetAllProgress()
    {
        foreach (var pair in _questLookup)
        {
            yield return (pair.Value, _progressLookup[pair.Key]);
        }
    }

    public void ApplyLoadedProgress(List<QuestProgress> loadedList)
    {
        foreach (var progress in loadedList)
        {
            if (_progressLookup.ContainsKey(progress.QuestId))
            {
                _progressLookup[progress.QuestId] = progress;
            }
        }

        // 퀘스트 패널 갱신
        var panel = FindObjectOfType<QuestPanelUI>();
        if (panel != null)
        {
            panel.RefreshAll();
        }

        Debug.Log("퀘스트 진행도 불러오기 완료");
    }
}