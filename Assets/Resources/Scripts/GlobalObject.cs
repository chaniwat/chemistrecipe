﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObject : MonoBehaviour {

    public string id = "NULL";

    // For Testing
    public string message = "NULL";

    public string courseID = "090e0932aa78714276b66dd521019777";
    public string userID = "uid1";
    public string playerName = "Marktrs";
    public int score;
    public int time;

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
            Destroy(this);
        }
    }

}
