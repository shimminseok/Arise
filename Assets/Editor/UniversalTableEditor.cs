using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScriptableObject), true)]
public class UniversalTableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (target is ITable table)
        {
            if (GUILayout.Button("데이터 등록"))
            {
                table.AutoAssignDatas();
            }
        }
    }
}