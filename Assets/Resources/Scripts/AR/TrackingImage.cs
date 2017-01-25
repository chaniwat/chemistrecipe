using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace chemistrecipe
{   
    public class TrackingImage : MonoBehaviour, ITrackableEventHandler
    {

        // Settings
        [Tooltip("FillableObject that attach to this tracking")]
        public FillableObject attachObject = null;
        [Tooltip("Can object filp?")]
        public bool canFilp = true;
        [Tooltip("Z Offset for centering the object")]
        public float zOffset = 0.0f;

        // Tracker
        private TrackableBehaviour mTrackableBehaviour;

        // Is tracking?
        private bool tracking = false;

        // Tracking vertical status
        private bool verticalRotation = false;

        // Initialize
        void Start()
        {
            // Register tracking
            registerTrackable();

            initializeObject();
        }

        private void initializeObject()
        {
            // Set to default local
            attachObject.transform.localPosition = Vector3.zero;
            attachObject.transform.localEulerAngles = Vector3.zero;
        }

        void Update()
        {
            if (tracking)
            {
                if (canFilp)
                {
                    Vector3 upAxis = transform.up;

                    if (!verticalRotation && upAxis.y <= 0.5f)
                    {
                        attachObject.transform.localEulerAngles = new Vector3(90, 0, 0);
                        attachObject.transform.localPosition = new Vector3(0, 0, zOffset);
                        verticalRotation = true;
                    }
                    else if (verticalRotation && upAxis.y > 0.5f)
                    {
                        attachObject.transform.localEulerAngles = Vector3.zero;
                        attachObject.transform.localPosition = Vector3.zero;
                        verticalRotation = false;
                    }
                }
            }
        }

        #region TrackableStateChange

        private void registerTrackable()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        // Tracking event handler
        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
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

        #endregion

        #region Tracking event handler

        protected void onTrackingFound()
        {
            Debug.Log(gameObject.name + " Detected");
            tracking = true;

            enableObject();

            attachObject.GetComponent<FillableObject>().enableFlow = true;
        }

        protected void onTrackingLost()
        {
            Debug.Log(gameObject.name + " Lost");
            tracking = false;

            disableObject();

            attachObject.GetComponent<FillableObject>().enableFlow = false;
        }

        protected void enableObject()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                // Skipping Particle System
                if(component.gameObject.GetComponent<ParticleSystem>() != null)
                {
                    continue;
                }

                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                // Skipping Particle System
                if (component.gameObject.GetComponent<ParticleSystem>() != null)
                {
                    continue;
                }

                component.enabled = true;
            }
        }

        protected void disableObject()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                // Skipping Particle System
                if (component.gameObject.GetComponent<ParticleSystem>() != null)
                {
                    continue;
                }

                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                // Skipping Particle System
                if (component.gameObject.GetComponent<ParticleSystem>() != null)
                {
                    continue;
                }

                component.enabled = false;
            }
        }

        #endregion
    }
}
