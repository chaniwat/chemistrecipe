using chemistrecipe.element;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chemistrecipe
{
    public class FillableObject : MonoBehaviour
    {

        // TODO Accumulate liquid flowing (with Liquid Particle)

        /* Settings & States */

        // Container Setting
        [Header("Container Setting")]
        [Tooltip("Max capacity to store a liquid (metric: mL)")]
        public float maxCapacity = 0.0f;
        [Tooltip("Initial capacity, current capacity (metric: mL)")]
        public float capacity = 0.0f;

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

        private Liquid liquid
        {
            get { return new MockLiquid(liquidName, liquidColor); }
        }

        // Flow Setting
        [Header("Flow Setting")]
        [Tooltip("Start flow when forward.y < this value")]
        public float startFlowAtY = -0.1f;
        [Tooltip("Minimum speed flow (mL) per secound (forward.y = startFlowAtY)")]
        public float minFlowSpeed = 0.0f;
        [Tooltip("Maximum speed flow (mL) per secound (forward.y = -1.0f)")]
        public float maxFlowSpeed = 10.0f;
        [Tooltip("Maxinum flow speed when forward.y <= this value")]
        public float maxFlowSpeedAtY = -1.0f;

        // Flow State
        private bool _isFlowing = false;
        public bool isFlowing
        {
            get { return _isFlowing; }
        }

        /* End Settings & States */
        /* --------------------- */
        /* Internal */

        // ParticleSystem
        private ParticleSystem lParticle;
        private ParticleSystem.EmissionModule lEmission;
        private float lEmissionBaseRateMultiplyer;

        // Text
        private TextMesh tText;

        // Use this for initialization
        void Start()
        {
            // Setting Particle System
            lParticle = GetComponentInChildren<ParticleSystem>();

            var lParticleMain = lParticle.main;
            lEmission = lParticle.emission;
            lEmissionBaseRateMultiplyer = lEmission.rateOverTimeMultiplier;

            lParticleMain.startColor = new ParticleSystem.MinMaxGradient(liquid.color, liquid.color);

            // Setting Text
            tText = GetComponentInChildren<TextMesh>();
            tText.text = capacity.ToString();
        }

        /* End Internal */

        /* For Debugging */
        [Header("Debugging")]
        public bool infinityCapacity = false;

        // Update is called once per frame
        void Update()
        {
            tText.text = capacity.ToString();

            if (!isContainLiquid)
            {
                lEmission.enabled = false;
                _isFlowing = false;
            }
            else
            {
                if (transform.forward.y < startFlowAtY)
                {
                    if (!_isFlowing)
                    {
                        lEmission.enabled = true;
                        _isFlowing = true;
                    }
                }
                else
                {
                    if (_isFlowing)
                    {
                        lEmission.enabled = false;
                        _isFlowing = false;
                    }
                }

                if (_isFlowing)
                {
                    doFlowLiquid();
                }
            }
        }

        protected void doFlowLiquid()
        {
            float forwardYScale = (transform.forward.y - startFlowAtY) / (maxFlowSpeedAtY - startFlowAtY);

            if(!infinityCapacity)
            {
                capacity -= Mathf.Lerp(minFlowSpeed, maxFlowSpeed, forwardYScale) * Time.deltaTime;
            }
            if (capacity <= 0)
            {
                capacity = 0;
            }

            lEmission.rateOverTimeMultiplier = lEmissionBaseRateMultiplyer * forwardYScale;
        }
    }
}
