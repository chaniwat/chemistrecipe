using chemistrecipe.localization;
using ChemistRecipe.AR;
using ChemistRecipe.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

namespace ChemistRecipe.Experiment
{
    [ExecuteInEditMode]
    public class CourseBehaviour : MonoBehaviour
    {
        public enum DatasetExistance
        {
            NOT_CHECK,
            FOUND,
            NOT_FOUND
        }

        public string DatasetName;
        public CourseScript CourseScript;
        public Dictionary<string, TrackingImage> trackers;
        public SceneController sceneController;
        public CourseUISoundController UISoundController;

        public TextMesh textMeshTemplate;
        public GameObject highlightPlaneTemplate;

        private GlobalObject _Global;
        public GlobalObject globalObject
        {
            get
            {
                return _Global;
            }
        }

        private bool _isFailState = false;
        public bool isFailState
        {
            get
            {
                return _isFailState;
            }
        }

        #region Vuforia variables

        // AR Controller
        private VuforiaARController mVuforiaArController;

        // Dataset
        private const string PATH_TO_DATASET = "/QCAR/";
        public DatasetExistance DatasetExistanceStatus = DatasetExistance.NOT_CHECK;
        public List<string> DatasetNameList;
        private DataSet _Dataset;

        #endregion

        #region Course variables

        public Text timerText;
        private float runTimer = 0;
        public float currentCourseTime
        {
            get
            {
                return runTimer;
            }
        }

        #endregion

        #region Internal

        private Camera playCamera;

        #endregion

        /// <summary>
        /// Unity Awake()
        /// Call before Start() (At first)
        /// </summary>
        void Awake()
        {
            checkCourseScript();

            mVuforiaArController = VuforiaARController.Instance;
            mVuforiaArController.RegisterVuforiaStartedCallback(StartVuforia);
        }

        private void StartVuforia()
        {
            if (CheckDataset())
            {
                mVuforiaArController.SetWorldCenterMode(VuforiaARController.WorldCenterMode.SPECIFIC_TARGET);
            }
            else
            {
                Debug.LogError("Error: Dataset not found");
                Application.Quit();
            }

            bool focusModeSet = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

            if (!focusModeSet)
            {
                Debug.Log("Failed to set focus mode (unsupported mode).");
            }

            ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

            objectTracker.Stop();  // stop tracker so that we can add new dataset
            objectTracker.DestroyAllDataSets(false);

            _Dataset = objectTracker.CreateDataSet();

            //if (_Dataset.Load(PathToDataset, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
            if (_Dataset.Load(DatasetName))
            {
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
                    }

                    // add additional script components for trackable
                    tb.gameObject.AddComponent<TurnOffBehaviour>();
                    TrackingImage ti = tb.gameObject.AddComponent<TrackingImage>();

                    // Init 2 gameObject (TextMesh & HighlightPlane)
                    TextMesh tmObject = Instantiate(textMeshTemplate, tb.gameObject.transform);
                    GameObject highlightPlane = Instantiate(highlightPlaneTemplate, tb.gameObject.transform);
                    highlightPlane.GetComponent<MeshRenderer>().enabled = false;

                    ti.textMesh = tmObject;
                    ti.HighlightPlaneObject = highlightPlane;

                    // Set specific center tracker
                    if (tb.TrackableName == CourseScript.baseTrackerName)
                    {
                        mVuforiaArController.SetWorldCenter(tb);
                    }

                    // add tracker in dictionary
                    if (trackers.ContainsKey(tb.TrackableName))
                    {
                        trackers[tb.TrackableName] = ti;
                    }
                    else
                    {
                        trackers.Add(tb.TrackableName, ti);
                    }

                    // Attach object to the child of tracker (from coursescript), mean that tracker used in course
                    Equipment equipment = null;
                    if (equipment = CourseScript.GetTrackerAttachObject(tb.TrackableName))
                    {
                        ti.attachObject = equipment;
                        TrackingImage.TrackingImageParam param = CourseScript.GetTrackerParameter(tb.TrackableName);
                        ti.canFilp = param.canFilp;
                        ti.filpXOffset = param.filpXOffset;
                        ti.filpYOffset = param.filpYOffset;
                        ti.filpZOffset = param.filpZOffset;
                        equipment.transform.SetParent(tb.transform);

                        ti.enableHighlightPlane = true;
                        ti.enableTextMesh = true;
                    }
                }

