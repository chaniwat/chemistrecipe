using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace chemistrecipe
{
    public class TrackingImage : MonoBehaviour, ITrackableEventHandler
    {

        // Models path
        public const string OBJECT_PATH = "Objects/";
        // Models load
        private Dictionary<string, UnityEngine.Object> models = new Dictionary<string, UnityEngine.Object>();

        // Tracker
        private TrackableBehaviour mTrackableBehaviour;
        // Status
        private TrackableBehaviour.Status tStatus;
        // State
        private bool flag = true;

        // Initialize
        void Start()
        {
            // Get and register image tracker
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }

            // Load models
            models["musk"] = Resources.Load(OBJECT_PATH + "musk") as UnityEngine.Object;
            models["beaker"] = Resources.Load(OBJECT_PATH + "beaker") as UnityEngine.Object;

            // Attach default child
            GameObject newObj = (GameObject)Instantiate(models["musk"], transform);
            newObj.transform.localPosition = new Vector3(0, 0, 0);
            newObj.transform.localEulerAngles = new Vector3(-90f, 0, 0);
            newObj.transform.localScale = new Vector3(0.00650444f, 0.00650444f, 0.00650444f);
        }

        // Tracker handler (Observer)
        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
            tStatus = newStatus;

            if (newStatus == TrackableBehaviour.Status.DETECTED ||
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
            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
            }
            children.ForEach(child => Destroy(child));

            // Create new model
            GameObject newObj;
            if (flag)
            {
                newObj = (GameObject)Instantiate(models["beaker"], transform);
                newObj.transform.localScale = new Vector3(0.005766949f, 0.005766949f, 0.005766949f);
            }
            else
            {
                newObj = (GameObject)Instantiate(models["musk"], transform);
                newObj.transform.localScale = new Vector3(0.00650444f, 0.00650444f, 0.00650444f);
            }

            newObj.transform.localPosition = new Vector3(0, 0, 0);
            newObj.transform.localEulerAngles = new Vector3(-90f, 0, 0);

            // Hide if can't detect marker
            if (tStatus == TrackableBehaviour.Status.NOT_FOUND ||
                    tStatus == TrackableBehaviour.Status.UNDEFINED ||
                    tStatus == TrackableBehaviour.Status.UNKNOWN)
            {
                Renderer[] rendererComponents = newObj.GetComponentsInChildren<Renderer>(true);
                Collider[] colliderComponents = newObj.GetComponentsInChildren<Collider>(true);

                // Disable rendering
                foreach (Renderer component in rendererComponents)
                {
                    component.enabled = false;
                }

                // Disable colliders
                foreach (Collider component in colliderComponents)
                {
                    component.enabled = false;
                }
            }

            flag = !flag;
        }

    }
}
