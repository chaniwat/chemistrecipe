﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObject : MonoBehaviour {

    public string message = "NULL";

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