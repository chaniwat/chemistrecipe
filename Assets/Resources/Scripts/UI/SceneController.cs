using ChemistRecipe.Experiment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChemistRecipe.UI
{
    public class SceneController : MonoBehaviour
    {

        [Header("Course Behavior")]
        public CourseBehaviour courseBehaviour;

        [Header("Canvas References")]
        public Canvas PlayCanvas;
        public Canvas SidebarMenuCanvas;
        public Canvas CheckListCanvas;
        public Canvas DebugCanvas;
        public Canvas TutorialCanvas;

        [Header("Play Overlay")]
        public Image Cursor;
        public Button MenuButton;
        public Button FinishCourseButton;
        public Button RestartCourseButton;
        public Button StirButton;
        public Text TimerText;
        public Text InstructionText;
        public Text EquipmentDetailText;
        public Text FailDetailText;

        [Header("Menu Buttons")]
        public Button ResumeButton;
        public Button InstructionButton;
        public Button CheckListButton;
        public Button SettingButton;
        public Button RestartButton;
        public Button MainMenuButton;

        [Header("Checklist Buttons")]
        public Button CloseCheckListButton;

        // Internal
        private FillableEquipment currentHitEquipment = null;

        // Use this for initialization
        void Start()
        {
            SetupUI();
        }

        void SetupUI()
        {
            // Default hide sidebar canvas
            SidebarMenuCanvas.enabled = false;
            CheckListCanvas.enabled = false;
            TutorialCanvas.enabled = false;

            // Disable Stir & Finish Course Button
            StirButton.gameObject.SetActive(false);
            FinishCourseButton.gameObject.SetActive(false);
            RestartCourseButton.gameObject.SetActive(false);

            // Hide Equipment Detail
            EquipmentDetailText.enabled = false;
            EquipmentDetailText.GetComponentInParent<Image>().enabled = false;

            // Hide Fail Detail
            FailDetailText.enabled = false;
            FailDetailText.GetComponentInParent<Image>().enabled = false;

            // Add Button action
            // Play Overlay
            MenuButton.onClick.AddListener(ShowSidebarMenu);
            StirButton.onClick.AddListener(() =>
            {
                Vibration.Vibrate(50);
                if (currentHitEquipment != null)
                {
                    currentHitEquipment.Stir();
                }
            });
            FinishCourseButton.onClick.AddListener(courseBehaviour.FinishCourse);
            RestartCourseButton.onClick.AddListener(courseBehaviour.RestartCourse);

            // Sidebar Menu
            ResumeButton.onClick.AddListener(HideAllSidebar);
            InstructionButton.onClick.AddListener(ShowTutorial);
            CheckListButton.onClick.AddListener(ShowCheckList);
            RestartButton.onClick.AddListener(HideAllSidebar);
            RestartButton.onClick.AddListener(courseBehaviour.RestartCourse);
            MainMenuButton.onClick.AddListener(courseBehaviour.StopCourse);

            // CheckList
            CloseCheckListButton.onClick.AddListener(HideAllSidebar);
        }

        public void TogglePlayCanvas(bool flag)
        {
            PlayCanvas.enabled = flag;
            DebugCanvas.enabled = flag;
        }

        public void ShowSidebarMenu()
        {
            SidebarMenuCanvas.enabled = true;
            TogglePlayCanvas(false);
        }

        public void ShowCheckList()
        {
            SidebarMenuCanvas.enabled = false;
            CheckListCanvas.enabled = true;
        }

        public void HideAllSidebar()
        {
            SidebarMenuCanvas.enabled = false;
            CheckListCanvas.enabled = false;

            TogglePlayCanvas(true);
        }

        public void HideAllCanvas()
        {
            TogglePlayCanvas(false);

            SidebarMenuCanvas.enabled = false;
            CheckListCanvas.enabled = false;
        }

        public void ShowFinishButton()
        {
            FinishCourseButton.gameObject.SetActive(true);
        }

        public void ShowRestartButton()
        {
            RestartCourseButton.gameObject.SetActive(true);
        }

        public void HideRestartButton()
        {
            RestartCourseButton.gameObject.SetActive(false);
        }

        public void ShowStirButton(FillableEquipment newHitEquipment)
        {
            currentHitEquipment = newHitEquipment;
            StirButton.gameObject.SetActive(true);
        }

        public void HideStirButton()
        {
            currentHitEquipment = null;
            StirButton.gameObject.SetActive(false);
        }

        public void ChangeInstructionMessage(string text)
        {
            InstructionText.text = "  ";
            InstructionText.text = text;
            InstructionText.text = text + " ";
        }

        public void ShowEquipmentDetail(string text)
        {
            EquipmentDetailText.enabled = true;
            EquipmentDetailText.GetComponentInParent<Image>().enabled = true;

            EquipmentDetailText.text = text;
            EquipmentDetailText.text = text + " ";
        }

        public void HideEquipmentDetail()
        {
            EquipmentDetailText.enabled = false;
            EquipmentDetailText.GetComponentInParent<Image>().enabled = false;

            EquipmentDetailText.text = "  ";
            EquipmentDetailText.text = " ";
        }

        public void ShowFailDetail(string text)
        {
            FailDetailText.enabled = true;
            FailDetailText.GetComponentInParent<Image>().enabled = true;

            FailDetailText.text = text;
            FailDetailText.text = text + " ";
        }

        public void HideFailDetail()
        {
            FailDetailText.enabled = false;
            FailDetailText.GetComponentInParent<Image>().enabled = false;

            FailDetailText.text = "  ";
            FailDetailText.text = " ";
        }

        public void ShowTutorial() {
            HideAllCanvas();
            TutorialCanvas.enabled = true;
        }
    }
}
