using chemistrecipe.button;
using chemistrecipe.data;
using ChemistRecipe;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace chemistrecipe.scene
{
    /// <summary>
    /// Control overall of Main Menu scene.
    /// </summary>
    public class MainMenuScript : MonoBehaviour
    {
        // Ref. GameObject
        [Header("Main Menu")]
        public GameObject MainMenuPanel;

        [Header("Course Selection")]
        public GameObject SelectCoursePanel;
        public GameObject CourseListGrid;
        public GameObject StarLevel;
        public GameObject PrintButton;
        public GameObject ProfilePanel;

        public GameObject TextScore;
        public GameObject TextCourseName;
        public GameObject TextCourseDescription;
        public Button TutorialButton;
        public Button PlayButton;

        [Header("Setting")]
        public GameObject SettingPanel;
        public Button confirmSetting;
        public InputField playerNameInput;
        public InputField playerAliasInput;
        public InputField playerUidInput;
        public Slider effectVolume;
        public Slider musicVolume;

        [Header("Course Image")]
        public Image courseImage;

        [Header("Course Image")]
        public Text playerName;
        public Text playerAlias;
        public Text playerLevel;
        public Image playerAvatar;

        // Constant
        private Course SelectedCourse;
        private string defaulDescription;
        private string courseURL;
        private string markerURL;

        private GlobalObject _Global;

        void Start()
        {
            // Get global object
            _Global = GameObject.Find("_Global").GetComponent<GlobalObject>();

            //setup player profile
            playerName.text = _Global.playerName;
            playerAlias.text = _Global.playerAlias;
            playerLevel.text = _Global.playerLevel;
            playerAvatar.sprite = _Global.playerAvatar;

            // Load CourseButton Prefab
            Object prefab = Resources.Load("Prefabs/UI/CourseButton");

            // Set up the Editor before calling into the realtime database.
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://chemresipe.firebaseio.com/");

            // Get the root reference location of the database.
            DatabaseReference courseReference = FirebaseDatabase.DefaultInstance.GetReference("courses");

            // Load Value
            courseReference.GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    // Handle the error... (no internet?)
                    Debug.LogError("Firebase : Something Wrong");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    // Do something with snapshot...
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        // Deserialize JSON into Object
                        Course data = JsonConvert.DeserializeObject<Course>(childSnapshot.GetRawJsonValue());
                        data.id = snapshot.Key;

                        // Create and Setup CourseButton
                        GameObject courseButton = (GameObject)Instantiate(prefab, CourseListGrid.transform);
                        courseButton.GetComponent<CourseButtonScript>().data = data;
                        courseButton.GetComponentInChildren<Text>().text = data.name;
                        markerURL = data.url.marker;
                        courseButton.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            SelectedCourse = data;
                            courseImage.overrideSprite = Resources.Load<Sprite>("Textures/090e0932aa78714276b66dd521019777");
                            updateSelectedCourse();
                        });
                    }
                }
            });

            // Get default description
            defaulDescription = TextCourseDescription.GetComponent<Text>().text;
            
        }

        private bool changeOpenTutorialState = false;

        void Update()
        {
            if (SelectedCourse == null)
            {
                PlayButton.interactable = false;
            }
            else if(SelectedCourse.name == "Creating Soap")
            {
                PlayButton.interactable = true;
            }

            // Check if change from tutorial
            if (!changeOpenTutorialState && _Global.isOpenTutorial)
            {
                SelectCoursePanel.SetActive(false);
                changeOpenTutorialState = true;
            }
            else if (changeOpenTutorialState && !_Global.isOpenTutorial)
            {
                SelectCoursePanel.SetActive(true);
                changeOpenTutorialState = false;
            }
        }

        // Call when change course
        protected void updateSelectedCourse()
        {
            if(SelectedCourse != null)
            {
                ProfilePanel.SetActive(false);
                StarLevel.GetComponent<Image>().enabled = true;
                PrintButton.GetComponent<Image>().enabled = true;
                PrintButton.GetComponentInChildren<Text>().enabled = true;
                PrintButton.GetComponent<Button>().interactable = true;
                TutorialButton.GetComponent<Image>().enabled = true;
                TutorialButton.GetComponentInChildren<Text>().enabled = true;
                TutorialButton.GetComponent<Button>().interactable = true;
                TextScore.GetComponent<Text>().enabled = true;
                TextCourseName.GetComponent<Text>().enabled = true;
                TextCourseName.GetComponent<Text>().text = SelectedCourse.name;
                TextCourseDescription.GetComponent<Text>().text = SelectedCourse.description;

                // TODO check if selected course's assetBundle is downloaded?
            }
            else
            {
                ProfilePanel.SetActive(true);
                StarLevel.GetComponent<Image>().enabled = false;
                PrintButton.GetComponent<Image>().enabled = false;
                PrintButton.GetComponentInChildren<Text>().enabled = false;
                PrintButton.GetComponent<Button>().interactable = false;
                TutorialButton.GetComponent<Image>().enabled = false;
                TutorialButton.GetComponentInChildren<Text>().enabled = false;
                TutorialButton.GetComponent<Button>().interactable = false;
                TextScore.GetComponent<Text>().enabled = false;
                TextCourseName.GetComponent<Text>().enabled = false;
                TextCourseDescription.GetComponent<Text>().text = defaulDescription;
                
            }
        }

        // Click by enter class button
        public void OnClickEnterClass()
        {
            MainMenuPanel.SetActive(false);
            SelectCoursePanel.SetActive(true);
            updateSelectedCourse();
        }

        // Click by exit button
        public void OnClickExit()
        {
            ChemistRecipeApp.Exit();
        }

        // Click by back button
        public void OnClickBackBtn()
        {
            MainMenuPanel.SetActive(true);
            SelectCoursePanel.SetActive(false);

            SelectedCourse = null;
            courseImage.overrideSprite = Resources.Load<Sprite>("Sprites/color_background2");
        }

        // Click by download button
        public void OnClickDownload()
        {
            string url = "https://firebasestorage.googleapis.com/v0/b/chemresipe.appspot.com/o/Marker%2FMarker-course1.pdf?alt=media&token=ef874247-eb59-4c7e-8a3e-68d448349153";
            
            Application.OpenURL(markerURL);
        }

        public void OnClickTutorial()
        {
            SceneManager.LoadScene(3, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
            _Global.isOpenTutorial = true;
        }

        // Click by play button
        public void OnClickPlayBtn()
        {
            Debug.Log(JsonConvert.SerializeObject(SelectedCourse));

            if (SelectedCourse.name == "Creating Soap")
            {
                SceneManager.LoadScene(1);
            }
        }

        //Click logo in select course panel
        public void OnclickLogo()
        {
            SelectedCourse = null;
            ProfilePanel.SetActive(true);
            courseImage.overrideSprite = Resources.Load<Sprite>("Textures/color_background2");
            StarLevel.GetComponent<Image>().enabled = false;
            PrintButton.GetComponent<Image>().enabled = false;
            PrintButton.GetComponentInChildren<Text>().enabled = false;
            PrintButton.GetComponent<Button>().interactable = false;
            TutorialButton.GetComponent<Image>().enabled = false;
            TutorialButton.GetComponentInChildren<Text>().enabled = false;
            TutorialButton.GetComponent<Button>().interactable = false;
            TextScore.GetComponent<Text>().enabled = false;
            TextCourseName.GetComponent<Text>().enabled = false;
            TextCourseDescription.GetComponent<Text>().text = defaulDescription;
            
        }

        public void OnclickSetting() {

            SettingPanel.SetActive(true);
            
            playerNameInput.GetComponentInChildren<Text>().text = _Global.playerName;
            playerAliasInput.GetComponentInChildren<Text>().text = _Global.playerAlias;
            playerUidInput.GetComponentInChildren<Text>().text = _Global.playerUid;

            // Add listener to inputfield
            playerNameInput.onEndEdit.AddListener(SubmitPlayerName);
            playerAliasInput.onEndEdit.AddListener(SubmitPlayerAlias);
            playerUidInput.onEndEdit.AddListener(SubmitPlayerUid);

            MainMenuPanel.SetActive(false);
        }

        public void OnclickConfirmSetting() {

            SettingPanel.SetActive(false);
            
            /*
            _Global.playerName = playerNameInput.transform.Find("Text").GetComponent<Text>().text;
            _Global.playerAlias = playerAliasInput.transform.Find("Text").GetComponent<Text>().text;
            _Global.playerUid = playerUidInput.transform.Find("Text").GetComponent<Text>().text;
            */
            
            playerName.text = _Global.playerName;
            playerAlias.text = _Global.playerAlias;
            playerLevel.text = _Global.playerLevel;
            playerAvatar.sprite = _Global.playerAvatar;

            MainMenuPanel.SetActive(true);
        }

        public void SubmitPlayerName(string name) {
            if ( name.Length > 0 ) {
                _Global.playerName = name;
                Debug.Log("Saved name \" " + _Global.playerName + " \"");
            }
            
        }

        public void SubmitPlayerAlias(string alias)
        {
            if ( alias.Length < 0) {
                _Global.playerAlias = alias;
                Debug.Log("Saved alias \" " + _Global.playerAlias + " \"");
            }
            
        }

        public void SubmitPlayerUid(string uid)
        {
            if (uid.Length > 0) {
                _Global.playerUid = uid;
                Debug.Log("Saved uid \" " + _Global.playerUid + " \"");
            }
            
        }

    }
}