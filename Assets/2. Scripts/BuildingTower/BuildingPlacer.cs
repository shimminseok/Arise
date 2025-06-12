using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public GridManager gridManager;
    private BuildingData buildingData;
    private Camera mainCamera;


    private bool selectedBuildData;
    private BuildingGhost buildingGhost;
    private GameObject selectedTowerPrefab;
    private GameObject ghostObj;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (selectedTowerPrefab == null)
            return;

        var mousePos = GetMouseWorldPosition();
        buildingGhost.SetPosition(mousePos);
        Vector3    mouseWorld    = mousePos;
        Vector3Int cell          = Vector3Int.FloorToInt(mouseWorld);
        bool       isCanBuilding = gridManager.CanPlaceBuilding(cell, buildingData.Size);
        buildingGhost.SetMaterialColor(isCanBuilding);
        if (Input.GetMouseButtonDown(0) && isCanBuilding)
        {
            gridManager.PlaceBuilding(selectedTowerPrefab, cell, buildingData.Size);
            selectedBuildData = false;
            buildingGhost.SetValid(true);
            selectedTowerPrefab = null;
            buildingGhost = null;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Cell")))
            return hit.point;

        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = 25f;

        return mainCamera.ScreenToWorldPoint(screenPosition);
    }


    private void OnGUI()
    {
        float buttonWidth  = 150f;
        float buttonHeight = 80f;
        float spacing      = 5f;

        float x = 10f;
        float y = Screen.height - buttonHeight - 50f;

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 0), buttonWidth, buttonHeight), $"Build_Tower1"))
        {
            selectedBuildData = true;
            selectedTowerPrefab = ObjectPoolManager.Instance.GetObject("ArcherTower_Lv1");
            TowerController controller = selectedTowerPrefab.GetComponent<TowerController>();
            controller.InitFromPool();
            buildingData = controller.BuildingData;
            buildingGhost = buildingData.BuildingGhost;
            buildingGhost.SetValid(false);
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 1), buttonWidth, buttonHeight), $"Build_Tower2"))
        {
            selectedBuildData = true;
            selectedTowerPrefab = ObjectPoolManager.Instance.GetObject("BallistaTower_LV1");
            TowerController controller = selectedTowerPrefab.GetComponent<TowerController>();
            controller.InitFromPool();
            buildingData = controller.BuildingData;
            buildingGhost = buildingData.BuildingGhost;
            buildingGhost.SetValid(false);
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 2), buttonWidth, buttonHeight), $"Build_Tower3"))
        {
            selectedBuildData = true;
            selectedTowerPrefab = ObjectPoolManager.Instance.GetObject("CanonTower_Lv1");
            TowerController controller = selectedTowerPrefab.GetComponent<TowerController>();
            controller.InitFromPool();
            buildingData = controller.BuildingData;
            buildingGhost = buildingData.BuildingGhost;
            buildingGhost.SetValid(false);
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 3), buttonWidth, buttonHeight), $"Build_Tower4"))
        {
            selectedBuildData = true;
            selectedTowerPrefab = ObjectPoolManager.Instance.GetObject("PoisonTower_Lv1");
            TowerController controller = selectedTowerPrefab.GetComponent<TowerController>();
            controller.InitFromPool();
            buildingData = controller.BuildingData;
            buildingGhost = buildingData.BuildingGhost;
            buildingGhost.SetValid(false);
        }
    }
}