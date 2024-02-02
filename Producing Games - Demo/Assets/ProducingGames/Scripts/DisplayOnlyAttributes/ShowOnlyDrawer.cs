using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
public class ShowOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string valueString;
        
        switch(property.propertyType)
        {
            case SerializedPropertyType.Boolean:
                valueString = property.boolValue.ToString();
                break;
                case SerializedPropertyType.String:
                valueString = property.stringValue.ToString();
                break;
            case SerializedPropertyType.Integer:
                valueString = property.intValue.ToString();
                break;
                case SerializedPropertyType.Float:
                valueString = property.floatValue.ToString();
                break;
            case SerializedPropertyType.Vector2:
                valueString = property.vector2Value.ToString();
                break;
            case SerializedPropertyType.Vector3:
                valueString = property.vector3Value.ToString();
                break;
                default:
                valueString = "(not supported)";
                break;


        }

        EditorGUI.LabelField(position, label.text, valueString);
    }
}
