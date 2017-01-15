using chemistrecipe.element;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chemistrecipe
{
    public class FillableObject : MonoBehaviour
    {
        /* Settings & States */

        // Container Setting
        [Header("Container Setting")]
        [Tooltip("Max capacity to store a liquid (metric: mL)")]
        public float maxCapacity = 0.0f;
        [Tooltip("Initial capacity, current capacity (metric: mL)")]
        public float capacity = 0.0f;
        [Tooltip("Fillable area (Collider3D)")]
        public Collider FillableArea;

        // Container State
        public bool isContainLiquid
        {
            get { return capacity > 0; }
        }

        // Liquid Setting
        [Header("Liquid Setting")]
        [Tooltip("Liquid Name (Element)")]
        public string liquidName = "";
        [Tooltip("Liquid Color")]
        public Color liquidColor = Color.white;

        // Flow Setting
        [Header("Flow Setting")]
        [Tooltip("Start flow when up.y (y of green axis) < this value")]
        public float startFlowAtY = -0.1f;
        [Tooltip("Minimum speed flow (mL) per secound (up.y (y of green axis) = startFlowAtY)")]
        public float minFlowSpeed = 0.0f;
        [Tooltip("Maximum speed flow (mL) per secound (up.y (y of green axis) = -1.0f)")]
        public float maxFlowSpeed = 10.0f;
        [Tooltip("Maxinum flow speed when up.y (y of green axis) <= this value")]
        public float maxFlowSpeedAtY = -1.0f;

        // Flow State
        public bool isFlowing
        {
            get
            {
                return lEmission.enabled;
            }
        }
        private float accumulateCapacity = 0f;
        private float releaseCapacity = 0.25f;

        // For Debugging
        [Header("Debugging")]
        public bool infinityCapacity = false;
        public TextMesh debugText = null;
        

        /* End Settings & States */
        /* --------------------- */
        /* Internal */

        // ParticleSystem
        private ParticleSystem lParticle;
        private ParticleSystem.EmissionModule lEmission;

        /* End Internal */

        void Start()
        {
            // Setting Particle System
            lParticle = GetComponentInChildren<ParticleSystem>();

            var lParticleMain = lParticle.main;
            lEmission = lParticle.emission;
            lEmission.rateOverTimeMultiplier = 0;

            lParticleMain.startColor = new ParticleSystem.MinMaxGradient(liquidColor, liquidColor);
        }

        void Update()
        {
            if(debugText != null)
            {
                debugText.text = capacity.ToString();
            }

            if (!isContainLiquid && !infinityCapacity)
            {
                lEmission.enabled = false;
            }
            else
            {
                if (transform.up.y <= startFlowAtY && !isFlowing)
                {
                    lEmission.enabled = true;
                }
                else if (transform.up.y > startFlowAtY && isFlowing)
                {
                    lEmission.enabled = false;
                }
            }

            if (isFlowing)
            {
                doFlowLiquid();
            }
        }

        protected void doFlowLiquid()
        {
            float upwardYScale = (transform.up.y - startFlowAtY) / (maxFlowSpeedAtY - startFlowAtY);

            if(!infinityCapacity)
            {
                float reCap = Mathf.Lerp(minFlowSpeed, maxFlowSpeed, upwardYScale) * Time.deltaTime;

                capacity -= reCap;
                accumulateCapacity += reCap;
            }

            if (capacity <= 0)
            {
                capacity = 0;
            }

            while(accumulateCapacity >= releaseCapacity)
            {
                accumulateCapacity -= releaseCapacity;
                lParticle.Emit(1);
            }
        }
    }
}
