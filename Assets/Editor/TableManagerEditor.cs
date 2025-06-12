using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TableManager))]
public class TableManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TableManager manager = (TableManager)target;

        if (GUILayout.Button("테이블 등록"))
        {
            manager.AutoAssignTables();
            Debug.Log("테이블 등록 성공!!");
        }
    }
}