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
        
        public GameObject TextScore;
        public GameObject TextCourseName;
        public GameObject TextCourseDescription;
        public Button TutorialButton;
        public Button PlayButton;

        // Constant
        private Course SelectedCourse;
        private string defaulDescription;
        private string courseURL;

        void Start()
        {
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
                        courseButton.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            SelectedCourse = data;
                            updateSelectedCourse();
                        });
                    }
                }
            });

            // Get default description
            defaulDescription = TextCourseDescription.GetComponent<Text>().text;
        }

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
        }

        // Call when change course
        protected void updateSelectedCourse()
        {
            if(SelectedCourse != null)
            {
                StarLevel.GetComponent<Image>().enabled = true;
                PrintButton.GetComponent<Image>().enabled = true;
                PrintButton.GetComponentInChildren<Text>().enabled = true;
                PrintButton.GetComponent<Button>().interactable = true;
                TutorialButton.interactable = true;
                TextScore.GetComponent<Text>().enabled = true;
                TextCourseName.GetComponent<Text>().enabled = true;

                TextCourseName.GetComponent<Text>().text = SelectedCourse.name;
                TextCourseDescription.GetComponent<Text>().text = SelectedCourse.description;

                // TODO check if selected course's assetBundle is downloaded?
            }
            else
            {
                StarLevel.GetComponent<Image>().enabled = false;
                PrintButton.GetComponent<Image>().enabled = false;
                PrintButton.GetComponentInChildren<Text>().enabled = false;
                TutorialButton.interactable = false;
                PrintButton.GetComponent<Button>().interactable = false;
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
        }

        // Click by download button
        public void OnClickDownload()
        {
            string url = "https://firebasestorage.googleapis.com/v0/b/chemresipe.appspot.com/o/Marker%2FCreating%20Soap.pdf?alt=media&token=62e31e95-1eb8-4a15-8477-aca9d00f6401";
            Application.OpenURL(url);
            Debug.Log("Downloading url");
        }

        public void OnClickTutorial()
        {
            SceneManager.LoadScene(3);
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
    }
}