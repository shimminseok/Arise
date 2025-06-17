using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private GameObject turretModeButton;
    [SerializeField] private Transform tutorialStartPoint;
    [SerializeField] private Transform tutorialEndPoint;
    [SerializeField] private MonsterSO tutorialMonsterSO;
    [SerializeField] private GameObject questPanelObject;

    private TutorialPhase currentPhase;
    private float phaseTimer = 0f;
    private bool isPhaseWaiting = false;
    private int killCount = 0;
    private bool questCompleted = false;
    private bool questPanelOpened = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 버튼 자동 연결
        if (turretModeButton == null)
            turretModeButton = UIManager.Instance?.TurretModeButton;
        if (turretModeButton == null)
            Debug.LogWarning("[TutorialManager] turretModeButton 연결 안됨");

        // 퀘스트 패널 자동 연결
        if (questPanelObject == null)
            questPanelObject = UIManager.Instance?.QuestPanelObject;
        if (questPanelObject == null)
            Debug.LogWarning("[TutorialManager] questPanelObject 연결 안됨");

        // 시점 문제 방지: 시작 시 타임스케일은 1, 이후 단계에서 0
        Time.timeScale = 1f;

        // 튜토리얼 스폰 위치 초기화
        EnemyManager.Instance.InitTutorialMode(tutorialStartPoint, tutorialEndPoint);

        // 튜토리얼 시작
        StartPhase(TutorialPhase.WaitForMove);
    }

    private void Update()
    {
        switch (currentPhase)
        {
            case TutorialPhase.WaitForMove:
                if (!isPhaseWaiting &&
                    (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                     Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
                     Input.GetKeyDown(KeyCode.LeftShift)))
                {
                    StartCoroutine(WaitAndAdvanceToNextPhase(3f));
                }
                break;

            case TutorialPhase.WaitForTower:
                if (TurretInstallTracker.Instance.HasInstalledTurret)
                {
                    CompletePhase();
                }
                break;

            case TutorialPhase.WaitForFirstWave:
                phaseTimer += Time.unscaledDeltaTime;
                if (phaseTimer >= 3f)
                {
                    Time.timeScale = 1f;
                    EnemyManager.Instance.SpawnTutorialMonster(tutorialMonsterSO, 3);
                    CompletePhase();
                }
                break;

            case TutorialPhase.AutoWaveProgression:
                if (EnemyManager.Instance.Enemies.Count == 0 && !isPhaseWaiting)
                {
                    StartCoroutine(DelayNextWave(5f));
                }
                break;

            case TutorialPhase.WaitForQuest:
                if (!questCompleted && killCount >= 10)
                {
                    questCompleted = true;
                    StartCoroutine(HandleQuestCompleteSequence());
                }

                if (questCompleted && !questPanelOpened && questPanelObject.activeSelf)
                {
                    questPanelOpened = true;
                    CompletePhase();
                }
                break;
        }
    }

    private void StartPhase(TutorialPhase phase)
    {
        currentPhase = phase;

        switch (phase)
        {
            case TutorialPhase.WaitForMove:
                tutorialText.text = "WASD 또는 Shift 키를 눌러 움직이세요!";
                Time.timeScale = 0f;
                break;

            case TutorialPhase.WaitForTower:
                tutorialText.text = "터렛 설치 버튼을 눌러 설치해보세요!";
                Time.timeScale = 0f;
                if (turretModeButton != null)
                    turretModeButton.SetActive(true);
                break;

            case TutorialPhase.WaitForFirstWave:
                tutorialText.text = "곧 몬스터가 등장합니다...";
                Time.timeScale = 0f;
                break;

            case TutorialPhase.AutoWaveProgression:
                tutorialText.text = "";
                Time.timeScale = 1f;
                break;

            case TutorialPhase.WaitForQuest:
                tutorialText.text = "몬스터 10마리를 처치해보세요!";
                break;
        }
    }

    private void CompletePhase()
    {
        currentPhase++;
        phaseTimer = 0;
        isPhaseWaiting = false;
        StartPhase(currentPhase);
    }

    private IEnumerator DelayNextWave(float delay)
    {
        isPhaseWaiting = true;
        yield return new WaitForSeconds(delay);
        EnemyManager.Instance.StartWaveSpawn();
    }

    private IEnumerator WaitAndAdvanceToNextPhase(float delay)
    {
        isPhaseWaiting = true;
        tutorialText.text = "잘 하셨어요! 잠시 후 다음 단계로 넘어갑니다...";
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(delay);
        CompletePhase();
    }

    private IEnumerator HandleQuestCompleteSequence()
    {
        tutorialText.text = "퀘스트가 완료되었습니다!";
        yield return new WaitForSecondsRealtime(2f);
        questPanelObject.SetActive(true);
    }

    public void OnEnemyKilled()
    {
        killCount++;
        if (currentPhase == TutorialPhase.AutoWaveProgression && killCount >= 10)
        {
            CompletePhase();
        }
    }
}
