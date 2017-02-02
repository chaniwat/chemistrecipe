using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text timerText;
    private float startTime;
    private bool finished = false;
    private int counter;
    // Use this for initialization
    void Start () {
        startTime = Time.time;
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (finished) return;
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        
        string seconds = (t % 60).ToString("0.00");

        seconds = seconds.Replace(".", ":");

        if (seconds.Length == 4) {
            seconds = "0" + seconds;
        }
        if (minutes.Length == 1)
        {
            minutes = "0" + minutes;
        }
        counter = Mathf.RoundToInt(t % 60) ;
        timerText.text = minutes + ":" + seconds;
    }

    public void Finish() {
        finished = true;
        timerText.color = Color.yellow;
    }

    public int getTime()
    {
        return counter;
    }
}
