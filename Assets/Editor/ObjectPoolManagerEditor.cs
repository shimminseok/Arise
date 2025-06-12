using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectPoolManager))]
public class ObjectPoolManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ObjectPoolManager manager = (ObjectPoolManager)target;

        if (GUILayout.Button("오브젝트 등록"))
        {
            manager.AutoAssignObject();
            Debug.Log("오브젝트 등록 성공!!");
        }
    }
}