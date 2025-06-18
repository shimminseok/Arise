using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyManager : MonoBehaviour
{
    public static TutorialEnemyManager Instance { get; set; }

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform destinationPoint;

    public List<EnemyController> ActiveEnemies { get; private set; } = new();

    public System.Action OnAllEnemiesCleared;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnWave(MonsterSO monsterSO, int count)
    {
        StartCoroutine(SpawnWaveCoroutine(monsterSO, count));
    }

    private IEnumerator SpawnWaveCoroutine(MonsterSO monsterSO, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var enemyObj = ObjectPoolManager.Instance.GetObject(monsterSO.name);
            var enemy = enemyObj.GetComponent<EnemyController>();
            enemy.Initialized(spawnPoint.position, destinationPoint.position);

            ActiveEnemies.Add(enemy);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnEnemyKilled(EnemyController enemy)
    {
        if (ActiveEnemies.Contains(enemy))
            ActiveEnemies.Remove(enemy);

        TutorialManager.Instance.OnEnemyKilled();

        if (ActiveEnemies.Count == 0)
        {
            OnAllEnemiesCleared?.Invoke();
        }
    }
}