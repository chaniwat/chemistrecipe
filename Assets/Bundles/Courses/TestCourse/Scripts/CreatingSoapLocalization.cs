using chemistrecipe.localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatingSoapLocalization : MonoBehaviour
{

    [Header("Overlay")]
    public Button stirBtn;
    public Button restartBtn;

    [Header("Menu")]
    public Button resumeBtn;
    public Button instructionBtn;
    public Button settingBtn;
    public Button restartMenuBtn;
    public Button MainMenuBtn;

    [Header("Tutorial")]
    public Button closeTutorialBtn;

    private GlobalObject _globalObject;

    // Use this for initialization
    void Start()
    {
        _globalObject = GameObject.Find("_Global").GetComponent<GlobalObject>();

        setLocale();
    }

    public void setLocale()
    {
        LocalLanguage currentLocale = _globalObject.currentLocale;

        string course = "course.";
        stirBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "stir");
        restartBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "restartbtn");

        resumeBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "resumebtn");
        instructionBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "instructionbtn");
        settingBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "settingbtn");
        restartMenuBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "restartbtn");
        MainMenuBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "mainmenubtn");

        closeTutorialBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "closetutorialbtn");
    }

}
