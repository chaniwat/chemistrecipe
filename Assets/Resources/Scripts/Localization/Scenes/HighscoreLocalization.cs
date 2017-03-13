using chemistrecipe.localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreLocalization : MonoBehaviour {

    [Header("Buttons")]
    public Button restartBtn;
    public Button mainmenuBtn;

    private GlobalObject _globalObject;

    void Start()
    {
        _globalObject = GameObject.Find("_Global").GetComponent<GlobalObject>();

        setLocale();
    }

    public void setLocale()
    {
        LocalLanguage currentLocale = _globalObject.currentLocale;

        string course = "course.";
        restartBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "restartbtn");
        mainmenuBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "mainmenubtn");
    }

}
