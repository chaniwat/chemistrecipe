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

public class CourseResult : MonoBehaviour {

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

    private GlobalObject _Global;
    public List<Score> scoreList;

    // Use this for initialization
    void Start () {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://chemresipe.firebaseio.com/");

        _Global = GameObject.Find("_Global").GetComponent<GlobalObject>();

        gameResult = _Global.gameResult;
        scoreList = new List<Score>();
        createHighScorePanel();
        showResult();
    }

    void showResult()
    {
        if (_Global.isHighScore)
        {
            resultTextMessage.GetComponent<Text>().text = "You made new Highscore!";
        }
        else if (_Global.isFastestTime)
        {
            resultTextMessage.GetComponent<Text>().text = "New best time record!";
        }
        else {
            resultTextMessage.GetComponent<Text>().text = "";
        }

        scoreText.GetComponent<Text>().text = gameResult.data.score + " / 100";
        timeText.GetComponent<Text>().text = formatSecondToTimePattern(gameResult.data.time);

        getMedal(gameResult.data.score);
    }

    void createHighScorePanel()
    {
        prefab = Resources.Load("Prefabs/UI/RankItemText");

        courseReference = FirebaseDatabase.DefaultInstance.GetReference("courses").Child(gameResult.courseId);
        DatabaseReference scoreListReference = courseReference.Child("scores");
        scoreListReference.OrderByChild("score").GetValueAsync().ContinueWith(task =>
        {
            
            DataSnapshot snapshot = task.Result;
            foreach (var childSnapshot in snapshot.Children)
            {
                // Deserialize JSON into Object
                scoreList.Add(JsonConvert.DeserializeObject<Score>(childSnapshot.GetRawJsonValue()));
            }

            scoreList.Sort((a, b) => (a.score > b.score ? -1 : a.score < b.score ? 1 : 0));

            int startRankNumber = 1;

            while (startRankNumber < scoreList.Count + 1)
            {

                GameObject rankItemText = (GameObject)Instantiate(prefab, LeftPanel.transform);
                GameObject playerNameText = (GameObject)Instantiate(prefab, nameColumn.transform);

                Score score = scoreList[startRankNumber - 1];
                if (score.name.Length > 8)
                {
                    score.name = score.name.Substring(0, 8);
                }
                playerNameText.GetComponent<Text>().text = score.name;
                rankItemText.GetComponent<Text>().text = "   " + startRankNumber
                                                        + "                      "
                                                        + "        " + formatSecondToTimePattern(score.time)
                                                        + "       " + score.score;

                if (score.time == gameResult.data.time) {
                    rankItemText.GetComponent<Text>().color = Color.yellow;
                    playerNameText.GetComponent<Text>().color = Color.yellow;
                }
                
                startRankNumber++;
                if (startRankNumber == 10)
                {
                    break;
                }
            }
            getRank();
        });
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
        long playerPosition = 0;
        playerPosition = scoreList.FindIndex((item) =>  item.name == gameResult.data.name) + 1; 
        ranknumber = playerPosition.ToString();
        rankText.GetComponent<Text>().text = ranknumber;
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

}
