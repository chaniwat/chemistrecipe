using UnityEditor;
using UnityEngine;
using System.Collections;
using ChemistRecipe.Experiment;
using Vuforia;

[CustomEditor(typeof(CourseBehaviour))]
public class CourseBehaviourCustomEditor : Editor
{
    SerializedProperty DatasetName;
    SerializedProperty DatasetExistanceStatus;
    SerializedProperty timerText;
    SerializedProperty sceneController;
    SerializedProperty UISoundController;
    SerializedProperty textMeshTemplate;
    SerializedProperty highlightPlaneTemplate;

    public void OnEnable()
    {
        DatasetName = serializedObject.FindProperty("DatasetName");
        DatasetExistanceStatus = serializedObject.FindProperty("DatasetExistanceStatus");
        timerText = serializedObject.FindProperty("timerText");
        sceneController = serializedObject.FindProperty("sceneController");
        UISoundController = serializedObject.FindProperty("UISoundController");
        textMeshTemplate = serializedObject.FindProperty("textMeshTemplate");
        highlightPlaneTemplate = serializedObject.FindProperty("highlightPlaneTemplate");
    }

    public override void OnInspectorGUI()
    {
        CourseBehaviour courseBehaviour = (CourseBehaviour)target;

        if (courseBehaviour.CourseScript)
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(DatasetName, new GUIContent("Vuforia Dataset"));
            EditorGUILayout.LabelField("Dataset Status", courseBehaviour.DatasetExistanceStatus.ToString());
            if (GUILayout.Button("Check Dataset"))
            {
                if (courseBehaviour.CheckDataset())
                {
                    DatasetExistanceStatus.enumValueIndex = (int)CourseBehaviour.DatasetExistance.FOUND;
                    courseBehaviour.loadTrackerNameList();
                }
                else
                {
                    DatasetExistanceStatus.enumValueIndex = (int)CourseBehaviour.DatasetExistance.NOT_FOUND;
                    courseBehaviour.resetTrackerNameList();
                }
            }

            GUILayout.Label("");

            EditorGUILayout.LabelField("Course Name", courseBehaviour.CourseScript.Name);
            EditorGUILayout.LabelField("Course Author", courseBehaviour.CourseScript.Author);
            EditorGUILayout.LabelField("Course Version", courseBehaviour.CourseScript.Version);

            GUILayout.Label("");

            EditorGUILayout.PropertyField(timerText, new GUIContent("Timer UI"));
            EditorGUILayout.PropertyField(sceneController, new GUIContent("Scene Controller"));
            EditorGUILayout.PropertyField(UISoundController, new GUIContent("Course UI Sound Controller"));

            GUILayout.Label("");

            EditorGUILayout.PropertyField(textMeshTemplate, new GUIContent("TextMesh Template"));
            EditorGUILayout.PropertyField(highlightPlaneTemplate, new GUIContent("Highlight Plane Template"));

            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            GUILayout.Label("Please attach CourseScript to component first");
        }
    }
}