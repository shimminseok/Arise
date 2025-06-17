using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class StageInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text stageInfoText;
    [SerializeField] private IntegerEventChannelSO waveChangedEvent;

    private int currentStage = -1;
    private int currentWave = -1;

    private void OnEnable()
    {
        waveChangedEvent.RegisterListener(OnWaveChanged);
    }

    private void OnDisable()
    {
        waveChangedEvent.UnregisterListener(OnWaveChanged);
    }

    private void OnWaveChanged(int wave)
    {
        currentWave = wave;
        UpdateText();
    }
    private void Start()
    {
        TrySetStageFromSceneName();
        UpdateText();
    }

    private void TrySetStageFromSceneName()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        Match match = Regex.Match(sceneName, @"\d+");
        if (match.Success)
        {
            currentStage = int.Parse(match.Value);
        }
        else
        {
            Debug.LogWarning("씬 이름에서 스테이지 번호를 추출할 수 없습니다.");
            currentStage = 0;
        }
    }

    public void SetWave(int wave)
    {
        currentWave = wave;
        UpdateText();
    }

    private void UpdateText()
    {
        if (currentStage < 0) return;

        if (currentWave >= 0)
            stageInfoText.text = $"{currentStage} - {currentWave}";
        else
            stageInfoText.text = $"{currentStage}";
    }
}