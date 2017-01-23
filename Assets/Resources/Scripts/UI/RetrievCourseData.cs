using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;

public class RetrievCourseData : MonoBehaviour {

    ArrayList Courses;

	// Use this for initialization
	void Start () {
        
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://chemresipe.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference courseReference = FirebaseDatabase.DefaultInstance.GetReference("courses");

        Object obj = Resources.Load("Prefabs/CourseButton");

        courseReference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
                foreach (var childSnapshot in snapshot.Children){
                    GameObject courseButton = (GameObject)Instantiate(obj, GameObject.Find("CourseListGrid").transform);
                    courseButton.GetComponentInChildren<Text>().text = childSnapshot.Child("name").Value.ToString();
                    courseButton.GetComponent<CourseButtonScript>().courseName = childSnapshot.Child("name").Value.ToString();
                    courseButton.GetComponent<CourseButtonScript>().courseID = childSnapshot.Value.ToString();
                    courseButton.GetComponent<CourseButtonScript>().courseDescription = childSnapshot.Child("description").Value.ToString();
                }
            }
        });

    }
    
}
