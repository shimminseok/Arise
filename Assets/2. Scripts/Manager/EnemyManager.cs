using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SceneOnlySingleton<EnemyManager>
{
    public List<EnemyController> Enemies { get; private set; } = new List<EnemyController>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SpawnMonster();
    }

    public void SpawnMonster(EnemyController monster = null)
    {
        //Test
        foreach (MonsterSO monsterSo in TableManager.Instance.GetTable<MonsterTable>().DataDic.Values)
        {
            GameObject monsterObj  = ObjectPoolManager.Instance.GetObject(monsterSo.name);
            var        monsterCtrl = monsterObj.GetComponent<EnemyController>();
            Enemies.Add(monsterCtrl);
            monsterCtrl.SpawnMonster(monsterSo, Vector3.zero);
        }
    }

    public void MonsterDead(EnemyController monster)
    {
        ObjectPoolManager.Instance.ReturnObject(monster.gameObject);
        Enemies.Remove(monster);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}