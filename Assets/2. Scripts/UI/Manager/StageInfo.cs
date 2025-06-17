using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using _2._Scripts.Events;

public class StageInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text stageInfoText;
    [SerializeField] private IntegerEventChannelSO waveChangedEvent;
    [SerializeField] private TMP_Text remainMonsterCountText;
    [SerializeField] private TMP_Text startWaveCountDown;
    [SerializeField] private TwoIntegerEvent remainMonsterCountEvent;
    [SerializeField] private IntegerEventChannelSO startWaveCountDownEvent;
    
    
    private int currentStage = -1;
    private int currentWave = -1;

    private void OnEnable()
    {
        waveChangedEvent.RegisterListener(OnWaveChanged);
        remainMonsterCountEvent.RegisterListener(UpdateRemainMonsterCount);
        startWaveCountDownEvent.RegisterListener(UpdateStartWaveCountDown);
    }

    private void OnDisable()
    {
        waveChangedEvent.UnregisterListener(OnWaveChanged);
        remainMonsterCountEvent.UnregisterListener(UpdateRemainMonsterCount);
        startWaveCountDownEvent.UnregisterListener(UpdateStartWaveCountDown);
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


    private void UpdateRemainMonsterCount(int cur, int max)
    {
        remainMonsterCountText.text = $"{cur} / {max}";
    }

    private void UpdateStartWaveCountDown(int countDown)
    {
        Debug.Log(countDown);
        startWaveCountDown.gameObject.SetActive(countDown > 0);
        startWaveCountDown.text = $"{countDown}";
    }
}