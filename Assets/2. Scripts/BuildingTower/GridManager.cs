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
                cell.Initialize(pos);

                if (IsCellOverBuildableGround(pos))
                {
                    cell.IsBuildable = true;
                }
                else
                {
                    cell.IsBuildable = false;
                }

                _cells[pos] = cell;
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
                if (!_cells.TryGetValue(checkPos, out var cell) || !cell.CanBuild())
                    return false;
            }
        }

        return true;
    }

    public void PlaceBuilding(GameObject prefab, Vector3Int baseCellPos, Vector2Int size)
    {
        Vector3 worldPos = baseCellPos + new Vector3(size.x / 2f - 2f, cellHeightOffset, size.y / 2f - 2f);
        // GameObject go       = Instantiate(prefab, worldPos, Quaternion.identity);
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

    public bool IsCellOverBuildableGround(Vector3 cellCenter)
    {
        if (Physics.Raycast(cellCenter + Vector3.up * 5f, Vector3.down, out var hit, 10f, LayerMask.GetMask("Ground")))
        {
            return true;
        }

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