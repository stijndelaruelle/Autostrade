using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(LevelTimeline))]
[CanEditMultipleObjects]
public class CustomLevelTimelineInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        LevelTimeline levelTimeline = (LevelTimeline)target;

        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.fixedHeight = 30;

        //Title
        EditorGUILayout.LabelField("Current Frame", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        //Step Slider
        SerializedProperty serializedProperty = serializedObject.FindProperty("m_CurrentFrame");
        int tempStep = serializedProperty.intValue;

        EditorGUI.BeginChangeCheck();
        tempStep = EditorGUILayout.IntSlider(tempStep, levelTimeline.TimelineMinRange, levelTimeline.TimelineMaxRange);

        if (EditorGUI.EndChangeCheck())
        {
            //We only want to change trough a function!
            levelTimeline.SetFrame(tempStep);
        }

        EditorGUILayout.Space();

        //Previous & Next Buttons
        EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("< Previous", style)) { levelTimeline.PreviousFrame(); }
            if (GUILayout.Button("Next >", style))     { levelTimeline.NextFrame(); }

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
