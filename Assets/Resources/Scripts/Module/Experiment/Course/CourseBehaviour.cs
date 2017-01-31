using ChemistRecipe.AR;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;
using Vuforia;

namespace ChemistRecipe.Experiment
{
    [ExecuteInEditMode]
    public class CourseBehaviour : MonoBehaviour
    {
        public string DatasetName;
        public CourseScript CourseScript;
        public Dictionary<string, TrackingImage> trackers;

        #region Vuforia variables

        // AR Controller
        private VuforiaARController mVuforiaArController;

        // Dataset
        private const string PATH_TO_DATASET = "/QCAR/";
        public DatasetExistance DatasetExistanceStatus = DatasetExistance.NOT_CHECK;
        public List<string> DatasetNameList;
        private DataSet _Dataset;

        #endregion

        /// <summary>
        /// Unity Awake()
        /// Call before Start() (At first)
        /// </summary>
        void Awake()
        {
            checkCourseScript();

            mVuforiaArController = VuforiaARController.Instance;
            
            mVuforiaArController.RegisterVuforiaInitializedCallback(() =>
            {
                if(CheckDataset())
                {
                    mVuforiaArController.SetWorldCenterMode(VuforiaARController.WorldCenterMode.SPECIFIC_TARGET);
                }
                else
                {
                    Debug.LogError("Error: Dataset not found");
                    Application.Quit();
                }
            });

            mVuforiaArController.RegisterVuforiaStartedCallback(() =>
            {
                ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

                _Dataset = objectTracker.CreateDataSet();

                if (_Dataset.Load(PathToDataset, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
                {
                    objectTracker.Stop();  // stop tracker so that we can add new dataset

                    if (!objectTracker.ActivateDataSet(_Dataset))
                    {
                        // Note: ImageTracker cannot have more than 100 total targets activated
                        Debug.Log("<color=yellow>Failed to Activate DataSet: " + DatasetName + "</color>");
                    }

                    if (!objectTracker.Start())
                    {
                        Debug.Log("<color=yellow>Tracker Failed to Start.</color>");
                    }

                    int counter = 0;

                    trackers = new Dictionary<string, TrackingImage>();

                    IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
                    foreach (TrackableBehaviour tb in tbs)
                    {
                        if (tb.name == "New Game Object")
                        {
                            // change generic name to include trackable name
                            tb.gameObject.name = ++counter + ":DynamicImageTarget-" + tb.TrackableName;

                            // add additional script components for trackable
                            tb.gameObject.AddComponent<TurnOffBehaviour>();
                            TrackingImage ti = tb.gameObject.AddComponent<TrackingImage>();

                            // Set specific center tracker
                            if(tb.TrackableName == CourseScript.baseTrackerName)
                            {
                                mVuforiaArController.SetWorldCenter(tb);
                            }

                            // add tracker in dictionary
                            trackers.Add(tb.TrackableName, ti);

                            // Attach object to the child of tracker (from coursescript)
                            Equipment equipment = null;
                            if (equipment = CourseScript.getObjectOfTracker(tb.TrackableName))
                            {
                                ti.attachObject = equipment;
                                equipment.transform.SetParent(tb.transform);
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("<color=yellow>Failed to load dataset: '" + DatasetName + "'</color>");
                }
            });
        }

        /// <summary>
        /// Unity Start()
        /// Call when object initialize (After Awake())
        /// </summary>
        void Start()
        {
            
        }

        /// <summary>
        /// Unity Update()
        /// Call each frames
        /// </summary>
        void Update()
        {
            checkCourseScript();
        }

        /// <summary>
        /// Unity OnDestroy()
        /// Call when object going to be destroyed
        /// </summary>
        void OnDestroy()
        {

        }

        /// <summary>
        /// Check CourseScript is attach to component, exit app if not
        /// </summary>
        public void checkCourseScript()
        {
            if (CourseScript == null)
            {
                CourseScript script = GetComponent<CourseScript>();
                if (script)
                {
                    CourseScript = script;
                }
                else
                {
                    Debug.LogError("CourseScript not found in component.");
                    if (Application.isPlaying) ChemistRecipeApp.Exit();
                }
            }
        }

        #region Dataset Management

        private string oldDatasetName = "";
        private bool needDatasetNameListRefresh = false;

        /// <summary>
        /// Check existance of dataset (Editor Only)
        /// </summary>
        public bool CheckDataset()
        {
            if (DatasetName == null)
            {
                Debug.LogError("Dataset name not set!");
                return false;
            }
            else
            {
                if (oldDatasetName != DatasetName)
                {
                    if (!DataSet.Exists(PathToDataset, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
                    {
                        Debug.LogError("Dataset not found!");
                        return false;
                    }

                    oldDatasetName = DatasetName;
                    needDatasetNameListRefresh = true;
                }
            }
            
            return true;
        }

        public void loadDatasetNameList()
        {
            if (needDatasetNameListRefresh)
            {
                DatasetNameList = new List<string>();

                XmlDocument datasetXML = new XmlDocument();
                datasetXML.Load(PathToDataset);

                XmlNodeList imageTargets = datasetXML.GetElementsByTagName("ImageTarget");

                foreach (XmlNode node in imageTargets)
                {
                    DatasetNameList.Add(node.Attributes["name"].Value);
                }

                if (CourseScript)
                {
                    CourseScript.UpdateTrackerList();
                }

                needDatasetNameListRefresh = false;
            }
        }

        public void resetDatasetNameList()
        {
            if (needDatasetNameListRefresh)
            {
                DatasetNameList = new List<string>();
                CourseScript.UpdateTrackerList();

                needDatasetNameListRefresh = false;
            }
        }

        /// <summary>
        /// Return absolute string path to dataset of this setting course
        /// </summary>
        public string PathToDataset
        {
            get
            {
                return Application.dataPath + "/Bundles/Courses/" + Regex.Replace(CourseScript.Name, @"\s+", "") + PATH_TO_DATASET + DatasetName + ".xml";
            }
        }

        #endregion

        public enum DatasetExistance
        {
            NOT_CHECK,
            FOUND,
            NOT_FOUND
        }

    }
}
