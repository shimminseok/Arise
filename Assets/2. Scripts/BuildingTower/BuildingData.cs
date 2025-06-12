using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingGhost))]
public class BuildingData : MonoBehaviour, IPoolObject
{
    public GameObject GameObject => gameObject;
    public string     PoolID     => "ArrowTower";
    public int        PoolSize   => 10;

    public BuildingGhost BuildingGhost { get; private set; }
    public Vector2Int Size;


    private void Awake()
    {
        BuildingGhost = GetComponent<BuildingGhost>();
    }


    public void InitFromPool()
    {
        BuildingGhost.SetValid(true);
    }
}