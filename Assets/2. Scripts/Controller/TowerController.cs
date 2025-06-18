using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerStates;
using UnityEngine.Serialization;

public enum TowerState
{
    Build,
    Idle,
    Attack
}

[RequireComponent(typeof(BuildingData))]
[RequireComponent(typeof(CapsuleCollider))]
public class TowerController : BaseController<TowerController, TowerState>, IPoolObject, IAttackable
{
    [SerializeField] private string poolId;
    [SerializeField] private int poolSize;
    [SerializeField] private TowerSO towerSO;
    [SerializeField] private SFX fireSound;

    [FormerlySerializedAs("ProjectilePoolId")]
    [SerializeField] private string projectilePoolId;
    [SerializeField] private Transform fireTransform;

    [FormerlySerializedAs("fireWeaponTransform")]
    [SerializeField] private Transform fireWeaponWeaponTransform;

    public StatBase     AttackStat          { get; private set; }
    public IDamageable  Target              { get; private set; }
    public BuildingData BuildingData        { get; private set; }
    public bool         IsPlaced            { get; private set; }
    public Collider[]   TargetResults       { get; private set; }
    public Animator     Animator            { get; private set; }

    public Transform    FireWeaponTransform => fireWeaponWeaponTransform;
    public Transform    FireTransform       => fireTransform;
    public GameObject   GameObject          => gameObject;
    public string       PoolID              => towerSO.name;
    public int          PoolSize            => poolSize;
    public TowerSO      TowerSO             => towerSO;
    public string       ProjectilePoolId    => projectilePoolId;
    public int UpgradeLevel { get; set; } = 1;



    private Collider m_Collider;
    private TowerTable towerTable;
    protected override void Awake()
    {
        base.Awake();
        BuildingData = GetComponent<BuildingData>();
        m_Collider = GetComponent<CapsuleCollider>();
        towerTable = TableManager.Instance.GetTable<TowerTable>();
        Animator = GetComponentInChildren<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        AttackStat = StatManager.GetStat<CalculatedStat>(StatType.AttackPow);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override IState<TowerController, TowerState> GetState(TowerState state)
    {
        return state switch
        {
            TowerState.Idle   => new IdleState(),
            TowerState.Attack => new AttackState(StatManager.GetValue(StatType.AttackSpd), StatManager.GetValue(StatType.AttackRange)),
            TowerState.Build  => new BuildState(),
            _                 => null
        };
    }

    public TowerState GetCurrentState()
    {
        return CurrentState;
    }
    public float GetTargetDistance()
    {
        if (Target != null && !Target.IsDead)
        {
            float distance = Utility.GetSqrDistanceBetween(m_Collider, Target.Collider);
            return distance;
        }

        return Mathf.Infinity;
    }

    public bool IsTargetInAttackRange()
    {
        float attackRange    = StatManager.GetValue(StatType.AttackRange);
        float sqrAttackRange = attackRange * attackRange;
        return GetTargetDistance() <= sqrAttackRange;
    }

    public override void FindTarget()
    {
        TargetResults = new Collider[towerSO.ProjectileCount];
        var size = Physics.OverlapSphereNonAlloc(transform.position, StatManager.GetValue(StatType.AttackRange), TargetResults, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < size; i++)
        {
            if (TargetResults[i].TryGetComponent<IDamageable>(out var damageable) && !damageable.IsDead)
            {
                Target = damageable;
                break;
            }
        }
    }


    public void OnSpawnFromPool()
    {
        IsPlaced = false;
        StatManager.Initialize(towerSO);
    }

    public void OnReturnToPool()
    {
        Target = null;
        StatusEffectManager.RemoveAllEffects();
        BuildingPlacer.Instance.GridManager.PlaceDestroying(BuildingData);
        ChangeState(TowerState.Build);
    }

    public void OnBuildComplete()
    {
        IsPlaced = true;
        BuildingManager.Instance?.RegisterTower(this);
    }

    public TowerController UpgradeTower()
    {
        var nextTowerData = towerTable.GetDataByID(TowerSO.ID + 1);

        if (nextTowerData == null)
        {
            Debug.Log("최대 레벨입니다.");
            return null;
        }

        int upgradeCost = nextTowerData.BuildCost;

        if (GoldManager.Instance.CurrentGold < upgradeCost)
        {
            Debug.Log("골드가 부족하여 업그레이드를 할 수 없습니다.");
            return null;
        }

        bool success = GoldManager.Instance.TrySpendGold(upgradeCost);

        if (!success)
        {
            Debug.Log("골드 차감 실패");
            return null;
        }

        GameObject upgradedTower = ObjectPoolManager.Instance.GetObject(nextTowerData.name);
        if (!upgradedTower.TryGetComponent(out TowerController nextTower))
        {
            ObjectPoolManager.Instance.ReturnObject(upgradedTower);
            return null;
        }

        nextTower.transform.position = transform.position;
        nextTower.OnSpawnFromPool();
        nextTower.OnBuildComplete();

        ObjectPoolManager.Instance.ReturnObject(gameObject);

        return nextTower;
    }

    public void DestroyTower()
    {
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }

    public void Attack()
    {
        FindTarget();
        TowerSO.AttackType.Attack(this);
        SoundManager.Instance.PlaySFX(fireSound);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
    }

    public BuildingSaveData GetSaveData()
    {
        return new BuildingSaveData
        {
            TowerId = towerSO.name,
            Position = transform.position,
            UpgradeLevel = towerSO.ID
        };
    }
}