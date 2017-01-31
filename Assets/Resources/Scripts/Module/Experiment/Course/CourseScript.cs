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
        public struct TrackerEquipmentMapping
        {
            [ShowOnly]
            public string trackerName;
            public Equipment equipmentObject;
        }

        #region Course setting properties

        public string Name;
        public string Author;
        public string Version;

        public string baseTrackerName = "";
        public TrackerEquipmentMapping[] trackersMapping;

        #endregion

        #region Internal

        private CourseBehaviour courseBehaviour;

        #endregion

        void Start()
        {
            if(!checkCourseBehaviour())
            {
                if (Application.isPlaying) ChemistRecipeApp.Exit();
            }
        }

        public bool checkCourseBehaviour()
        {
            // Check CourseBehaviour
            if (!(courseBehaviour = GetComponent<CourseBehaviour>()))
            {
                Debug.LogError("CourseScript component must in the same gameObject of CourseBehaviour.");
                return false;
            }

            return true;
        }

        public void UpdateTrackerList()
        {
            if(checkCourseBehaviour())
            {
                trackersMapping = new TrackerEquipmentMapping[courseBehaviour.DatasetNameList.Count];

                int counter = 0;
                foreach (string name in courseBehaviour.DatasetNameList)
                {
                    trackersMapping[counter++].trackerName = name;
                }
            }
        }

        public Equipment getObjectOfTracker(string name)
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

        public abstract void SetupCoruse();
        public abstract void FinishCourse();

    }
}
