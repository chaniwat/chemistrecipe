using chemistrecipe.experiment.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chemistrecipe.experiment.model
{
    public abstract class FillableEquipment : Equipment
    {

        #region Fillable State

        protected Dictionary<Material, Volume> _materials = new Dictionary<Material, Volume>();
        public Dictionary<Material, Volume> materials
        {
            get
            {
                return _materials;
            }
        }

        public Volume MaximumCapacity;
        public Volume CurrentCapacity
        {
            get
            {
                float totalCapacity = 0f;

                foreach (KeyValuePair<Material, Volume> pair in _materials)
                {
                    if (pair.Value.metric != MaximumCapacity.metric)
                    {
                        totalCapacity += (
                            MaximumCapacity.metric == Volume.Metric.g ?
                            MeasurementUtils.convertToGram(pair.Value) :
                            MeasurementUtils.convertToMiliLitre(pair.Value)
                            ).volume;
                    }
                    else
                    {
                        totalCapacity += pair.Value.volume;
                    }
                }

                return new Volume(totalCapacity, MaximumCapacity.metric);
            }
        }

        #endregion

        #region Action handler

        public Action UpdateFunc = () => { };

        public Action BeforePourFunc = () => { };
        public Action AfterPourFunc = () => { };
        public Action BeforeFillFunc = () => { };
        public Action AfterFillFunc = () => { };

        public Action StirFunc = () => { };

        #endregion

        public FillableEquipment(string name, string description, Volume MaximumCapacity) : base(name, description)
        {
            this.MaximumCapacity = MaximumCapacity;
        }

        #region Get and Set Material - Volume

        /// <summary>
        /// Get the material from this fillable equipment.
        /// </summary>
        public Material getMaterial(string materialName)
        {
            foreach (KeyValuePair<Material, Volume> pair in _materials)
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
        public bool containMaterial(string materialName)
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
                _materials.Remove(matTarget);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get volume of the material in this fillable equipment.
        /// </summary>
        public Volume getVolumeOfMaterial(Material material)
        {
            if (_materials.ContainsKey(material))
            {
                return _materials[material];
            }

            return null;
        }

        /// <summary>
        /// Get Volume of the material.
        /// </summary>
        public Volume getVolumeOfMaterial(string materialName)
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
            if (_materials.ContainsKey(material))
            {
                _materials[material] = newVolume;
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

        #region Fillable Action

        /// <summary>
        /// Add new material to the fillable equipment.
        /// </summary>
        public void fill(Material material, Volume volume)
        {
            if(BeforeFillFunc != null) BeforeFillFunc();

            _materials.Add(material, volume);

            if(AfterFillFunc != null) AfterFillFunc();
        }

        /// <summary>
        /// Pour all materials to other fillable equipment.
        /// </summary>
        public void pour(FillableEquipment otherFillable)
        {
            if(BeforePourFunc != null) BeforePourFunc();

            while (_materials.Count > 0)
            {
                KeyValuePair<Material, Volume> pair = this._materials.First();
                otherFillable._materials.Add(pair.Key, pair.Value);
                this._materials.Remove(pair.Key);
            }

            if(AfterPourFunc != null) AfterPourFunc();
        }

        /// <summary>
        /// Stir this fillable equipment.
        /// </summary>
        public void Stir()
        {
            if(StirFunc != null) StirFunc();
        }

        /// <summary>
        /// Call Update on every frame (Update() in MonoBehaviour)
        /// </summary>
        public void Update()
        {
            if (UpdateFunc != null) UpdateFunc();
        }

        #endregion
    }
}
