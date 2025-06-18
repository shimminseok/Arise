using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Vector3Int GridSize;
    public GameObject cellPrefab;
    public float cellHeightOffset;
    private readonly Dictionary<Vector3Int, GridCell> _cells = new Dictionary<Vector3Int, GridCell>();


    private void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int z = 0; z < GridSize.z; z++)
            {
                Vector3Int pos    = new Vector3Int(x, 0, z);
                GameObject cellGo = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                var        cell   = cellGo.GetComponent<GridCell>();
                cell.IsBuildable = IsCellOverBuildableGround(pos, out Vector3Int hitPos);
                cellGo.transform.localPosition = hitPos;
                cell.Initialize(hitPos);

                _cells[hitPos] = cell;
            }
        }
    }

    public GridCell GetCell(Vector3 pos)
    {
        Vector3Int gridPos = Vector3Int.FloorToInt(pos);
        _cells.TryGetValue(gridPos, out GridCell cell);
        return cell;
    }

    public bool CanPlaceBuilding(Vector3Int pos, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector3Int checkPos = pos + new Vector3Int(x, 0, z);
                if (!_cells.TryGetValue(checkPos, out GridCell cell) || !cell.CanBuild())
                    return false;
            }
        }

        return true;
    }

    public void PlaceBuilding(GameObject prefab, Vector3Int baseCellPos, Vector2Int size)
    {
        Vector3 worldPos = baseCellPos + new Vector3(size.x / 2f - 0.5f, cellHeightOffset, size.y / 2f - 0.5f);
        prefab.transform.position = worldPos;
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector3Int pos = baseCellPos + new Vector3Int(x, 0, z);
                _cells[pos].OccupiedObject = prefab;
            }
        }
    }

    public void PlaceDestroying(BuildingData data)
    {
        for (int x = 0; x < data.Size.x; x++)
        {
            for (int z = 0; z < data.Size.y; z++)
            {
                Vector3Int pos = Vector3Int.FloorToInt(data.PlaceBaseCell + new Vector3Int(x, 0, z));
                if (_cells.TryGetValue(pos, out GridCell cell))
                    cell.OccupiedObject = null;
            }
        }
    }

    public bool IsCellOverBuildableGround(Vector3 cellCenter, out Vector3Int hitPos)
    {
        if (Physics.Raycast(cellCenter + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 15f, LayerMask.GetMask("Ground")))
        {
            hitPos = Vector3Int.FloorToInt(hit.point);
            return true;
        }


        hitPos = Vector3Int.FloorToInt(cellCenter);
        return false;
    }


    private void OnDrawGizmosSelected()
    {
        foreach (KeyValuePair<Vector3Int, GridCell> cells in _cells)
        {
            Gizmos.color = cells.Value.IsBuildable ? Color.green : Color.red;
            Gizmos.DrawWireCube(cells.Key, Vector3.one);
        }
    }
}