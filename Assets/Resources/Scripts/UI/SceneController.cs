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

        [Header("Play Overlay")]
        public Button MenuButton;
        public Text TimerText;
        public Text InstructionText;

        [Header("Menu Buttons")]
        public Button ResumeButton;
        public Button InstructionButton;
        public Button CheckListButton;
        public Button SettingButton;
        public Button RestartButton;
        public Button MainMenuButton;

        [Header("Checklist Buttons")]
        public Button CloseCheckListButton;

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

            // Add Button action
            // Play Overlay
            MenuButton.onClick.AddListener(ShowSidebarMenu);

            // Sidebar Menu
            ResumeButton.onClick.AddListener(HideAllSidebar);
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

    }
}
