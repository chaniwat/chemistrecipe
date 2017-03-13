using chemistrecipe.localization;
using chemistrecipe.scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLocalization : MonoBehaviour {

    [Header("Main Menu")]
    public Button startBtn;
    public Button connectBtn;
    public Button settingBtn;
    public Button exitBtn;

    [Header("Course Selection")]
    public Button tutorialBtn;
    public Button getMarkerBtn;
    public Button playBtn;
    public Button backBtn;
    public Text courseDescriptionText;

    [Header("Setting")]
    public Text profileText;
    public Text nameText;
    public Text aliasText;
    public Text avatarText;
    public Text soundText;
    public Text volumeText;
    public Text languageText;
    public Text gameText;
    public Text playerUidText;
    public Button confirmBtn;

    [Header("Lazy Mode")]
    public MainMenuScript mainMenuScript;

    private GlobalObject _globalObject;

    void Start()
    {
        _globalObject = GameObject.Find("_Global").GetComponent<GlobalObject>();

        setLocale();
    }

    public void setLocale()
    {
        LocalLanguage currentLocale = _globalObject.currentLocale;

        string main = "mainmenu.main.";
        startBtn.GetComponentInChildren<Text>().text = currentLocale.getString(main + "start");
        connectBtn.GetComponentInChildren<Text>().text = currentLocale.getString(main + "connect");
        settingBtn.GetComponentInChildren<Text>().text = currentLocale.getString(main + "setting");
        exitBtn.GetComponentInChildren<Text>().text = currentLocale.getString(main + "exit");

        string course = "mainmenu.course.";
        tutorialBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "tutorialbtn");
        getMarkerBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "getmarkerbtn");
        playBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "playbtn");
        backBtn.GetComponentInChildren<Text>().text = currentLocale.getString(course + "backbtn");
        courseDescriptionText.text = currentLocale.getString(course + "textcoursedescription");
        mainMenuScript.defaulDescription = currentLocale.getString(course + "textcoursedescription");

        string setting = "mainmenu.setting.";
        profileText.text = currentLocale.getString(setting + "profile");
        nameText.text = currentLocale.getString(setting + "profile.nametext");
        aliasText.text = currentLocale.getString(setting + "profile.alias");
        avatarText.text = currentLocale.getString(setting + "profile.avatar");
        soundText.text = currentLocale.getString(setting + "sound");
        volumeText.text = currentLocale.getString(setting + "sound.volume");
        languageText.text = currentLocale.getString(setting + "language");
        gameText.text = currentLocale.getString(setting + "game");
        playerUidText.text = currentLocale.getString(setting + "game.player");
        confirmBtn.GetComponentInChildren<Text>().text = currentLocale.getString(setting + "confirm");

    }

}
