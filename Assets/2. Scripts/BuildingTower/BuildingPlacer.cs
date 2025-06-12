using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject buildingPrefab;
    private BuildingData buildingData;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        buildingData = buildingPrefab.GetComponent<BuildingData>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3    mouseWorld = GetMouseWorldPosition();
            Vector3Int cell       = Vector3Int.FloorToInt(mouseWorld);

            if (gridManager.CanPlaceBuilding(cell, buildingData.Size))
            {
                gridManager.PlaceBuilding(buildingPrefab, cell, buildingData.Size);
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out RaycastHit hit, Mathf.Infinity,LayerMask.GetMask("Cell")))
            return hit.point;

        return Vector3.zero;
    }
}
