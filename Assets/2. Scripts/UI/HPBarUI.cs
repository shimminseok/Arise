using EnemyStates;
using UnityEngine;
using UnityEngine.UI;


public class HPBarUI : MonoBehaviour, IPoolObject
{
    [SerializeField] private string poolId;
    [SerializeField] private int poolSize;

    [SerializeField] RectTransform barRect;
    [SerializeField] Image fillImage;
    [SerializeField] Vector3 offset;

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolId;
    public int        PoolSize   => poolSize;

    private BaseController<EnemyController, EnemyState> target;
    private Transform targetTrans;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void Initialize(BaseController<EnemyController, EnemyState> owner)
    {
        target = owner;
        OnSpawnFromPool();
    }

    public void UpdatePosion()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetTrans.position + offset);
        barRect.position = screenPos;
    }

    public void UpdateHealthBarWrapper(float cur)
    {
        UpdateFill(cur, target.StatManager.GetValue(StatType.MaxHp));
    }

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
        targetTrans = target.transform;
        transform.SetParent(HealthBarManager.Instance.hpBarCanvas.transform);
    }

    public void OnReturnToPool()
    {
        target = null;
        fillImage.fillAmount = 1f;
        barRect.position = Vector3.zero;
    }
}