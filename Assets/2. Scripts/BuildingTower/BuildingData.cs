using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingGhost))]
public class BuildingData : MonoBehaviour, IPoolObject
{
    [SerializeField] private string poolId;
    [SerializeField] private int poolSize;
    public GameObject GameObject => gameObject;
    public string     PoolID     => poolId;
    public int        PoolSize   => poolSize;

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