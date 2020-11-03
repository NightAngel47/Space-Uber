﻿/*
 * CustPropertyDrawer.cs
 * Author(s): Sydney
 * Created on: #CREATIONDATE#
 * Description: Sets up the custom property drawer in the inspector
 */

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(ArrayLayout))]
[CustomPropertyDrawer(typeof(ArrayLayoutBool))]
public class CustPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);
        Rect newposition = position;
        newposition.y += 18f;
        SerializedProperty data = property.FindPropertyRelative("rows");
        //data.rows[0][]
        for (int j = 0; j < 10; j++)
        {
            SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
            newposition.height = 18f;
            if (row.arraySize != 10)
                row.arraySize = 10;
            newposition.width = position.width / 10;
            for (int i = 0; i < 10; i++)
            {
                EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(i), GUIContent.none);
                newposition.x += newposition.width;
            }

            newposition.x = position.x;
            newposition.y += 18f;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 25f * 8;
    }
}
