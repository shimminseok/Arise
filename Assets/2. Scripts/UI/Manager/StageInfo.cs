using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using _2._Scripts.Events;

public class StageInfoUI : MonoBehaviour
{
    [Header("UI 텍스트")]
    [SerializeField] private TMP_Text stageInfoText;
    [SerializeField] private TMP_Text remainMonsterCountText;
    [SerializeField] private TMP_Text startWaveCountDown;

    [Header("이벤트 채널")]
    [SerializeField] private IntegerEventChannelSO waveChangedEvent;
    [SerializeField] private TwoIntegerEvent remainMonsterCountEvent;
    [SerializeField] private IntegerEventChannelSO startWaveCountDownEvent;

    [Header("튜토리얼 전용 끄기 대상")]
    [SerializeField] private GameObject waveTextObject;
    [SerializeField] private GameObject waveStartTimerObject;

    private int currentStage = -1;
    private int currentWave = -1;

    private void OnEnable()
    {
        waveChangedEvent.RegisterListener(OnWaveChanged);
        remainMonsterCountEvent.RegisterListener(UpdateRemainMonsterCount);
        startWaveCountDownEvent.RegisterListener(UpdateStartWaveCountDown);

        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private void OnDisable()
    {
        waveChangedEvent.UnregisterListener(OnWaveChanged);
        remainMonsterCountEvent.UnregisterListener(UpdateRemainMonsterCount);
        startWaveCountDownEvent.UnregisterListener(UpdateStartWaveCountDown);

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        TrySetStageFromSceneName();
        UpdateText();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TrySetStageFromSceneName();
        UpdateText();
        
        string currentSceneName = SceneManager.GetActiveScene().name;

        // if (selectedScene == "Tutorial")
        // {
        //     waveTextObject?.SetActive(false);
        //     waveStartTimerObject?.SetActive(false);
        // }
        // else
        // {
        //     waveTextObject?.SetActive(true);
        //     waveStartTimerObject?.SetActive(true);
        // }
    }

    private void OnWaveChanged(int wave)
    {
        currentWave = wave;
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
            currentStage = 0;
        }
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
        startWaveCountDown.gameObject.SetActive(false);
        startWaveCountDown.gameObject.SetActive(countDown > 0);
        startWaveCountDown.text = $"{countDown}";
    }
}
