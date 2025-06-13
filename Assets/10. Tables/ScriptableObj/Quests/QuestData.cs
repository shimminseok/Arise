using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/QuestData")]
public class QuestData : ScriptableObject
{
    public string QuestId;
    public string Title;
    public string Description;

    public QuestCondition Condition;
    public int RewardGold;
}