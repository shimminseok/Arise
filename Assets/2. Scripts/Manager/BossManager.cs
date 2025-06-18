using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : SceneOnlySingleton<BossManager>
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    public List<BossController> Enemies { get; private set; } = new List<BossController>();

    private int _arrivalOrder = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // SpawnBoss();
        // StartCoroutine(StartBossSpawn());
    }

    public void SpawnBoss(BossController Boss = null)
    {
        foreach (BossSO bossSo in TableManager.Instance.GetTable<BossTable>().DataDic.Values)
        {
            GameObject bossObj  = ObjectPoolManager.Instance.GetObject(bossSo.name);
            var        bossCtrl = bossObj.GetComponent<BossController>();
            bossCtrl.Initialized(_startPoint.position, _endPoint.position);
            Enemies.Add(bossCtrl);
        }
    }


    public IEnumerator StartBossSpawn()
    {
        int count = 1;
        // while (count > 0)
        while (count > 0)
        {
            yield return new WaitForSeconds(1f);
            SpawnBoss();
            count--;
        }

        yield return null;
    }

    public void BossDead(BossController boss)
    {
        ObjectPoolManager.Instance.ReturnObject(boss.GameObject);
        Enemies.Remove(boss);
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