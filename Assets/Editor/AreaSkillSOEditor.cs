#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(AreaSkillSO))]
public class AreaSkillSOEditor : Editor
{
    SerializedProperty shapeTypeProp;
    SerializedProperty radiusProp;
    SerializedProperty angleProp;
    SerializedProperty rectSizeProp;
    SerializedProperty ownerProp;
    SerializedProperty offsetProp;

    void OnEnable()
    {
        shapeTypeProp = serializedObject.FindProperty("ShapeType");
        radiusProp = serializedObject.FindProperty("Radius");
        angleProp = serializedObject.FindProperty("Angle");
        rectSizeProp = serializedObject.FindProperty("RectSize");
        ownerProp = serializedObject.FindProperty("Owner");
        offsetProp = serializedObject.FindProperty("Offset");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject,
            "ShapeType", "Radius", "Angle", "RectSize", "Owner", "Offset" );

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
            case AreaSkillSO.Shape.Cone:
                EditorGUILayout.PropertyField(radiusProp);
                EditorGUILayout.PropertyField(angleProp);
                break;
        }
        EditorGUILayout.PropertyField(ownerProp);
        EditorGUILayout.PropertyField(offsetProp);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
