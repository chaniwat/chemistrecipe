using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace chemistrecipe
{
    public class TrackingImage : MonoBehaviour, ITrackableEventHandler
    {

        // Object Manager
        private ObjectManager objectManager;
        // Tracker
        private TrackableBehaviour mTrackableBehaviour;

        // Initialize
        void Start()
        {
            // Get object manager
            objectManager = GameObject.Find("_ObjectManager").GetComponent<ObjectManager>();

            // Subscribe tracking event
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }

            // Attach default child
            Instantiate(objectManager.getObject(ChemstObject.BOILING_FLASK), transform);
        }

        // Tracking event handler
        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
            tStatus = newStatus;

            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                    newStatus == TrackableBehaviour.Status.TRACKED ||
                    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                onTrackingFound();
            }
            else
            {
                onTrackingLost();
            }
        }

        protected void onTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            Debug.Log(gameObject.name + " Detected");
        }

        protected void onTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            Debug.Log(gameObject.name + " Lost");
        }

        // ---------------------------------------------------------------------------------------
        // Swapping model
        // Only for debugging

        // Status (Debugging swap model)
        private TrackableBehaviour.Status tStatus;

        // State
        private bool flag = true;

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
                newObj = (GameObject)Instantiate(objectManager.getObject(ChemstObject.BEAKER), transform);
            }
            else
            {
                newObj = (GameObject)Instantiate(objectManager.getObject(ChemstObject.BOILING_FLASK), transform);
            }

            // Hide if marker is lost
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
