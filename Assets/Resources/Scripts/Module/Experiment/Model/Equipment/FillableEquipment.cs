using Chemistrecipe.Experiment.Utility;
using ChemistRecipe.Mechanism;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChemistRecipe.Experiment
{
    [ExecuteInEditMode]
    public class FillableEquipment : Equipment
    {

        #region Settings & States

        // Container variable, struct
        [Serializable]
        public struct MaterialVolumeMapping
        {
            public Material material;
            public Volume volume;
        }

        // Model Setting
        [Header("Model Setting")]
        public float scale = 1f;

        // Container Setting
        [Header("Container Setting")]
        [Tooltip("Max capacity to store materials")]
        public float maximumCapacity = 0f;
        [Tooltip("Base metric of this container")]
        public Volume.Metric metric = Volume.Metric.ANY;
        [Tooltip("Fillable area (Collider3D)")]
        public Collider FillableArea;
        [Tooltip("Liquid Mesh (Liquid Generator)")]
        public CylinderGenerator LiquidMesh;
        [Tooltip("Initial material of this container")]
        public MaterialVolumeMapping[] initialMaterials;

        // Container State
        protected Dictionary<Material, Volume> _Materials;
        public Dictionary<Material, Volume> Materials
        {
            get
            {
                return _Materials;
            }
        }
        public float currentCapacity
        {
            get
            {
                float totalCapacity = 0f;
                foreach (KeyValuePair<Material, Volume> pair in _Materials)
                {
                    totalCapacity += pair.Value.volume;
                }
                return totalCapacity;
            }
        }
        public bool isContainMaterial
        {
            get {
                return _Materials.Count > 0;
            }
        }

        // Flow variable, struct
        public enum FlowBaseAxis
        {
            GREEN,
            BLUE,
            RED
        }

        public enum FlowType
        {
            LIQUID,
            POWDER,
            SOLID
        }

        // Flow Setting
        [Header("Flow Setting")]
        [Tooltip("Particle System for generate water flow")]
        public ParticleSystem liquidParticleSystem;
        [Tooltip("Can it flow?")]
        public bool enableFlow = true;
        [Tooltip("How to visual object flow?")]
        public FlowType flowType = FlowType.LIQUID;
        [Tooltip("Base of axis to calculate flowing (Default is Green Axis)")]
        public FlowBaseAxis flowBaseAxis = FlowBaseAxis.GREEN;
        [ShowOnly]
        public float BaseAxisY = 0.0f;
        [Tooltip("Minimum flow when BaseAxis.Y < this value")]
        public float minSpeedFlowAtY = 0f;
        [Tooltip("Maximum flow speed when BaseAxis.Y <= this value")]
        public float maxSpeedFlowAtY = -1.0f;
        [Tooltip("Minimum speed flow value per secound")]
        public float minimumFlowSpeed = 0.0f;
        [Tooltip("Maximum speed flow value per secound")]
        public float maximumFlowSpeed = 0.0f;
        [Tooltip("Shoot speed of particle (when emitted the particle)")]
        public float flowShootSpeed = 1.5f;

        // Flow State
        public bool isPouring
        {
            get
            {
                if (!isContainMaterial)
                {
                    return false;
                }
                else
                {
                    if (BaseAxisY <= minSpeedFlowAtY && enableFlow)
                    {
                        return true;
                    }
                    else
                    {
                        accumulateCapacity = 0f; // Reset accumulate
                        return false;
                    }
                }
            }
        }
        private float accumulateCapacity = 0f;
        private float releaseCapacity = 0.25f;

        // Particle Setting
        [Header("Particle Setting")]
        [Tooltip("Particle Color")]
        public Color particleColor = Color.white;
        [Tooltip("Particle Size")]
        public float particleSize = 0.5f;

        // For Debugging
        [Header("Debugging")]
        public bool infinityCapacity = false;
        public TextMesh debugText = null;

        #endregion

        // TODO Highlight flag
        public bool highlighting = false;

        #region Internal Only

        // ParticleSystem
        private ParticleSystem.MainModule lParticleMain;
        private ParticleSystem.EmissionModule lEmission;

        // Pour buffer
        private Dictionary<uint, FlowParticle.FlowParticleParam> pourBuffer;
        private uint pourCounter = 0;

        // Stir
        private float _stirAmplifier = 1f;
        public float stirAmplifier
        {
            get
            {
                return _stirAmplifier;
            }
        }

        #endregion

        #region Action handler

        /// <summary>
        /// Called before check should pouring in Update()
        /// </summary>
        public Action OnBeforeUpdate;
        /// <summary>
        /// Called after check should pouring in Update()
        /// </summary>
        public Action OnAfterUpdate;
        public Action OnBeforePour;
        public Action OnAfterPour;
        public Action<Material, Volume> OnBeforeFill;
        public Action<Material, Volume> OnAfterFill;
        public Action OnStir;

        #endregion

        #region Get and Set Material - Volume

        /// <summary>
        /// Get the material from this fillable equipment.
        /// </summary>
        public Material getMaterial(string materialName)
        {
            foreach (KeyValuePair<Material, Volume> pair in _Materials)
            {
                if (pair.Key.name == materialName)
                {
                    return pair.Key;
                }
            }

            return null;
        }

        /// <summary>
        /// Is the name of material contain in the fillable equipment.
        /// </summary>
        public bool ContainMaterial(string materialName)
        {
            return getMaterial(materialName) != null;
        }

        /// <summary>
        /// Remove the material from this fillable equipment.
        /// </summary>
        public bool removeMaterial(string materialName)
        {
            Material matTarget = getMaterial(materialName);

            if (matTarget != null)
            {
                _Materials.Remove(matTarget);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get volume of the material in this fillable equipment.
        /// </summary>
        public Volume getVolumeOfMaterial(Material material)
        {
            if (_Materials.ContainsKey(material))
            {
                return _Materials[material];
            }

            return null;
        }

        /// <summary>
        /// Get Volume of the material.
        /// </summary>
        public Volume GetVolumeOfMaterial(string materialName)
        {
            Material mat = getMaterial(materialName);
            if (mat != null)
            {
                return getVolumeOfMaterial(mat);
            }

            return null;
        }

        /// <summary>
        /// Set new volume of the material.
        /// </summary>
        public bool setVolumeOfMaterial(Material material, Volume newVolume)
        {
            if (_Materials.ContainsKey(material))
            {
                _Materials[material] = newVolume;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Set new volume of the material.
        /// </summary>
        public bool setVolumeOfMaterial(string materialName, Volume newVolume)
        {
            Material mat = getMaterial(materialName);
            
            if(mat != null)
            {
                return setVolumeOfMaterial(mat, newVolume);
            }

            return false;
        }

        #endregion

        #region Get pour buffer

        public FlowParticle.FlowParticleParam GetParticleData(ParticleSystem.Particle particle)
        {
            return pourBuffer[particle.randomSeed];
        }

        #endregion

        #region Fillable Action

        /// <summary>
        /// fill material into equipment.
        /// </summary>
        public void Fill(Material material, Volume volume)
        {
            if (OnBeforeFill != null) OnBeforeFill(material, volume);

            /*
            // Check capacity
            float currentCap = currentCapacity;
            if (currentCap + volume.volume > maximumCapacity)
            {
                volume.volume = maximumCapacity - volume.volume;
            }
            */

            // Add volume if exist, if not then add new material and volume to it
            Volume oldVolume = GetVolumeOfMaterial(material.name);
            if (oldVolume != null)
            {
                oldVolume.volume += volume.volume;
            }
            else
            {
                _Materials.Add(material, volume);
            }

            if (OnAfterFill != null) OnAfterFill(material, volume);
        }

        /// <summary>
        /// Pour materials.
        /// </summary>
        public void Pour()
        {
            if(OnBeforePour != null) OnBeforePour();

            if(_Materials.Count > 0)
            {
                // Get first of contain material
                KeyValuePair<Material, Volume> pair = _Materials.First();

                // If not infinity capacity, reduce volume
                if (!infinityCapacity)
                {
                    pair.Value.volume -= releaseCapacity;

                    // If volume below zero, remove material
                    if (pair.Value.volume <= 0f)
                    {
                        _Materials.Remove(pair.Key);
                    }
                }

                // Emit particle (water drop) & Add to buffer for seaching custom data per-particle
                ParticleSystem.EmitParams param = new ParticleSystem.EmitParams();
                param.randomSeed = pourCounter;
                liquidParticleSystem.Emit(param, 1);
                pourBuffer.Add(pourCounter++, new FlowParticle.FlowParticleParam() { material = pair.Key, volume = new Volume(releaseCapacity, pair.Value.metric) });
            }

            if (OnAfterPour != null) OnAfterPour();
        }

        /// <summary>
        /// Stir this fillable equipment.
        /// </summary>
        public void Stir()
        {
            _stirAmplifier += 0.85f;

            if (_stirAmplifier > 25f)
            {
                _stirAmplifier = 25f;
            }

            if (OnStir != null) OnStir();
        }

        #endregion

        /// <summary>
        /// Start()
        /// </summary>
        void Start()
        {
            // Setting default PS
            lParticleMain = liquidParticleSystem.main;
            lEmission = liquidParticleSystem.emission;
            lEmission.rateOverTimeMultiplier = 0; // Disable automatic emitting particle

            InitialEquipment();
        }

        /// <summary>
        /// Update on every frame
        /// </summary>
        void Update()
        {
            if (ChemistRecipeApp.isEditing)
            {
                Start();
            }

            UpdateDebug();
            UpdateProperty();

            if (!ChemistRecipeApp.isPlaying) return;

            if (OnBeforeUpdate != null) OnBeforeUpdate();
            
            #region reduce stirAmplifier over time if > 1f

            if (_stirAmplifier > 1f)
            {
                _stirAmplifier -= 2.6f * Time.deltaTime;
            }

            #endregion

            // Pour equipment
            if (isPouring)
            {
                float yScale = (BaseAxisY - minSpeedFlowAtY) / (maxSpeedFlowAtY - minSpeedFlowAtY);
                float releaseCap = Mathf.Lerp(minimumFlowSpeed, maximumFlowSpeed, yScale) * Time.deltaTime;
                accumulateCapacity += releaseCap;

                while (accumulateCapacity >= releaseCapacity)
                {
                    Pour();
                    accumulateCapacity -= releaseCapacity;
                }
            }

            if (OnAfterUpdate != null) OnAfterUpdate();
        }

        /// <summary>
        /// Update liquid mesh
        /// </summary>
        void LateUpdate()
        {
            if (!ChemistRecipeApp.isPlaying) return;

            // Update liquid mesh current Y (after Update)
            if (LiquidMesh)
            {
                LiquidMesh.currentYNormalize = currentCapacity / maximumCapacity;
                LiquidMesh.GetComponent<MeshRenderer>().material.color = particleColor;
            }
        }

        /// <summary>
        /// Update for debugging
        /// </summary>
        protected void UpdateDebug()
        {
            if (debugText != null)
            {
                debugText.text = currentCapacity.ToString("0.00");
            }
        }

        #region Old value setting
        
        private float oldFlowShootSpeed = 0f;
        private Color oldParticleColor = Color.white;
        private float oldParticleSize = 0f;

        #endregion

        /// <summary>
        /// Update property to components
        /// </summary>
        protected void UpdateProperty()
        {
            // Update BaseAxisY Inspector
            Vector3 baseAxisVector = new Vector3();
            switch (flowBaseAxis)
            {
                case FlowBaseAxis.GREEN: baseAxisVector = transform.up; break;
                case FlowBaseAxis.RED: baseAxisVector = transform.right; break;
                case FlowBaseAxis.BLUE: baseAxisVector = transform.forward; break;
            }
            BaseAxisY = baseAxisVector.y;

            // Update Scale
            transform.localScale = new Vector3(scale, scale, scale);

            if (oldFlowShootSpeed != flowShootSpeed)
            {
                lParticleMain.startSpeed = new ParticleSystem.MinMaxCurve(flowShootSpeed);
                oldFlowShootSpeed = flowShootSpeed;
            }

            if (oldParticleColor != particleColor)
            {
                lParticleMain.startColor = new ParticleSystem.MinMaxGradient(particleColor);
                oldParticleColor = particleColor;
            }

            if (oldParticleSize != particleSize)
            {
                lParticleMain.startSize = new ParticleSystem.MinMaxCurve(particleSize);
                oldParticleSize = particleSize;
            }
        }

        /// <summary>
        /// Initial the material from setting
        /// </summary>
        private void InitialMaterial()
        {
            _Materials = new Dictionary<Material, Volume>();
            if (initialMaterials.Length > 0)
            {
                foreach (MaterialVolumeMapping pair in initialMaterials)
                {
                    _Materials.Add(new Material(pair.material), new Volume(pair.volume));
                }
            }
        }

        /// <summary>
        /// Call when start course (or restart course)
        /// </summary>
        public void InitialEquipment()
        {
            // Initial pour buffer
            pourBuffer = new Dictionary<uint, FlowParticle.FlowParticleParam>();

            InitialMaterial();
        }

    }
}
