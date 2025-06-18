using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingGhost))]
public class BuildingData : MonoBehaviour
{
    public BuildingGhost BuildingGhost { get; private set; }
    public Vector2Int Size;
    public Vector3Int PlaceBaseCell;


    private void Awake()
    {
        BuildingGhost = GetComponent<BuildingGhost>();
    }
}