using chemistrecipe.localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObject : MonoBehaviour {

    public string id = "NULL";

    // For Testing
    public string message = "NULL";

    public bool isOpenTutorial = false;
    
    public GameResult gameResult;
    public bool isHighScore = false;
    public bool isFastestTime = false;

    public string playerUid;
    public string playerName;
    public string playerAlias;
    public string playerLevel;
    public Sprite playerAvatar;

    private Dictionary<string, LocalLanguage> _localization;
    public Dictionary<string, LocalLanguage> localization
    {
        get
        {
            return _localization;
        }
    }
    private LocalLanguage _currentLocale;
    public LocalLanguage currentLocale
    {
        get
        {
            return _currentLocale;
        }
    }

    public static GlobalObject instance;

    // Called when object is loaded (before start)
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);

            _localization = new Dictionary<string, LocalLanguage>();
            _localization.Add("en", new EnLanguage());
            _localization.Add("th", new ThLanguage());
            _currentLocale = _localization["en"];

            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    

}
