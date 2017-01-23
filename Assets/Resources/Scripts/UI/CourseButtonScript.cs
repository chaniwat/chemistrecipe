using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourseButtonScript : MonoBehaviour {

    public string courseID = "";
    public string courseName = "";
    public string courseDescription = "";
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(changeCoursePage);
    }

    void changeCoursePage()
    {
        GameObject.Find("StarLevel").GetComponent<Image>().enabled = true;
        GameObject.Find("PrintButton").GetComponent<Image>().enabled = true;
        GameObject.Find("PrintButton").GetComponentInChildren<Text>().enabled = true;
        GameObject.Find("TextScore").GetComponent<Text>().enabled = true;
        GameObject.Find("TextCourseName").GetComponent<Text>().enabled = true;

        GameObject.Find("TextCourseName").GetComponent<Text>().text = courseName;
        GameObject.Find("TextCourseDescription").GetComponent<Text>().text = courseDescription;
    }
    

}
