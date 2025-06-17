using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : SceneOnlySingleton<EnemyManager>
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private IntegerEventChannelSO waveChangedEvent;
    [SerializeField] private VoidEventChannelSO onGameClearEvent;

    public List<EnemyController> Enemies { get; private set; } = new List<EnemyController>();

    private int _arrivalOrder = 0;
    private bool isSpawning = false;
    private bool isTutorialMode = false;

    [Header("웨이브 데이터")]
    [SerializeField] private StageWaveSO stageWaves;
    private int currentWaveIndex = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (isTutorialMode || IsTutorialScene())
            return;

        StartCoroutine(StartMonsterSpawn());
    }

    public void InitTutorialMode(Transform start, Transform end)
    {
        isTutorialMode = true;
        _startPoint = start;
        _endPoint = end;
        Enemies.Clear();
        ResetArrivalOrder();
    }

    public void SpawnTutorialMonster(MonsterSO monsterSO, int count = 1, float interval = 0.5f)
    {
        if (!isTutorialMode)
        {
            Debug.LogWarning("튜토리얼 모드 아님");
            return;
        }

        StartCoroutine(SpawnTutorialRoutine(monsterSO, count, interval));
    }

    private IEnumerator SpawnTutorialRoutine(MonsterSO monsterSO, int count, float interval)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnSingleMonster(monsterSO);
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator StartMonsterSpawn()
    {
        while (currentWaveIndex < stageWaves.waves.Count)
        {
            WaveSO wave = stageWaves.waves[currentWaveIndex];
            waveChangedEvent?.Raise(currentWaveIndex + 1);
            isSpawning = true;

            List<Coroutine> spawnCoroutines = new List<Coroutine>();
            foreach (var spawnInfo in wave.spawnList)
            {
                Coroutine c = StartCoroutine(SpawnMonsterTypeRoutine(spawnInfo.monster, spawnInfo.count));
                spawnCoroutines.Add(c);
            }

            foreach (var coroutine in spawnCoroutines)
            {
                yield return coroutine;
            }

            isSpawning = false;

            while (Enemies.Count > 0 || isSpawning)
            {
                yield return null;
            }

            Debug.Log($"웨이브 {currentWaveIndex + 1} 몬스터 전멸 확인.");
            yield return new WaitForSeconds(3f);

            currentWaveIndex++;
        }

        OnAllWavesComplete();
    }

    private IEnumerator SpawnMonsterTypeRoutine(MonsterSO monsterSo, int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnSingleMonster(monsterSo);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnSingleMonster(MonsterSO monsterSo)
    {
        GameObject monsterObj = ObjectPoolManager.Instance.GetObject(monsterSo.name);
        var monsterCtrl = monsterObj.GetComponent<EnemyController>();
        monsterCtrl.Initialized(_startPoint.position, _endPoint.position);
        Enemies.Add(monsterCtrl);
    }

    public void MonsterDead(EnemyController monster)
    {
        ObjectPoolManager.Instance.ReturnObject(monster.GameObject, 2f);
        Enemies.Remove(monster);
    }

    public int GetArrivalOrder() => _arrivalOrder++;

    public void ResetArrivalOrder() => _arrivalOrder = 0;

    private void OnAllWavesComplete()
    {
        Debug.Log("모든 웨이브가 완료되었습니다!");
        onGameClearEvent?.Raise();
    }

    public void StartWaveSpawn()
    {
        StartCoroutine(StartMonsterSpawn());
    }

    private bool IsTutorialScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainScene 1";
    }
}
