using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SceneOnlySingleton<EnemyManager>
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    public List<EnemyController> Enemies { get; private set; } = new List<EnemyController>();

    private int _arrivalOrder = 0;

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
            monsterCtrl.Initialized(_startPoint.position, _endPoint.position);
            Enemies.Add(monsterCtrl);
        }
    }


    public IEnumerator StartMonsterSpawn()
    {
        int count = 1;
        while (count > 0)
            // while (true)
        {
            yield return new WaitForSeconds(2f);
            SpawnMonster();
            count--;
        }

        yield return null;
    }

    public void MonsterDead(EnemyController monster)
    {
        ObjectPoolManager.Instance.ReturnObject(monster.GameObject, 2f);
        Enemies.Remove(monster);
    }

    public int GetArrivalOrder()
    {
        return _arrivalOrder++;
    }

    public void ResetArrivalOrder()
    {
        _arrivalOrder = 0;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}