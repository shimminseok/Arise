using UnityEngine;


public class UITowerUpgrade : UIBase
{
    [SerializeField] private Vector3 offset;

    private TowerController _selectedTower;
    private Camera _mainCamera;


    private TowerTable _towerTable;

    private void Start()
    {
        _mainCamera = Camera.main;
        _towerTable = TableManager.Instance.GetTable<TowerTable>();
    }

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetMouseButton(0) && BuildingPlacer.Instance.IsBuildingMode)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Tower")))
            {
                if (hit.collider.TryGetComponent(out TowerController tower))
                {
                    SelectTower(tower);
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
    }

    public void SelectTower(TowerController selectedTower)
    {
        UIManager.Instance.Open<UITowerUpgrade>();
        _selectedTower = selectedTower;
        Contents.position = _mainCamera.WorldToScreenPoint(_selectedTower.transform.position);
        Contents.position += offset;
    }


    public void OnClickUpgradeTower()
    {
        if (_selectedTower == null)
            return;
        TowerSO nextLevelTower = _towerTable.GetDataByID(_selectedTower.TowerSO.ID + 1);
        if (nextLevelTower == null)
            return;
        GameObject tower = ObjectPoolManager.Instance.GetObject(nextLevelTower.name);
        if (!tower.TryGetComponent(out TowerController towerController))
        {
            ObjectPoolManager.Instance.ReturnObject(tower);
            return;
        }

        towerController = tower.GetComponent<TowerController>();
        towerController.OnSpawnFromPool();
        towerController.transform.position = _selectedTower.transform.position;
        towerController.OnBuildComplete();
        ObjectPoolManager.Instance.ReturnObject(_selectedTower.GameObject);
        _selectedTower = null;
        UIManager.Instance.Close<UITowerUpgrade>();
    }

    public void OnClickDestroyTower()
    {
        if (_selectedTower == null)
            return;

        ObjectPoolManager.Instance.ReturnObject(_selectedTower.GameObject);
        _selectedTower = null;
        UIManager.Instance.Close<UITowerUpgrade>();
    }
}