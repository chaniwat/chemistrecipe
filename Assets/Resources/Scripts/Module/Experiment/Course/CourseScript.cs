using ChemistRecipe.AR;
using System;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace ChemistRecipe.Experiment
{
    [ExecuteInEditMode]
    public abstract class CourseScript : MonoBehaviour
    {
        [Serializable]
        public class TrackerEquipmentMapping
        {
            [ShowOnly]
            public string trackerName = "";
            public Equipment equipmentObject = null;
            public bool canFilp = true;
            public float zOffset = 0.0f;
        }

        #region Course setting properties

        public string Name;
        public string Author;
        public string Version;

        public string baseTrackerName = "";
        public TrackerEquipmentMapping[] trackersMapping;

        #endregion

        #region Internal

        private CourseBehaviour _courseBehaviour;
        private Dictionary<string, Equipment> equipmentMap = null;

        public CourseBehaviour courseBehaviour
        {
            get
            {
                return _courseBehaviour;
            }
        }

        #endregion

        #region Life-Cycle control by CourseBehaviour and UIs

        public void setup()
        {
            checkCourseBehaviour();
            SetupCoruse();
        }

        public void update()
        {
            UpdateCoruse();
        }

        public void restart()
        {
            // Re-Initial Equipment
            foreach (Equipment equipment in GetAllEquipment().Values)
            {
                if (equipment is FillableEquipment)
                {
                    ((FillableEquipment)equipment).InitialEquipment();
                }
            }

            RestartCoruse();
        }

        public void finish()
        {
            FinishCourse();
        }

        #endregion

        /// <summary>
        /// Check CourseBehaviour component
        /// </summary>
        public bool checkCourseBehaviour()
        {
            // Check CourseBehaviour
            if (!(_courseBehaviour = GetComponent<CourseBehaviour>()))
            {
                Debug.LogError("CourseScript component must in the same gameObject of CourseBehaviour.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Call when need to update the tracker list (load new dataset)
        /// </summary>
        public void UpdateTrackerList()
        {
            if(checkCourseBehaviour())
            {
                trackersMapping = new TrackerEquipmentMapping[_courseBehaviour.DatasetNameList.Count];

                int counter = 0;
                foreach (string name in _courseBehaviour.DatasetNameList)
                {
                    trackersMapping[counter] = new TrackerEquipmentMapping();
                    trackersMapping[counter++].trackerName = name;
                }
            }
        }

        /// <summary>
        /// Create a new equipment dictionary (Internal only)
        /// </summary>
        protected void createEquipmentDict()
        {
            equipmentMap = new Dictionary<string, Equipment>();

            foreach (TrackerEquipmentMapping o in trackersMapping)
            {
                if (o.equipmentObject == null) continue;
                equipmentMap.Add(o.equipmentObject.name, o.equipmentObject);
            }
        }

        /// <summary>
        /// Get all attach equipments
        /// </summary>
        public Dictionary<string, Equipment> GetAllEquipment()
        {
            if (equipmentMap == null)
            {
                createEquipmentDict();
            }

            return equipmentMap;
        }

        /// <summary>
        /// Get equipment by gameObject.name
        /// </summary>
        public Equipment GetEquipmentByObjectName(string name)
        {
            if (equipmentMap == null)
            {
                createEquipmentDict();
            }

            return equipmentMap[name];
        }

        /// <summary>
        /// Get the attach object (equipment) of tracker
        /// </summary>
        public Equipment GetTrackerAttachObject(string name)
        {
            foreach(TrackerEquipmentMapping o in trackersMapping)
            {
                if(o.trackerName == name)
                {
                    return o.equipmentObject;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the parameter setting (TrackingImage's property) of tracker
        /// </summary>
        public TrackingImage.TrackingImageParam GetTrackerParameter(string name)
        {
            TrackingImage.TrackingImageParam param = new TrackingImage.TrackingImageParam();

            foreach (TrackerEquipmentMapping o in trackersMapping)
            {
                if (o.trackerName == name)
                {
                    param.canFilp = o.canFilp;
                    param.filpZOffset = o.zOffset;

                    break;
                }
            }

            return param;
        }

        protected abstract void SetupCoruse();
        protected abstract void UpdateCoruse();
        protected abstract void RestartCoruse();
        protected abstract void FinishCourse();

        // TODO register raycasting camera hit gameObject handler

    }
}
