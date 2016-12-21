using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackingImage : MonoBehaviour, ITrackableEventHandler {

    // Models path
    public const string MODEL_PATH = "Models/";
    // Models load
    private Dictionary<string, UnityEngine.Object> models = new Dictionary<string, UnityEngine.Object>();

    // Tracker
    private TrackableBehaviour mTrackableBehaviour;
    // State
    private bool flag = true;

    // Initialize
    void Start () {
        // Get and register image tracker
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if(mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        // Load models
        models["m1"] = Resources.Load(MODEL_PATH + "m1") as UnityEngine.Object;
        models["m2"] = Resources.Load(MODEL_PATH + "m2") as UnityEngine.Object;

        Debug.Log(models["m1"]);

        // Attach default child
        GameObject newObj = (GameObject)Instantiate(models["m1"], transform);
        newObj.transform.localPosition = new Vector3(0, 0, 0);
        newObj.transform.localEulerAngles = new Vector3(-90f, 0, 0);
        newObj.transform.localScale = new Vector3(0.00650444f, 0.00650444f, 0.00650444f);
    }

    // Tracker handler (Observer)
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if(newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("detected");
        }
        else
        {
            Debug.Log("lost");
        }
    }

    // Swapping model
    public void swapModel()
    {
        // Remove all child
        List<GameObject> children = new List<GameObject>();
        foreach(Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(child => Destroy(child));

        if (flag)
        {
            GameObject newObj = (GameObject) Instantiate(models["m2"], transform);
            newObj.transform.localPosition = new Vector3(0, 0, 0);
            newObj.transform.localEulerAngles = new Vector3(-90f, 0, 0);
            newObj.transform.localScale = new Vector3(0.005766949f, 0.005766949f, 0.005766949f);
        }
        else
        {
            GameObject newObj = (GameObject)Instantiate(models["m1"], transform);
            newObj.transform.localPosition = new Vector3(0, 0, 0);
            newObj.transform.localEulerAngles = new Vector3(-90f, 0, 0);
            newObj.transform.localScale = new Vector3(0.00650444f, 0.00650444f, 0.00650444f); 
        }

        flag = !flag;
    }

}
