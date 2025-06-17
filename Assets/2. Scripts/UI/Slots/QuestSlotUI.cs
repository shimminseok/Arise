using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private Button receiveButton;

    private string _questId;

    public void SetData(QuestData data, QuestProgress progress)
    {
        _questId = data.QuestId;
        titleText.text = data.Title;
        progressSlider.maxValue = data.Condition.TargetValue;
        progressSlider.value = progress.CurrentValue;
        progressText.text = $"{progress.CurrentValue} / {data.Condition.TargetValue}";
        rewardText.text = $"{data.RewardGold}";

        receiveButton.interactable = progress.IsCompleted && !progress.RewardClaimed;
        receiveButton.onClick.RemoveAllListeners();
        receiveButton.onClick.AddListener(() =>
        {
            QuestManager.Instance.ClaimReward(_questId);
            receiveButton.interactable = false;
        });
    }
    
    public void Refresh(QuestProgress progress)
    {
        progressSlider.value = progress.CurrentValue;
        progressText.text = $"{progress.CurrentValue} / {progressSlider.maxValue}";
        receiveButton.interactable = progress.IsCompleted && !progress.RewardClaimed;
    }
}