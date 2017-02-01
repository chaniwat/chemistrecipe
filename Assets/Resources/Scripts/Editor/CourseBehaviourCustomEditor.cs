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

    public void OnEnable()
    {
        DatasetName = serializedObject.FindProperty("DatasetName");
        DatasetExistanceStatus = serializedObject.FindProperty("DatasetExistanceStatus");
        timerText = serializedObject.FindProperty("timerText");
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

            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            GUILayout.Label("Please attach CourseScript to component first");
        }
    }
}