using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SceneOnlySingleton<EnemyManager>
{
    public List<EnemyController> Enemies { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    public void SpawnMonster(EnemyController monster)
    {
        Enemies.Add(monster);
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