using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public int CurrentStage { get; private set; } = 1;
    public int CurrentWave { get; private set; } = -1;

    public int MaxStage { get; set; } = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetStage(int stage)
    {
        CurrentStage = stage;

        var questTimer = FindObjectOfType<TimeLimitQuestHelper>();
        if (questTimer != null)
            questTimer.StartTimer();
    }

    public void SetWave(int wave)
    {
        CurrentWave = wave;
    }
}