                int counter2 = 0;
                foreach (DataSet dataset in objectTracker.GetDataSets())
                {
                    counter2++;
                }
                Debug.Log("<color=red>Dataset Count : " + counter2 + "</color>");
            }
            else
            {
                Debug.LogError("<color=yellow>Failed to load dataset: '" + DatasetName + "'</color>");
            }
        }

        /// <summary>
        /// Unity Start()
        /// Call when object initialize (After Awake())
        /// </summary>
        void Start()
        {
            checkCourseScript();

            if (!ChemistRecipeApp.isPlaying) return;

            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://chemresipe.firebaseio.com/");

            _Global = GameObject.Find("_Global").GetComponent<GlobalObject>();
            playCamera = GameObject.Find("Camera").GetComponent<Camera>();

            // Setup gameResult
            _Global.gameResult = new GameResult();
            _Global.gameResult.courseId = "090e0932aa78714276b66dd521019777";
            //_Global.gameResult.uID = CalculateMD5Hash(new DateTime().Millisecond.ToString());
            _Global.gameResult.uID = _Global.playerUid;
            _Global.gameResult.data = new Score(_Global.playerName, 0, 0);

            _Global.isHighScore = false;
            _Global.isFastestTime = false;

            CourseScript.setup();
        }

        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        private GameObject previousHitObject;
        private bool disableFlag = false;

        /// <summary>
        /// Unity Update()
        /// Call each frames
        /// </summary>
        void Update()
        {
            if (ChemistRecipeApp.isEditing)
            {
                checkCourseScript();
            }

            if (!ChemistRecipeApp.isPlaying) return;

            LocalLanguage locale = globalObject.currentLocale;

            if (!isFailState)
            {
                runTimer += Time.deltaTime;

                int minute = (int)(runTimer / 60);
                float second = runTimer - (minute * 60);
                timerText.text = minute.ToString("00") + "." + second.ToString("00.00");

                CourseScript.update();
            }

            // Ray casting at cursor
            Ray ray = playCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                FillableEquipment hitEquipment;
                if ((hitEquipment = hit.collider.GetComponentInParent<FillableEquipment>()) != null)
                {
                    if (previousHitObject == null || hitEquipment.gameObject.name != previousHitObject.name)
                    {
                        previousHitObject = hitEquipment.gameObject;
                        sceneController.ShowStirButton(hitEquipment);
                        disableFlag = false;
                    }

                    // Update EquipmentDetail
                    string tempLocale = locale.getString("course.equipment.detail.temperature");
                    string updateBuffer1 = tempLocale+": ";

                    string volLocale = locale.getString("course.equipment.detail.volume");
                    string updateBuffer2 = volLocale+": " + hitEquipment.currentCapacity + "\n";
                    updateBuffer2 += "---\n";

                    float temp = 0f;
                    bool setTempFlag = false;

                    int counter = hitEquipment.Materials.Count;
                    foreach (KeyValuePair<Material, Volume> pair in hitEquipment.Materials)
                    {
                        if (!setTempFlag)
                        {
                            setTempFlag = true;
                            temp = pair.Value.tempature;
                        }

                        if (pair.Value.tempature > temp)
                        {
                            temp = pair.Value.tempature;
                        }

                        updateBuffer2 += "> " + pair.Key.name + " (" + pair.Value.volume.ToString("0.00") + pair.Value.metric.ToString() + ")";
                        if(counter > 1)
                        {
                            updateBuffer2 += "\n";
                            counter--;
                        }
                    }

                    updateBuffer1 += temp.ToString("0.00") + "c\n";
                    updateBuffer2.Remove(updateBuffer2.Length - 5, 4);

                    sceneController.ShowEquipmentDetail(updateBuffer1 + updateBuffer2);
                }
            }
            else if (!disableFlag)
            {
                sceneController.HideStirButton();
                sceneController.HideEquipmentDetail();
                previousHitObject = null;
                disableFlag = true;
            }
        }

        /// <summary>
        /// Unity OnDestroy()
        /// Call when object going to be destroyed
        /// </summary>
        void OnDestroy()
        {
            if (!ChemistRecipeApp.isPlaying) return;

            mVuforiaArController.UnregisterVuforiaStartedCallback(StartVuforia);
        }

        /// <summary>
        /// Restart Course
        /// </summary>
        public void RestartCourse()
        {
            runTimer = 0;
            _isFailState = false;
            sceneController.HideRestartButton();
            sceneController.HideFailDetail();
            sceneController.HideFinishButton();
            CourseScript.restart();
        }

        /// <summary>
        /// Finish Course
        /// </summary>
        public void FinishCourse()
        {
            CourseScript.finish();
        }

        /// <summary>
        /// Stop Course (Go to Main Menu)
        /// </summary>
        public void StopCourse()
        {
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Fail the course (need to restart | incorrect step)
        /// </summary>
        public void FailCourse(String text)
        {
            _isFailState = true;

            sceneController.ShowRestartButton();
            sceneController.ShowFailDetail(text);

            CourseScript.fail();
        }

        /// <summary>
        /// Submit the score
        /// </summary>
        public void SubmitScore(GameResult gameResult)
        {
            FirebaseDatabase.DefaultInstance.GetReference("courses")
                .Child(gameResult.courseId)
                .Child("scores")
                .Child(gameResult.uID)
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        // Handle the error... (no internet?)
                        Debug.LogError("Firebase : Something Wrong");
                    }
                    else if (task.Result.ChildrenCount == 0)
                    {
                        Debug.Log("No data to compare");
                        Debug.Log("CourseId = " + gameResult.courseId + " uID = " + gameResult.uID);
                        FirebaseDatabase.DefaultInstance.GetReference("courses")
                            .Child(gameResult.courseId)
                            .Child("scores")
                            .Child(gameResult.uID)
                            .SetRawJsonValueAsync(JsonConvert.SerializeObject(gameResult.data));
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        Score data = JsonConvert.DeserializeObject<Score>(snapshot.GetRawJsonValue());

                        GameResult forUpdate = new GameResult();
                        forUpdate.courseId = gameResult.courseId;
                        forUpdate.uID = gameResult.uID;
                        forUpdate.data = new Score(gameResult.data.name, data.time, data.score);

                        if (gameResult.data.score > data.score)
                        {
                            _Global.isHighScore = true;
                            forUpdate.data.score = gameResult.data.score;
                            forUpdate.data.time = gameResult.data.time;
                        }

                        if (gameResult.data.score == data.score && gameResult.data.time < data.time)
                        {
                            _Global.isFastestTime = true;
                            forUpdate.data.time = gameResult.data.time;
                        }

                        Debug.Log("GameResult :" + JsonConvert.SerializeObject(gameResult.data));
                        Debug.Log("Finalize :" + JsonConvert.SerializeObject(forUpdate.data));

                        FirebaseDatabase.DefaultInstance.GetReference("courses")
                            .Child(forUpdate.courseId)
                            .Child("scores")
                            .Child(forUpdate.uID)
                            .SetRawJsonValueAsync(JsonConvert.SerializeObject(forUpdate.data));
                    }
                    
                });
        }

        /// <summary>
        /// Check CourseScript is attach to component, exit app if not
        /// </summary>
        protected void checkCourseScript()
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
                    /*
                    if (!DataSet.Exists(PathToDataset, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
                    {
                        Debug.LogError("Dataset not found!");
                        return false;
                    }
                    */
                    if (!DataSet.Exists(DatasetName))
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

        /// <summary>
        /// Load trackers name list
        /// </summary>
        public void loadTrackerNameList()
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

        /// <summary>
        /// Clear all trackers name list
        /// </summary>
        public void resetTrackerNameList()
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
                //return Application.dataPath + "/StreamingAssets/Bundles/Courses/" + Regex.Replace(CourseScript.Name, @"\s+", "") + PATH_TO_DATASET + DatasetName + ".xml";
                return Application.dataPath + "/StreamingAssets" + PATH_TO_DATASET + DatasetName + ".xml";
            }
        }

        #endregion

        // TODO raycasting camera hit gameObject call CourseScript handler (for instruction message or additional UI prompt-up)

    }
}
