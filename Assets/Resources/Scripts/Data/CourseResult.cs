using chemistrecipe.data;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Resources.Scripts.Data;

public class CourseResult : MonoBehaviour {

    public static int startRankNumber = 1;

    public GameObject LeftPanel;
    public GameObject nameColumn;
    public GameObject timeText;
    public GameObject scoreText;
    public GameObject rankText;
    public GameObject resultTextMessage;
    public GameObject medal;

    public Sprite goldMedal;
    public Sprite silverMedal;
    public Sprite copperMedal;

    //get these value from previous scene (gameplay scene)
    GameResult gameResult; // Complete, time out, failed

    UnityEngine.Object prefab;
    DatabaseReference courseReference;

    // Use this for initialization
    void Start () {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://chemresipe.firebaseio.com/");

        //Example
        gameResult = new GameResult();
        gameResult.courseId = "090e0932aa78714276b66dd521019777";
        gameResult.uID = "uid1";
        gameResult.data = new Score("Marktrs", 612, 60); // Score(string name, int time, int score);

        showResult();
        createHighScorePanel();
    }

    void showResult()
    {
        int newScore = gameResult.data.score;
        int newTime = gameResult.data.time;
        resultTextMessage.GetComponent<Text>().text = compareScoreAndTime(newScore, newTime); //get gameResultMessage
        scoreText.GetComponent<Text>().text = newScore + " / 100"; //show score
        timeText.GetComponent<Text>().text = formatSecondToTimePattern(newTime); //show used time
        getMedal(newScore);
        getRank();
    }
    
    string compareScoreAndTime(int newScore, int newTime)
    {
        string result="";
        int oldScore, oldTime;
        FirebaseDatabase.DefaultInstance.GetReference("courses")
            .Child(gameResult.courseId)
            .Child("scores")
            .Child(gameResult.uID)
            .GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {   
                // Handle the error... (no internet?)
                Debug.LogError("Firebase : Something Wrong");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Score data = JsonConvert.DeserializeObject<Score>(snapshot.GetRawJsonValue());
                
                oldScore = data.score;
                oldTime = data.time;

                if (newScore >= oldScore)
                {
                    submitGameResult(newScore, newTime);
                    result = "You made new Highscore!";
                    if (newTime < oldTime)
                    {   
                        result = "New best time record and Highscore !";
                    }
                }
            }
        });
        return result;
    }
   
    void submitGameResult(int score, int time)
    {
        DatabaseReference userRecordRef = FirebaseDatabase.DefaultInstance.GetReference("courses")
            .Child(gameResult.courseId)
            .Child("scores")
            .Child(gameResult.uID);
        userRecordRef.Child("score").SetValueAsync(score);
        userRecordRef.Child("time").SetValueAsync(time);
    }

    void createHighScorePanel()
    {
        prefab = Resources.Load("Prefabs/UI/RankItemText");

        courseReference = FirebaseDatabase.DefaultInstance.GetReference("courses").Child(gameResult.courseId);
        DatabaseReference scoreListReference = courseReference.Child("scores");
        scoreListReference.OrderByChild("score").ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        foreach (Transform child in LeftPanel.transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in nameColumn.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<Score> scoreList = new List<Score>();

        DataSnapshot snapshot = args.Snapshot;
        foreach (var childSnapshot in snapshot.Children)
        {
            // Deserialize JSON into Object
            scoreList.Add(JsonConvert.DeserializeObject<Score>(childSnapshot.GetRawJsonValue()));
        }

        scoreList.Sort((a, b) => (a.score > b.score ? -1 : a.score < b.score ? 1 : 0));

        while (startRankNumber < scoreList.Count+1) {

            GameObject rankItemText = (GameObject)Instantiate(prefab, LeftPanel.transform);
            GameObject playerNameText = (GameObject)Instantiate(prefab, nameColumn.transform);

            Score score = scoreList[startRankNumber - 1];
            if (score.name.Length > 8)
            {
                score.name = score.name.Substring(0, 8);
            }
            score.name += new string(' ', 8 - score.name.Length);
            playerNameText.GetComponent<Text>().text = score.name;
            rankItemText.GetComponent<Text>().text = "   " + startRankNumber
                                                    + "                      "
                                                    + "      " + formatSecondToTimePattern(score.time)
                                                    + "       " + score.score;
            
            startRankNumber++;
            if (startRankNumber == 10) {
                break;
            }
        }
    }

    string formatSecondToTimePattern(int sec) {
        int hour = 0;
        int minute = 0;
        string formatedTime = "";

        minute = sec / 60;
        sec -= minute * 60;
        hour = minute / 60;
        minute -= hour * 60;

        string hourText = hour.ToString();
        string minuteText = minute.ToString();
        string secText = sec.ToString();

        if (hour < 10) {
            hourText = "0" + hourText;
        }
        if (minute < 10)
        {
            minuteText = "0" + minuteText;
        }
        if (sec < 10)
        {
            secText = "0" + secText;
        }

        formatedTime = hourText + ":" + minuteText + ":" + secText;
        return formatedTime;
    }

    void getRank() {
        string ranknumber = "-";
        //count from db
        FirebaseDatabase.DefaultInstance.GetReference("courses")
            .Child(gameResult.courseId)
            .Child("scores").OrderByChild("score")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error... (no internet?)
                    Debug.LogError("Firebase : Something Wrong");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    long playerPosition = -1;
                    foreach (var child in snapshot.Children) {
                        playerPosition++;
                        Score data = JsonConvert.DeserializeObject<Score>(child.GetRawJsonValue());
                        if (data.name == gameResult.data.name);
                        {
                            break;
                        }
                    }
                    ranknumber = (snapshot.ChildrenCount - playerPosition).ToString();
                    rankText.GetComponent<Text>().text = ranknumber;
                }
            });
   }

    void getMedal(int newScore) {
        //calculate and show medal
        if (newScore > 80)
        {
            medal.GetComponent<Image>().sprite = goldMedal;
        }
        else if (newScore > 60)
        {
            medal.GetComponent<Image>().sprite = silverMedal;
        }
        else
        {
            medal.GetComponent<Image>().sprite = copperMedal;
        }
    }

    void getGameResultMessage() {
        //eg. "you get new highscore", "1st in global rank", "new best time recorded"
    }
}
