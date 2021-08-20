using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FlatVector))]
public class FlatVectorDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var xLabelRect = new Rect(position.x, position.y, 30, position.height);
        var xRect = new Rect(position.x + 12, position.y, 62, position.height);
        var zLabelRect = new Rect(position.x + 79, position.y, 30, position.height);
        var zRect = new Rect(position.x + 90, position.y, 62, position.height);

        EditorGUI.LabelField(xLabelRect, "X");
        EditorGUI.PropertyField(xRect, property.FindPropertyRelative("x"), GUIContent.none);
        EditorGUI.LabelField(zLabelRect, "Z");
        EditorGUI.PropertyField(zRect, property.FindPropertyRelative("z"), GUIContent.none);


        EditorGUI.EndProperty();
    }

}