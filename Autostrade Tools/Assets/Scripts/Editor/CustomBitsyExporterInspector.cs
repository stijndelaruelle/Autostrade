using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BitsyExporter))]
[CanEditMultipleObjects]
public class CustomBitsyExporterInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        BitsyExporter bitsyExporter = (BitsyExporter)target;

        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.fixedHeight = 30;

        //Title
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        //Timeline
        SerializedProperty serializedProperty = serializedObject.FindProperty("m_LevelTimeline");
        EditorGUILayout.ObjectField(serializedProperty, new GUIContent("Level Timeline"));

        //Game Name
        serializedProperty = serializedObject.FindProperty("m_GameName");
        serializedProperty.stringValue = EditorGUILayout.TextField("Game Name", serializedProperty.stringValue);
        EditorGUILayout.Space();

        //Previous & Next Buttons
        if (GUILayout.Button("Export", style))
        {
            bitsyExporter.Export();
        }

        if (GUILayout.Button("Open File", style))
        {
            bitsyExporter.OpenExportedFile();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
