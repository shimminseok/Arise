#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProjectileSkillSO))]
public class ProjectileSkillSOEditor : Editor
{
    SerializedProperty shapeTypeProp;
    SerializedProperty radiusProp;
    SerializedProperty rectSizeProp;
    SerializedProperty ownerProp;

    void OnEnable()
    {
        shapeTypeProp = serializedObject.FindProperty("ShapeType");
        radiusProp = serializedObject.FindProperty("Radius");
        rectSizeProp = serializedObject.FindProperty("RectSize");
        ownerProp = serializedObject.FindProperty("Owner");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "ShapeType", "Radius", "RectSize", "Owner");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Shape Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(shapeTypeProp);
        var shape = (AreaSkillSO.Shape)shapeTypeProp.enumValueIndex;
        switch (shape)
        {
            case AreaSkillSO.Shape.Circle:
                EditorGUILayout.PropertyField(radiusProp);
                break;
            case AreaSkillSO.Shape.Rect:
                EditorGUILayout.PropertyField(rectSizeProp);
                break;
        }
        
        EditorGUILayout.PropertyField(ownerProp);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
