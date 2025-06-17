using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject questSlotPrefab;
    [SerializeField] private Transform contentParent;

    private Dictionary<string, QuestSlotUI> _slotDict = new();

    private IEnumerator Start()
    {
        while (QuestManager.Instance == null || !QuestManager.Instance.IsInitialized)
            yield return null;

        foreach (var (data, progress) in QuestManager.Instance.GetAllProgress())
        {
            GameObject go = Instantiate(questSlotPrefab, contentParent);
            QuestSlotUI slotUI = go.GetComponent<QuestSlotUI>();
            slotUI.SetData(data, progress);
            _slotDict[data.QuestId] = slotUI;
        }
    }

    public void RefreshAll()
    {
        foreach (var (data, progress) in QuestManager.Instance.GetAllProgress())
        {
            if (_slotDict.TryGetValue(data.QuestId, out var slot))
            {
                slot.Refresh(progress);
            }
        }
    }
}
