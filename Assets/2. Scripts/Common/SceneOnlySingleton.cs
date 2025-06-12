using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOnlySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType(typeof(T)) as T;
                if (instance == null)
                {
                    SetupInstance();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private static void SetupInstance()
    {
        GameObject gameObj = new GameObject();
        gameObj.name = typeof(T).Name;
        instance = gameObj.AddComponent<T>();
    }
}