using EnemyStates;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class HPBarUI : MonoBehaviour, IPoolObject
{
    [FormerlySerializedAs("_poolId")]
    [SerializeField] private string poolId;

    [FormerlySerializedAs("_poolSize")]
    [SerializeField] private int poolSize;

    [FormerlySerializedAs("_barRect")]
    [SerializeField] RectTransform barRect;

    [FormerlySerializedAs("_fillImage")]
    [SerializeField] Image fillImage;

    [FormerlySerializedAs("_offset")]
    [SerializeField] Vector3 offset;

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolId;
    public int        PoolSize   => poolSize;

    private IDamageable _target;
    private Transform _targetTransform;
    private Camera _mainCamera;
    private float heightOffset;

    StatManager _statManager;
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void Initialize(IDamageable owner)
    {
        _target = owner;
        OnSpawnFromPool();
        _statManager = _target.Collider.GetComponent<StatManager>();
    }

    public void UpdatePosion()
    {
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(_targetTransform.position + offset);
        barRect.position = screenPos;
    }

    public void UpdateHealthBarWrapper(float cur)
    {
        UpdateFill(cur, _statManager.GetValue(StatType.MaxHp));
    }

    /// <summary>
    /// FillAmount를 업데이트 시켜주는 메서드
    /// </summary>
    /// <param name="cur">현재 값</param>
    /// <param name="max">맥스 값</param>
    public void UpdateFill(float cur, float max)
    {
        fillImage.fillAmount = Mathf.Clamp01(cur / max);
    }

    public void UnLink()
    {
        HealthBarManager.Instance.DespawnHealthBar(this);
    }

    public void OnSpawnFromPool()
    {
        offset = Vector3.up;
        _targetTransform = _target.Collider.transform;
        heightOffset = _target.Collider.bounds.size.y;
        offset.y += heightOffset;
        transform.SetParent(HealthBarManager.Instance.hpBarCanvas.transform);
            
    }

    public void OnReturnToPool()
    {
        _target = null;
        fillImage.fillAmount = 1f;
        barRect.position = Vector3.zero;
    }
}