using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITowerUpgrade : UIBase
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private GameObject upgradeIcon;
    [SerializeField] private EventTrigger eventTrigger;
    private TowerController _selectedTower;
    private Camera _mainCamera;
    private TowerTable _towerTable;

    private void Start()
    {
        _mainCamera = Camera.main;
        _towerTable = TableManager.Instance.GetTable<TowerTable>();


        EventTrigger.Entry clickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };

        clickEntry.callback.AddListener((eventData) => { UIManager.Instance.Close<UITowerUpgrade>(); });

        eventTrigger.triggers.Add(clickEntry);
    }

    private void Update()
    {
        HandleSelectionInput();
    }

    private void HandleSelectionInput()
    {
        
        if (Input.GetMouseButton(0) && BuildingPlacer.Instance.IsBuildingMode)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Tower")))
            {
                if (hit.collider.TryGetComponent(out TowerController tower))
                {
                    if (tower.GetCurrentState() != TowerState.Build)
                    {
                        SelectTower(tower);
                    }
                }
            }
        }
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
        _selectedTower = null;
        UpdateUpgradeCostText();
    }

    public void SelectTower(TowerController selectedTower)
    {
        UIManager.Instance.Open<UITowerUpgrade>();
        _selectedTower = selectedTower;
        Contents.position = _mainCamera.WorldToScreenPoint(_selectedTower.transform.position);
        Contents.position += offset;

        UpdateUpgradeCostText();
    }

    private void UpdateUpgradeCostText()
    {
        if (_selectedTower == null)
        {
            upgradeCostText.text = "";
            upgradeIcon.SetActive(false);
            return;
        }

        var nextTowerData = _towerTable.GetDataByID(_selectedTower.TowerSO.ID + 1);

        if (nextTowerData == null)
        {
            // 최대 레벨일 때
            upgradeCostText.text = "MAX";
            upgradeIcon.SetActive(false);
            upgradeCostText.alignment = TextAlignmentOptions.Center;
            return;
        }

        // 최대 레벨이 아닐 때는 항상 다음 단계 골드와 아이콘 표시
        upgradeCostText.text = nextTowerData.BuildCost.ToString();
        upgradeIcon.SetActive(true);
        upgradeCostText.alignment = TextAlignmentOptions.Right; // 아이콘 옆에 있으니 오른쪽 정렬 권장
    }

    public void OnClickUpgradeTower()
    {
        if (_selectedTower == null)
            return;

        var nextTowerData = _towerTable.GetDataByID(_selectedTower.TowerSO.ID + 1);
        if (nextTowerData == null)
        {
            Debug.Log("최대 레벨입니다.");
            return;
        }

        int upgradeCost = nextTowerData.BuildCost;

        if (GoldManager.Instance.CurrentGold < upgradeCost)
        {
            Debug.Log("골드가 부족합니다. 업그레이드를 할 수 없습니다.");
            return;
        }

        bool success = GoldManager.Instance.TrySpendGold(upgradeCost);
        if (!success)
        {
            Debug.Log("골드 차감 실패");
            return;
        }

        _selectedTower = _selectedTower.UpgradeTower();
        UpdateUpgradeCostText();
    }

    public void OnClickDestroyTower()
    {
        if (_selectedTower == null)
            return;

        _selectedTower.DestroyTower();
        _selectedTower = null;
        UIManager.Instance.Close<UITowerUpgrade>();

    }
}
