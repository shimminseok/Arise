using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridCell : MonoBehaviour
{
    public Vector3Int CellPosition { get; private set; }
    public bool IsBuildable = true;
    public GameObject OccupiedObject;

    public void Initialize(Vector3Int pos)
    {
        CellPosition = pos;
        // IsBuildable = true;
        OccupiedObject = null;
    }

    public bool CanBuild()
    {
        return IsBuildable && OccupiedObject == null;
    }
}
