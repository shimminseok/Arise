using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _2._Scripts.Events;
using UnityEngine;
using System.Linq;

public class EnemyManager : SceneOnlySingleton<EnemyManager>
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private IntegerEventChannelSO waveChangedEvent;
    [SerializeField] private VoidEventChannelSO onGameClearEvent;
    [SerializeField] private TwoIntegerEvent waveRemainMonsterCountEvent;
    [SerializeField] private IntegerEventChannelSO waveCountDownEvent;
    [SerializeField] private VoidEventChannelSO onPassiveSkillPanelEvent;
    public List<MonsterSO> Enemies { get; private set; } = new List<MonsterSO>();

    private int _arrivalOrder = 0;
    private bool isSpawning = false;
    private bool isTutorialMode = false;
    private bool isWaveStarted = false; // 중복 실행 방지

    [Header("웨이브 데이터")]
    [SerializeField] private StageWaveSO stageWaves;
    private int currentWaveIndex = 0;
    private int waveTotalMonsterCount = 0;
    private int waveCurrentMonsterCount = 0;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (IsTutorialScene()) return;
        StartWaveSpawn();
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
        if (!isTutorialMode) return;
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

            isSpawning = true; // 스폰 시작
            waveTotalMonsterCount = 0;
            List<Coroutine> spawnCoroutines = new List<Coroutine>();
            foreach (var spawnInfo in wave.spawnList)
            {
                waveTotalMonsterCount += spawnInfo.count;
                Coroutine c = StartCoroutine(SpawnMonsterTypeRoutine(spawnInfo.monster, spawnInfo.count));
                spawnCoroutines.Add(c);
            }

            waveCurrentMonsterCount = waveTotalMonsterCount;
            waveRemainMonsterCountEvent?.Raise(waveCurrentMonsterCount, waveTotalMonsterCount);

            foreach (var coroutine in spawnCoroutines)
            {
                yield return coroutine;
            }

            isSpawning = false;

            while (Enemies.Count > 0 || isSpawning)
            {
                yield return null;
            }

            if (!isTutorialMode)
            {
                yield return new WaitForSeconds(1f);
                onPassiveSkillPanelEvent.Raise();
            }
            
            StartCoroutine(StartWaveCountDown());
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

        if (monsterSo is BossSO boss)
        {
            var bossCtrl = monsterObj.GetComponent<BossController>();
            bossCtrl.Initialized(_startPoint.position, _endPoint.position);
            if (!isTutorialMode)
                Enemies.Add(boss);
        }
        else
        {
            var monsterCtrl = monsterObj.GetComponent<EnemyController>();
            monsterCtrl.Initialized(_startPoint.position, _endPoint.position);
            if (!isTutorialMode)
                Enemies.Add(monsterSo);
        }

    }

    public void MonsterDead(EnemyController monster)
    {
        GameObject gold = ObjectPoolManager.Instance.GetObject("Gold");
        gold.transform.localPosition = monster.transform.position + Vector3.up;
        gold.GetComponent<GoldRooting>().Initialized(10 + currentWaveIndex);
        ObjectPoolManager.Instance.ReturnObject(monster.GameObject, 2f);
        Enemies.Remove(monster.MonsterSo);
        waveRemainMonsterCountEvent?.Raise(--waveCurrentMonsterCount, waveTotalMonsterCount);
    }

    public void MonsterDead(BossController boss)
    {
        ObjectPoolManager.Instance.ReturnObject(boss.GameObject, 2f);
        Enemies.Remove(boss.BossSo);
        waveRemainMonsterCountEvent?.Raise(--waveCurrentMonsterCount, waveTotalMonsterCount);
    }

    public int GetArrivalOrder() => _arrivalOrder++;
    public void ResetArrivalOrder() => _arrivalOrder = 0;

    private void OnAllWavesComplete()
    {
        Debug.Log("모든 웨이브가 완료되었습니다!");
        onGameClearEvent?.Raise();
    }

    private IEnumerator StartWaveCountDown()
    {
        int count = 3;
        while (count > 0)
        {
            yield return new WaitForSeconds(0.1f);
            waveCountDownEvent?.Raise(count);
            yield return new WaitForSeconds(1f);
            count--;
        }

        waveCountDownEvent?.Raise(count);
    }

    protected override void OnDestroy()
    {
        
    }

    public void StartWaveSpawn()
    {
        if (isWaveStarted) return; // 중복 실행 방지
        isWaveStarted = true;

        Debug.Log("[EnemyManager] StartWaveSpawn() 실행됨");
        StartCoroutine(StartMonsterSpawn());
    }

    private bool IsTutorialScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial";
    }
}
