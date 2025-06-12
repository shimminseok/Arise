using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ObjectPoolManager : SceneOnlySingleton<ObjectPoolManager>
{
    [SerializeField] private List<GameObject> poolObjectList = new List<GameObject>();
    private List<IPoolObject> pools = new List<IPoolObject>();
    private Dictionary<string, Queue<GameObject>> poolObjects = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> registeredObj = new Dictionary<string, GameObject>();
    private Dictionary<string, Transform> parentCache = new Dictionary<string, Transform>();

#if UNITY_EDITOR
    public void AutoAssignObject()
    {
        poolObjectList.Clear();
        string[] guids =
            UnityEditor.AssetDatabase.FindAssets("t:GameObject", new[] { "Assets/3. Prefabs/Pool" });

        foreach (string guid in guids)
        {
            string path  = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var    asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (asset.TryGetComponent<IPoolObject>(out var poolObject))
            {
                if (poolObject != null && !poolObjectList.Contains(asset))
                {
                    poolObjectList.Add(asset);
                }
            }
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
    protected override void Awake()
    {
        foreach (var obj in poolObjectList)
        {
            if (obj.TryGetComponent<IPoolObject>(out var ipool))
            {
                pools.Add(ipool);
            }
            else
            {
                Debug.LogError($"오브젝트에 IPoolObject이 상속 되어 있지 않습니다. {obj.name}");
            }
        }

        foreach (var pool in pools)
        {
            CreatePool(pool, pool.PoolSize);
        }
    }

    /// <summary>
    /// 풀에 오브잭트를 등록해주는 메서드
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="poolSize"></param>
    public void CreatePool(IPoolObject iPoolObject, int poolSize)
    {
        if (poolObjects.ContainsKey(iPoolObject.PoolID))
        {
            Debug.LogWarning($"등록된 풀이 있습니다. : {iPoolObject.PoolID}");
            return;
        }

        string     poolname   = iPoolObject.PoolID;
        GameObject poolObject = iPoolObject.GameObject;

        Queue<GameObject> newPool   = new Queue<GameObject>();
        GameObject        parentObj = new GameObject(poolname) { transform = { parent = transform } };
        parentCache[poolname] = parentObj.transform;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(poolObject, parentObj.transform);
            obj.name = poolname;
            obj.SetActive(false);
            newPool.Enqueue(obj);
        }

        poolObjects[poolname] = newPool;
        registeredObj[poolname] = poolObject;
    }

    /// <summary>
    /// 풀에서 오브젝트를 꺼내는 메서드
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetObject(string poolId)
    {
        string poolName = poolId.ToString();
        if (!poolObjects.TryGetValue(poolId, out Queue<GameObject> pool))
        {
            Debug.LogWarning($"등록된 풀이 없습니다. : {poolId}");
            return null;
        }

        if (pool.Count > 0)
        {
            GameObject go = pool.Dequeue();
            go.SetActive(true);
            return go;
        }
        else
        {
            GameObject prefab = registeredObj[poolId];
            GameObject newObj = Instantiate(prefab);
            newObj.name = poolName;
            newObj.transform.SetParent(parentCache[poolId]);
            newObj.SetActive(true);
            return newObj;
        }
    }

    /// <summary>
    /// 오브젝트를 풀에 반환하는 함수
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="returnTime"></param>
    /// <param name="action"></param>
    public void ReturnObject(GameObject obj, float returnTime = 0, UnityAction action = null)
    {
        StartCoroutine(DelayedReturnObject(obj, action, returnTime));
    }

    private IEnumerator DelayedReturnObject(GameObject obj, UnityAction action, float returnTime)
    {
        if (!obj.TryGetComponent<IPoolObject>(out IPoolObject iPoolObject))
        {
            Debug.LogError("풀링 오브젝트가 아닙니다.");
            yield break;
        }

        if (!poolObjects.ContainsKey(iPoolObject.PoolID))
        {
            Debug.LogWarning($"등록된 풀이 없습니다. : {iPoolObject.PoolID}");
            CreatePool(iPoolObject, 1);
        }

        if (returnTime > 0)
            yield return new WaitForSeconds(returnTime);
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;
        action?.Invoke();
        poolObjects[iPoolObject.PoolID].Enqueue(obj);
        obj.transform.SetParent(parentCache[iPoolObject.PoolID]);
    }


    public void RemovePool(string poolId)
    {
        Destroy(parentCache[poolId].gameObject);
        parentCache.Remove(poolId);
        poolObjects.Remove(poolId);
        registeredObj.Remove(poolId);
    }
}