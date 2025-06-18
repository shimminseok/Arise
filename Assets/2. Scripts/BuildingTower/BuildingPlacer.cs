using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BuildingPlacer : SceneOnlySingleton<BuildingPlacer>
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private CinemachineVirtualCamera topViewCam;
    [SerializeField] private LayerMask cellLayerMask;
    private BuildingData _buildingData;
    private Camera _mainCamera;

    private BuildingGhost _buildingGhost;
    private TowerController _selectedTower;
    private GameObject _ghostObj;

    public bool        IsBuildingMode { get; private set; }
    public GridManager GridManager    => gridManager;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        _mainCamera = Camera.main;
    }
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, cellLayerMask))
        {
            return hit.point;
        }

        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = 50f;

        return _mainCamera.ScreenToWorldPoint(screenPosition);
    }

    public (bool canBuild, Vector3Int cell) GetGhostBuildInfo()
    {
        Vector3    mousePos   = GetMouseWorldPosition();
        Vector3Int centerCell = Vector3Int.FloorToInt(mousePos);
        Vector3 baseCellF = centerCell + new Vector3(
            -_buildingData.Size.x / 2f + 0.5f,
            0,
            -_buildingData.Size.y / 2f + 0.5f
        );
        // 중심 셀 → 좌하단 셀로 보정
        Vector3Int baseCell = Vector3Int.FloorToInt(baseCellF);

        bool canBuild = gridManager.CanPlaceBuilding(baseCell, _buildingData.Size);

        _buildingGhost.SetMaterialColor(canBuild);
        _buildingGhost.SetPosition(mousePos);

        return (canBuild, baseCell);
    }

    public void TryBuildingTower(TowerSO tower)
    {
        if (_selectedTower != null)
            RefundOrClearSelectedTower();
        GameObject towerObj = ObjectPoolManager.Instance.GetObject(tower.name);
        if (!towerObj.TryGetComponent(out TowerController towerController))
            return;

        _selectedTower = towerController;
        _selectedTower.OnSpawnFromPool();
        _buildingData = _selectedTower.BuildingData;
        _buildingGhost = _buildingData.BuildingGhost;
        _buildingGhost.SetValid(false);
    }

    private void RefundOrClearSelectedTower()
    {
        ObjectPoolManager.Instance.ReturnObject(_selectedTower.GameObject);
        _selectedTower = null;
        _buildingGhost = null;
    }
    public void CompleteBuildingTower(Vector3Int cell)
    {
        int cost = _selectedTower.TowerSO.BuildCost;

        if (GoldManager.Instance.CurrentGold < cost)
        {
            Debug.Log("골드 부족으로 타워 설치를 할 수 없음.");
            RefundOrClearSelectedTower();
            return;
        }

        bool success = GoldManager.Instance.TrySpendGold(cost);
        if (!success)
        {
            return;
        }

        // 골드 차감 성공 후에만 설치
        gridManager.PlaceBuilding(_selectedTower.GameObject, cell, _buildingData.Size);
        _selectedTower.BuildingData.PlaceBaseCell = cell;
        _selectedTower.OnBuildComplete();
        
        if (TurretInstallTracker.Instance != null)
            TurretInstallTracker.Instance.OnTurretInstalled();

        QuestManager.Instance.UpdateProgress(QuestType.BuildTower, 1);


        _buildingGhost.SetValid(true);

        // 설치 완료 후 변수 초기화
        _selectedTower = null;
        _buildingGhost = null;
    }

    // private void OnGUI()
    // {
    //     float buttonWidth  = 150f;
    //     float buttonHeight = 80f;
    //     float spacing      = 5f;
    //
    //     float x = Screen.width - (buttonWidth + 10f);
    //     float y = Screen.height - buttonHeight - 50f;
    //
    //     if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 0), buttonWidth, buttonHeight), "Build_Tower1"))
    //     {
    //         TryBuildingTower(towers[0]);
    //     }
    //
    //     if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 1), buttonWidth, buttonHeight), "Build_Tower2"))
    //     {
    //         TryBuildingTower(towers[1]);
    //     }
    //
    //     if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 2), buttonWidth, buttonHeight), "Build_Tower3"))
    //     {
    //         TryBuildingTower(towers[2]);
    //
    //     }
    //
    //     if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 3), buttonWidth, buttonHeight), "Build_Tower4"))
    //     {
    //         TryBuildingTower(towers[3]);
    //     }
    //
    //     if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 4), buttonWidth, buttonHeight), "Build_Tower4"))
    //     {
    //         TryBuildingTower(towers[4]);
    //     }
    //
    //     if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 5), buttonWidth, buttonHeight), "x2"))
    //     {
    //         Time.timeScale *= 2f;
    //     }
    //
    //     if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 6), buttonWidth, buttonHeight), "ResetGameSpeed"))
    //     {
    //         Time.timeScale = 1f;
    //     }
    // }

    public void ChangeBuildMode(bool isBuilding)
    {
        IsBuildingMode = isBuilding;
        topViewCam.gameObject.SetActive(IsBuildingMode);
        UIManager.Instance.Close<UITowerUpgrade>();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}