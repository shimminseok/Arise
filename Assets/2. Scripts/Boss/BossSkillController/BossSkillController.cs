using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillController : MonoBehaviour, IPoolObject
{
    [SerializeField] private string _poolId;
    [SerializeField] private int _poolSize;
    [SerializeField] private float _bossSkillSpeed;
    [SerializeField] private BossSkillTrigger _bossSkillTrigger;
    public GameObject GameObject => gameObject;
    public string     PoolID     => _poolId;
    public int        PoolSize   => _poolSize;

    private Coroutine _fireCoroutine;
    public IDamageable Target   { get; private set; }
    public IAttackable Attacker { get; private set; }

    public void Awake()
    {
        if (_bossSkillTrigger == null)
            _bossSkillTrigger = GetComponentInChildren<BossSkillTrigger>();
    }

    public void OnSpawnFromPool()
    {
        if (Target != null)
        {
            _fireCoroutine = StartCoroutine(FireBossSkill());
      //      _bossSkillTrigger.SetTarget(this);
        }
    }

    public void OnReturnToPool()
    {
        if (_fireCoroutine != null)
            StopCoroutine(_fireCoroutine);

        _fireCoroutine = null;
        Target = null;
    }

    public void SetTarget(IAttackable attacker, IDamageable target)
    {
        Attacker = attacker;
        Target = target;
        OnSpawnFromPool();
    }

    private IEnumerator FireBossSkill()
    {
        while (true)
        {
            yield return null;

            if (Target == null || Target.IsDead)
            {
                ObjectPoolManager.Instance.ReturnObject(gameObject);
                yield break;
            }
        }
    }
}