using UnityEditor;
using UnityEngine;

public class BoolShowIfAttribute : PropertyAttribute
{
    public string ConditionFieldName;

    public BoolShowIfAttribute(string conditionFieldName)
    {
        ConditionFieldName = conditionFieldName;
    }
}

[CustomPropertyDrawer(typeof(BoolShowIfAttribute))]
public class BoolShowIfDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        BoolShowIfAttribute showIf = (BoolShowIfAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIf.ConditionFieldName);

        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
        {
            bool enabled = conditionProperty.boolValue;
            return enabled ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
        }

        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        BoolShowIfAttribute showIf = (BoolShowIfAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIf.ConditionFieldName);

        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean && conditionProperty.boolValue)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}