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

    private TutorialPhase _currentPhase;
    private float _phaseTimer = 0f;
    private bool _isPhaseWaiting = false;
    private bool _isTransitioningPhase = false;

    private int _killCount = 0;
    private bool _zPressed = false;
    private bool _xPressed = false;
    private bool _cPressed = false;

    private bool _hasStoppedForQuest = false;
    private bool _hasResumedAfterQuest = false;
    private bool _hasShownFinalClearMessage = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (turretModeButton == null)
            turretModeButton = UIManager.Instance?.TurretModeButton;
        if (turretModeButton == null)
            Debug.LogWarning("[TutorialManager] turretModeButton 연결 안됨");

        if (questPanelObject == null)
            questPanelObject = UIManager.Instance?.QuestPanelObject;
        if (questPanelObject == null)
            Debug.LogWarning("[TutorialManager] questPanelObject 연결 안됨");

        Time.timeScale = 1f;
        EnemyManager.Instance.InitTutorialMode(tutorialStartPoint, tutorialEndPoint);

        StartCoroutine(PlayIntroSequence());
    }

    private void Update()
    {
        switch (_currentPhase)
        {
            case TutorialPhase.WaitForMove:
                if (!_isPhaseWaiting &&
                    (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                     Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
                     Input.GetKeyDown(KeyCode.LeftShift)))
                {
                    CompletePhase();
                }
                break;

            case TutorialPhase.WaitForTower:
                if (TurretInstallTracker.Instance.HasInstalledTurret)
                {
                    CompletePhase();
                }
                break;

            case TutorialPhase.WaitForFirstWave:
                if (!_isPhaseWaiting)
                {
                    _isPhaseWaiting = true;
                    StartCoroutine(HandleFirstWaveSpawn());
                }
                break;

            case TutorialPhase.AutoWaveProgression:
                break;

            case TutorialPhase.WaitForSkillKeys:
                if (Input.GetKeyDown(KeyCode.Z)) _zPressed = true;
                if (Input.GetKeyDown(KeyCode.X)) _xPressed = true;
                if (Input.GetKeyDown(KeyCode.C)) _cPressed = true;

                if (_zPressed && _xPressed && _cPressed && !_isPhaseWaiting)
                {
                    StartCoroutine(ShowNextPhaseMessage());
                }
                else
                {
                    tutorialText.text = "Z, X, C 키를 눌러 스킬을 사용해보세요!";
                }
                break;

            case TutorialPhase.FinalMessage:
                if (!_isPhaseWaiting)
                    StartCoroutine(ShowFinalMessageThenNext());
                break;

            case TutorialPhase.StartWaveAfterTutorial:
                tutorialText.text = "";

                if (tutorialText != null)
                {
                    var parent = tutorialText.transform.parent;
                    if (parent != null)
                        parent.gameObject.SetActive(false);
                }

                TutorialEnemyManager.Instance = null;
                EnemyManager.Instance.StartWaveSpawn();
                break;
        }
    }

    private void StartPhase(TutorialPhase phase)
    {
        _currentPhase = phase;

        switch (phase)
        {
            case TutorialPhase.WaitForMove:
                tutorialText.text = "WASD 키를 눌러 움직이세요! (Shift는 대쉬입니다.)";
                Time.timeScale = 0f;
                break;

            case TutorialPhase.WaitForTower:
                tutorialText.text = "터렛 설치 버튼을 눌러 설치해보세요!";
                Time.timeScale = 0f;
                break;

            case TutorialPhase.WaitForFirstWave:
                Time.timeScale = 0f;
                break;

            case TutorialPhase.AutoWaveProgression:
                tutorialText.text = "몬스터를 모두 처치하세요!";
                Time.timeScale = 1f;
                break;

            case TutorialPhase.WaitForSkillKeys:
                tutorialText.text = "Z, X, C 키를 눌러 스킬을 사용해보세요!";
                break;

            case TutorialPhase.FinalMessage:
                break;
        }
    }

    private void CompletePhase()
    {
        if (_isTransitioningPhase) return;
        _isTransitioningPhase = true;

        if (_currentPhase == TutorialPhase.WaitForFirstWave || _currentPhase == TutorialPhase.FinalMessage)
        {
            _currentPhase++;
            _isTransitioningPhase = false;
            StartPhase(_currentPhase);
            return;
        }

        StartCoroutine(ShowNextPhaseMessage());
    }

    private IEnumerator ShowNextPhaseMessage()
    {
        _isPhaseWaiting = true;
        tutorialText.text = "잘 하셨어요! 잠시 후 다음 단계로 넘어갑니다.";
        Time.timeScale = 1f;

        yield return new WaitForSecondsRealtime(2f);

        _currentPhase++;
        _phaseTimer = 0;
        _isPhaseWaiting = false;
        _isTransitioningPhase = false;
        StartPhase(_currentPhase);
    }

    private IEnumerator HandleFirstWaveSpawn()
    {
        tutorialText.text = "곧 몬스터가 등장합니다.";
        Time.timeScale = 1f;

        yield return new WaitForSecondsRealtime(2f);

        EnemyManager.Instance.SpawnTutorialMonster(tutorialMonsterSO, 20);

        _isPhaseWaiting = false;
        CompletePhase();
    }

    private IEnumerator ShowFinalMessageThenNext()
    {
        _isPhaseWaiting = true;
        tutorialText.text = "튜토리얼 완료! 이제 게임이 시작됩니다!";
        yield return new WaitForSecondsRealtime(3f);
        _isPhaseWaiting = false;
        CompletePhase();
    }

    private IEnumerator HandleQuestCompleteSequence()
    {
        tutorialText.text = "퀘스트가 완료되었습니다!";
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        questPanelObject.SetActive(true);
    }

    public void OnQuestPanelConfirmed()
    {
        Time.timeScale = 1f;
        questPanelObject.SetActive(false);
        _hasResumedAfterQuest = true;
        StartCoroutine(ShowMessageThenWait("잘 하셨어요! 성에 몬스터가 접근하지 못하도록 모두 잡아주세요!", 2f));
    }

    private IEnumerator ShowMessageThenWait(string message, float duration)
    {
        tutorialText.text = message;
        yield return new WaitForSecondsRealtime(duration);
        tutorialText.text = "몬스터를 모두 처치하세요!";
    }

    private IEnumerator HandleAllMonsterCleared()
    {
        tutorialText.text = "모두 처치하셨습니다!";
        yield return new WaitForSecondsRealtime(2f);
        CompletePhase();
    }
    
    private IEnumerator PlayIntroSequence()
    {
        tutorialText.gameObject.SetActive(true);

        Time.timeScale = 0f;

        tutorialText.text = "안녕하세요! 우리 게임에 오신걸 환영해요!";
        yield return new WaitForSecondsRealtime(2f);

        tutorialText.text = "간단한 조작과 게임 흐름을 알려드리도록 하겠습니다.";
        yield return new WaitForSecondsRealtime(2f);

        StartPhase(TutorialPhase.WaitForMove);
    }
    public void OnEnemyKilled()
    {
        _killCount++;

        if (!_hasStoppedForQuest && _killCount >= 10)
        {
            _hasStoppedForQuest = true;
            StartCoroutine(HandleQuestCompleteSequence());
            return;
        }

        if (_killCount >= 20 && _hasResumedAfterQuest && !_hasShownFinalClearMessage)
        {
            _hasShownFinalClearMessage = true;
            StartCoroutine(HandleAllMonsterCleared());
        }
    }
}
