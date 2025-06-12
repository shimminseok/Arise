using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseTable<TKey, TValue> : ScriptableObject, ITable where TKey : notnull where TValue : ScriptableObject
{
    [SerializeField] protected List<TValue> dataList = new List<TValue>();

    public Dictionary<TKey, TValue> DataDic { get; private set; } = new Dictionary<TKey, TValue>();

    public             Type     Type     { get; protected set; }
    protected abstract string[] DataPath { get; }

    public abstract void CreateTable();

    public TValue GetDataByID(TKey id)
    {
        return DataDic.GetValueOrDefault(id);
    }

#if UNITY_EDITOR
    public void AutoAssignDatas()
    {
        dataList.Clear();

        string[] guids =
            UnityEditor.AssetDatabase.FindAssets($"t:{typeof(TValue)}", DataPath);

        foreach (string guid in guids)
        {
            string path  = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var    asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TValue>(path);

            if (asset != null && !dataList.Contains(asset))
            {
                dataList.Add(asset);
            }
        }

        Debug.Log($"[{typeof(TValue)}] 데이터 등록 완료!");
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}