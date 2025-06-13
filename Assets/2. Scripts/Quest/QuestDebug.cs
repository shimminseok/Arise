using UnityEngine;

public class QuestDebug : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (var quest in QuestManager.Instance.GetAllProgress())
            {
                Debug.Log($"{quest.Data.Title} : {quest.Progress.CurrentValue}/{quest.Data.Condition.TargetValue}");
            }
        }
    }
}