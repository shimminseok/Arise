using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour, IPoolObject
{
    [SerializeField] private string _poolId;
    [SerializeField] private int _poolSize;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private ProjectileTrigger _projectileTrigger;
    public GameObject GameObject => gameObject;
    public string     PoolID     => _poolId;
    public int        PoolSize   => _poolSize;

    private Coroutine _fireCoroutine;
    public IDamageable Target       { get; private set; }
    public IAttackable Attacker     { get; private set; }
    public float       SplashRadius { get; private set; }
    public TowerSO     OwnerTower   { get; private set; }

    public void Awake()
    {
        if (_projectileTrigger == null)
            _projectileTrigger = GetComponentInChildren<ProjectileTrigger>();
    }

    public void OnSpawnFromPool()
    {
        if (Target != null)
        {
            _fireCoroutine = StartCoroutine(FireProjectile());
            _projectileTrigger.SetTarget(this, SplashRadius);
        }
    }

    public void OnReturnToPool()
    {
        if (_fireCoroutine != null)
            StopCoroutine(_fireCoroutine);

        _fireCoroutine = null;
        Target = null;
    }

    public void SetTarget(TowerController attacker, IDamageable target)
    {
        Attacker = attacker;
        OwnerTower = attacker.TowerSO;
        SplashRadius = OwnerTower.SplashRadius;
        Target = target;
        OnSpawnFromPool();
    }

    private IEnumerator FireProjectile()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.Collider.transform.position, _projectileSpeed * Time.deltaTime);
            transform.LookAt(Target.Collider.transform);
            yield return null;

            if (Target == null || Target.IsDead)
            {
                ObjectPoolManager.Instance.ReturnObject(gameObject);
                yield break;
            }
        }
    }
}