using ChemistRecipe.AppObject;
using ChemistRecipe.Experiment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace ChemistRecipe.AR
{   
    public class TrackingImage : MonoBehaviour, ITrackableEventHandler
    {
        [Serializable]
        public class TrackingImageParam
        {
            public bool canFilp = true;
            public float filpXOffset = 0.0f;
            public float filpYOffset = 0.0f;
            public float filpZOffset = 0.0f;
        }

        #region Settings

        [Tooltip("FillableObject that attach to this tracking")]
        public Equipment attachObject = null;
        [Tooltip("Can object filp?")]
        public bool canFilp = true;
        [Tooltip("X Offset for centering the object")]
        public float filpXOffset = 0.0f;
        [Tooltip("Y Offset for centering the object")]
        public float filpYOffset = 0.0f;
        [Tooltip("Z Offset for centering the object")]
        public float filpZOffset = 0.0f;

        #endregion

        #region Internal vars

        private bool trackingVerticalRotation = false;

        #endregion

        #region Vuforia vars

        private TrackableBehaviour mTrackableBehaviour;
        private bool tracking = false;

        #endregion

        #region Initialize tracker

        void Start()
        {
            registerTrackingHandler();
            initializeObject();
        }
        
        /// <summary>
        /// Initialize attach object
        /// </summary>
        private void initializeObject()
        { 
            if(attachObject)
            {
                // Set to default local
                attachObject.transform.localPosition = Vector3.zero;
                attachObject.transform.localEulerAngles = Vector3.zero;
            }
            else
            {
                Debug.LogWarning("No object attach to " + gameObject.name);
            }
        }

        #endregion

        #region TrackableStateChange

        /// <summary>
        /// Register the tracking state change handler
        /// </summary>
        private void registerTrackingHandler()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        /// <summary>
        /// Tracking state change handler
        /// </summary>
        /// <param name="previousStatus"></param>
        /// <param name="newStatus"></param>
        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                    newStatus == TrackableBehaviour.Status.TRACKED ||
                    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                Debug.Log(gameObject.name + " Detected");
                tracking = true;

                OnTrackingFound();
            }
            else
            {
                Debug.Log(gameObject.name + " Lost");
                tracking = false;

                OnTrackingLost();
            }
        }

        #endregion

        void Update()
        {
            if (tracking)
            {
                OnTracking();   
            }
        }

        private void OnTracking()
        {
            if (attachObject)
            {
                // Filp the object
                if (canFilp)
                {
                    Vector3 upAxis = transform.up;

                    if (!trackingVerticalRotation && upAxis.y <= 0.5f)
                    {
                        attachObject.transform.localEulerAngles = new Vector3(90, 0, 0);
                        attachObject.transform.localPosition = new Vector3(filpXOffset, filpYOffset, filpZOffset);
                        trackingVerticalRotation = true;
                    }
                    else if (trackingVerticalRotation && upAxis.y > 0.5f)
                    {
                        attachObject.transform.localEulerAngles = Vector3.zero;
                        attachObject.transform.localPosition = Vector3.zero;
                        trackingVerticalRotation = false;
                    }
                }
            }
        }

        #region Tracking event handler

        protected void OnTrackingFound()
        {
            toggleObject(true);
        }

        protected void OnTrackingLost()
        {
            toggleObject(false);
        }

        protected void toggleObject(bool toggleTo)
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                // Skipping Particle System
                if (attachObject && component.gameObject.GetComponent<ParticleSystem>())
                {
                    continue;
                }

                component.enabled = toggleTo;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                // Skipping Particle System
                if (attachObject && component.gameObject.GetComponent<ParticleSystem>())
                {
                    continue;
                }

                component.enabled = toggleTo;
            }

            // Toggle enable flowing
            if (attachObject)
            {
                attachObject.GetComponent<FillableEquipment>().enableFlow = toggleTo;
            }
        }

        #endregion
    }
}
