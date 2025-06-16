using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BuildingPlacer : SceneOnlySingleton<BuildingPlacer>
{
    [SerializeField] private GridManager gridManager;
    public List<TowerSO> towers = new List<TowerSO>();
    private BuildingData buildingData;
    private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera topViewCam;

    private BuildingGhost buildingGhost;
    private TowerController selectedTower;
    private GameObject ghostObj;

    public bool        IsBuildingMode { get; private set; }
    public GridManager GridManager    => gridManager;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        mainCamera = Camera.main;
    }
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Cell")))
        {
            return hit.point;
        }

        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = 50f;

        return mainCamera.ScreenToWorldPoint(screenPosition);
    }

    public void HandleGhostTower(out (bool, Vector3Int) isCanBuilding)
    {
        var        mousePos = GetMouseWorldPosition();
        Vector3Int cell     = Vector3Int.FloorToInt(mousePos);
        isCanBuilding.Item1 = gridManager.CanPlaceBuilding(cell, buildingData.Size);
        isCanBuilding.Item2 = cell;
        buildingGhost.SetMaterialColor(isCanBuilding.Item1);
        buildingGhost.SetPosition(mousePos);
    }

    public void TryBuildingTower(TowerSO tower)
    {
        GameObject towerObj = ObjectPoolManager.Instance.GetObject(tower.name);
        if (!towerObj.TryGetComponent(out TowerController towerController))
            return;

        selectedTower = towerController;
        selectedTower.OnSpawnFromPool();
        buildingData = selectedTower.BuildingData;
        buildingGhost = buildingData.BuildingGhost;
        buildingGhost.SetValid(false);
        topViewCam.gameObject.SetActive(true);
    }

    public void CompleteBuildingTower(Vector3Int cell)
    {
        gridManager.PlaceBuilding(selectedTower.GameObject, cell, buildingData.Size);
        selectedTower.OnBuildComplete();
        buildingGhost.SetValid(true);
        selectedTower = null;
        buildingGhost = null;
    }

    private void OnGUI()
    {
        float buttonWidth  = 150f;
        float buttonHeight = 80f;
        float spacing      = 5f;

        float x = Screen.width - (buttonWidth + 10f);
        float y = Screen.height - buttonHeight - 50f;

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 0), buttonWidth, buttonHeight), "Build_Tower1"))
        {
            TryBuildingTower(towers[0]);
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 1), buttonWidth, buttonHeight), "Build_Tower2"))
        {
            TryBuildingTower(towers[1]);
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 2), buttonWidth, buttonHeight), "Build_Tower3"))
        {
            TryBuildingTower(towers[2]);

        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 3), buttonWidth, buttonHeight), "Build_Tower4"))
        {
            TryBuildingTower(towers[3]);
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 4), buttonWidth, buttonHeight), "Build_Tower4"))
        {
            TryBuildingTower(towers[4]);
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 5), buttonWidth, buttonHeight), "x2"))
        {
            Time.timeScale *= 2f;
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 6), buttonWidth, buttonHeight), "ResetGameSpeed"))
        {
            Time.timeScale = 1f;
        }

        if (GUI.Button(new Rect(10, y, buttonWidth, buttonHeight), "BuildingMode"))
        {
            IsBuildingMode = !IsBuildingMode;
            topViewCam.gameObject.SetActive(IsBuildingMode);
            UIManager.Instance.Close<UITowerUpgrade>();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}