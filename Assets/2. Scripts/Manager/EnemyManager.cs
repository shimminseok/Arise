using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SceneOnlySingleton<EnemyManager>
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    public List<EnemyController> Enemies { get; private set; } = new List<EnemyController>();

    public Vector3 Endpoint => _endPoint.position;
    public Vector3 StartPoint => _startPoint.position;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // SpawnMonster();
        StartCoroutine(StartMonsterSpawn());
    }

    public void SpawnMonster(EnemyController monster = null)
    {
        foreach (MonsterSO monsterSo in TableManager.Instance.GetTable<MonsterTable>().DataDic.Values)
        {
            GameObject monsterObj  = ObjectPoolManager.Instance.GetObject(monsterSo.name);
            var        monsterCtrl = monsterObj.GetComponent<EnemyController>();
            Enemies.Add(monsterCtrl);
        }
    }


    public IEnumerator StartMonsterSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SpawnMonster();
        }

        yield return null;
    }

    public void MonsterDead(EnemyController monster)
    {
        ObjectPoolManager.Instance.ReturnObject(monster.GameObject);
        Enemies.Remove(monster);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}