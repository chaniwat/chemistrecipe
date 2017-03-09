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

    public static GlobalObject instance;

    // Called when object is loaded (before start)
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    

}
