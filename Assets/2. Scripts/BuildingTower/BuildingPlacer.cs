using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public GridManager gridManager;
    public List<TowerSO> towers = new List<TowerSO>();
    private BuildingData buildingData;
    private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera topViewCam;

    private bool selectedBuildData;
    private BuildingGhost buildingGhost;
    private TowerController selectedTower;
    private GameObject ghostObj;

    public bool BuildingMode { get; private set; }


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (selectedTower == null)
            return;

        var mousePos = GetMouseWorldPosition();
        buildingGhost.SetPosition(mousePos);
        Vector3    mouseWorld    = mousePos;
        Vector3Int cell          = Vector3Int.FloorToInt(mouseWorld);
        bool       isCanBuilding = gridManager.CanPlaceBuilding(cell, buildingData.Size);
        buildingGhost.SetMaterialColor(isCanBuilding);
        if (Input.GetMouseButtonDown(0) && isCanBuilding)
        {
            gridManager.PlaceBuilding(selectedTower.GameObject, cell, buildingData.Size);
            selectedTower.OnBuildComplete();
            selectedBuildData = false;
            buildingGhost.SetValid(true);
            selectedTower = null;
            buildingGhost = null;
            BuildingMode = false;
            topViewCam.gameObject.SetActive(BuildingMode);
        }
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


    private void OnGUI()
    {
        float buttonWidth  = 150f;
        float buttonHeight = 80f;
        float spacing      = 5f;

        float x = Screen.width - (buttonWidth + 10f);
        float y = Screen.height - buttonHeight - 50f;

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 0), buttonWidth, buttonHeight), $"Build_Tower1"))
        {
            selectedTower = ObjectPoolManager.Instance.GetObject(towers[0].name).GetComponent<TowerController>();
            selectedTower.OnSpawnFromPool();
            buildingData = selectedTower.BuildingData;
            buildingGhost = buildingData.BuildingGhost;
            buildingGhost.SetValid(false);
            topViewCam.gameObject.SetActive(true);
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 1), buttonWidth, buttonHeight), $"Build_Tower2"))
        {
            selectedBuildData = true;
            selectedTower = ObjectPoolManager.Instance.GetObject(towers[1].name).GetComponent<TowerController>();
            selectedTower.OnSpawnFromPool();
            buildingData = selectedTower.BuildingData;
            buildingGhost = buildingData.BuildingGhost;
            buildingGhost = buildingData.BuildingGhost;
            buildingGhost.SetValid(false);
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 2), buttonWidth, buttonHeight), $"Build_Tower3"))
        {
            selectedBuildData = true;
            selectedTower = ObjectPoolManager.Instance.GetObject(towers[2].name).GetComponent<TowerController>();
            selectedTower.OnSpawnFromPool();
            buildingData = selectedTower.BuildingData;
            buildingGhost = buildingData.BuildingGhost;
            buildingGhost = buildingData.BuildingGhost;
            buildingGhost.SetValid(false);
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 3), buttonWidth, buttonHeight), $"Build_Tower4"))
        {
            selectedBuildData = true;
            selectedTower = ObjectPoolManager.Instance.GetObject(towers[3].name).GetComponent<TowerController>();
            selectedTower.OnSpawnFromPool();
            buildingData = selectedTower.BuildingData;
            buildingGhost = buildingData.BuildingGhost;
            buildingGhost = buildingData.BuildingGhost;
            buildingGhost.SetValid(false);
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 4), buttonWidth, buttonHeight), $"x2"))
        {
            Time.timeScale *= 2f;
        }

        if (GUI.Button(new Rect(x, y - ((buttonHeight + spacing) * 5), buttonWidth, buttonHeight), $"ResetGameSpeed"))
        {
            Time.timeScale = 1f;
        }

        if (GUI.Button(new Rect(10, y, buttonWidth, buttonHeight), $"BuildingMode"))
        {
            BuildingMode = !BuildingMode;
            topViewCam.gameObject.SetActive(BuildingMode);
        }
        
    }
}