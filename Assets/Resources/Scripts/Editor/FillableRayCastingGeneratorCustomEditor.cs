using UnityEditor;
using UnityEngine;
using System.Collections;
using ChemistRecipe.Experiment;
using Vuforia;
using ChemistRecipe.Utility;

[CustomEditor(typeof(FillableRayCastingGenerator))]
[CanEditMultipleObjects]
public class FillableRayCastingGeneratorCustomEditor : Editor
{
    SerializedProperty yOffset;

    public void OnEnable()
    {
        yOffset = serializedObject.FindProperty("currentY");
    }

    public override void OnInspectorGUI()
    {
        FillableRayCastingGenerator generator = (FillableRayCastingGenerator)target;
        
        serializedObject.Update();

        EditorGUILayout.PropertyField(yOffset, new GUIContent("Y Offset"));

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Show Verties to log"))
        {
            generator.copyVerties();
        }
    }
}