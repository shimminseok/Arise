using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
    [SerializeField] private List<ScriptableObject> tableList = new List<ScriptableObject>();

    private Dictionary<Type, ITable> tableDic = new Dictionary<Type, ITable>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var tableObj in tableList)
        {
            if (tableObj is ITable table)
            {
                table.AutoAssignDatas();
                table.CreateTable();
                tableDic[table.Type] = table;
            }
        }
    }

    /// <summary>
    /// 등록된 테이블을 가져오는 함수 
    /// </summary>
    /// <typeparam name="T">사용할 Table</typeparam>
    /// <returns></returns>
    public T GetTable<T>() where T : class
    {
        return tableDic[typeof(T)] as T;
    }

#if UNITY_EDITOR
    public void AutoAssignTables()
    {
        tableList.Clear();

        string[] guids =
            UnityEditor.AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/10. Tables/Tables" });

        foreach (string guid in guids)
        {
            string path  = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var    asset = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (asset is ITable)
            {
                if (!tableList.Contains(asset))
                {
                    tableList.Add(asset);
                }
            }
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}