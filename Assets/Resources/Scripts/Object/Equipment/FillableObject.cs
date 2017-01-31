using chemistrecipe.element;
using ChemistRecipe.Experiment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistRecipe.AppObject
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Collider), typeof(ParticleSystem))] // Require a fillable collider area
    public class FillableObject : Equipment
    {
        public enum FlowType
        {
            LIQUID,
            POWDER,
            SOLID
        }

        #region Settings & States
        
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

        // Flow Setting
        [Header("Flow Setting")]
        [Tooltip("Particle System for generate water flow")]
        public ParticleSystem liquidParticleSystem;
        [Tooltip("Can it flow?")]
        public bool enableFlow = true;
        [Tooltip("How to visual object flow?")]
        public FlowType flowType = FlowType.LIQUID;
        [ShowOnly]
        public float GreenAxisY = 0.0f;
        [Tooltip("Start flow when GreenAxis.Y < this value")]
        public float minSpeedFlowAtY = -0.1f;
        [Tooltip("Maxinum flow speed when GreenAxis.Y <= this value")]
        public float maxSpeedFlowAtY = -1.0f;
        [Tooltip("Minimum speed flow (mL) per secound")]
        public float minimumFlowSpeed = 0.0f;
        [Tooltip("Maximum speed flow (mL) per secound")]
        public float maximumFlowSpeed = 10.0f;
        [Tooltip("Speed of particle (when emitted per object)")]
        public float particleSpeed = 1.5f;

        // Flow State
        public bool isFlowing = false;
        private float accumulateCapacity = 0f;
        private float releaseCapacity = 0.25f;

        // Liquid Setting
        [Header("Liquid Setting")]
        [Tooltip("Liquid Color")]
        public Color liquidColor = Color.white;

        // For Debugging
        [Header("Debugging")]
        public bool infinityCapacity = false;
        public TextMesh debugText = null;

        #endregion

        #region Internal Only

        // ParticleSystem
        private ParticleSystem.MainModule lParticleMain;
        private ParticleSystem.EmissionModule lEmission;

        // Settings
        private float oldParticleSpeed = 0f;
        private Color oldLiquidColor = Color.white;

        #endregion

        void Start()
        {
            lParticleMain = liquidParticleSystem.main;
            lEmission = liquidParticleSystem.emission;
            lEmission.rateOverTimeMultiplier = 0; // Disable automatic emitting particle
        }

        void Update()
        {
#if UNITY_EDITOR

            if (Application.isEditor)
            {
                Start();
            }

            // Update GreenAxisY Inspector

            GreenAxisY = transform.up.y;
#endif

            UpdateProperty();

            if (debugText != null)
            {
                debugText.text = capacity.ToString();
            }

            if (Application.isPlaying)
            {
                if (!isContainLiquid && !infinityCapacity)
                {
                    isFlowing = false;
                }
                else
                {
                    if (transform.up.y <= minSpeedFlowAtY && !isFlowing)
                    {
                        isFlowing = true;
                    }
                    else if (transform.up.y > minSpeedFlowAtY && isFlowing)
                    {
                        isFlowing = false;
                        accumulateCapacity = 0f; // Reset accumulate
                    }
                }

                if (isFlowing && enableFlow)
                {
                    flowLiquid();
                }
            }
        }

        private void UpdateProperty()
        {
            if(oldParticleSpeed != particleSpeed)
            {
                lParticleMain.startSpeed = new ParticleSystem.MinMaxCurve(particleSpeed);
                oldParticleSpeed = particleSpeed;
            }

            if(oldLiquidColor != liquidColor)
            {
                lParticleMain.startColor = new ParticleSystem.MinMaxGradient(liquidColor);
                oldLiquidColor = liquidColor;
            }
        }

        protected void flowLiquid()
        {
            float upwardYScale = (transform.up.y - minSpeedFlowAtY) / (maxSpeedFlowAtY - minSpeedFlowAtY);
            float releaseCap = Mathf.Lerp(minimumFlowSpeed, maximumFlowSpeed, upwardYScale) * Time.deltaTime;
            accumulateCapacity += releaseCap;

            while(accumulateCapacity >= releaseCapacity)
            {
                if (!infinityCapacity)
                {
                    capacity -= releaseCapacity;
                }
                
                accumulateCapacity -= releaseCapacity;

                liquidParticleSystem.Emit(1);
            }

            if (capacity <= 0)
            {
                capacity = 0;
            }
        }
    }
}
