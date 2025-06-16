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
                    if (tower.GetCurrentState() != TowerState.Build)
                        SelectTower(tower);
                }
                else
                {
                    UIManager.Instance.Close<UITowerUpgrade>();
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

        _selectedTower.UpgradeTower();
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