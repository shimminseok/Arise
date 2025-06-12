using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject
{
    public GameObject GameObject { get; }
    public string     PoolID     { get; }
    public int        PoolSize   { get; }

    public void InitFromPool();
}