using chemistrecipe.data;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourseResult : MonoBehaviour {

    public GameObject LeftPanel;
    public string courseId;
    public int rankNumber = 1;

    // Use this for initialization
    void Start () {

        Object prefab = Resources.Load("Prefabs/UI/RankItemText");

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://chemresipe.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference courseReference = FirebaseDatabase.DefaultInstance.GetReference("courses");
        
        courseId = "090e0932aa78714276b66dd521019777";
        DatabaseReference scoreListReference = courseReference.Child(courseId).Child("scores");

        // Load Value
        scoreListReference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error... (no internet?)
                Debug.LogError("Firebase : Something Wrong");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Do something with snapshot...
                foreach (var childSnapshot in snapshot.Children)
                {
                    // Deserialize JSON into Object
                    Score data = JsonConvert.DeserializeObject<Score>(childSnapshot.GetRawJsonValue());
                    GameObject rankItemText = (GameObject)Instantiate(prefab, LeftPanel.transform);
                    rankItemText.GetComponent<Text>().text = "    "+rankNumber+"         "+data.name+"      "+data.time+"       "+data.score;
                    rankNumber++;
                }
            }
        });
    }
	
}
