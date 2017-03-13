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
        public Button RestartCourseButton;
        public Button StirButton;
        public Text TimerText;
        public Text InstructionText;
        public Text EquipmentDetailText;
        public Text FailDetailText;

        [Header("Result Overlay")]
        public Canvas resultCanvas;
        public Image resultImage;
        public Button FinishCourseButton;

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
        private bool isHoldStirButton = false;

        private float accumulateHoldingStirButton = 0f;
        private float targetHoldingStirButtonTime = 0.13f;

        // Use this for initialization
        void Start()
        {
            SetupUI();
        }

        private string updateInstructionText = "";
        private string updateEquipmentText = "";
        private string updateFailText = "";

        private bool forceUpdateInstruction = false;
        private bool forceUpdateEquipmentDetail = false;
        private bool forceUpdateFailDetail = false;

        private bool highlightInstructionText = false;
        private float accumulateInstructionText = 0f;
        private bool blinkState = false;
        private float accumulateblinkInstructionText = 0f;

        public void Update()
        {
            if(isHoldStirButton)
            {
                accumulateHoldingStirButton += Time.deltaTime;

                while(accumulateHoldingStirButton > targetHoldingStirButtonTime)
                {
                    accumulateHoldingStirButton -= targetHoldingStirButtonTime;
                    CallStirButtonAction();
                }
            }
            else
            {
                accumulateHoldingStirButton = 0f;
            }

            if(forceUpdateInstruction)
            {
                InstructionText.text = updateInstructionText + " ";
                forceUpdateInstruction = false;

                highlightInstructionText = true;
                accumulateInstructionText = 2.5f;
                accumulateblinkInstructionText = 0.25f;
                InstructionText.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0f);
            }

            if(highlightInstructionText)
            {
                accumulateblinkInstructionText -= Time.deltaTime;

                if(accumulateblinkInstructionText < 0f)
                {
                    accumulateblinkInstructionText = 0.25f;

                    if(!blinkState)
                    {
                        InstructionText.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 1f);
                        blinkState = true;
                    }
                    else
                    {
                        InstructionText.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0f);
                        blinkState = false;
                    }
                }

                accumulateInstructionText -= Time.deltaTime;

                if (accumulateInstructionText < 0f)
                {
                    highlightInstructionText = false;
                    InstructionText.color = Color.white;
                }
            }

            if(forceUpdateEquipmentDetail)
            {
                EquipmentDetailText.text = updateEquipmentText + " ";
                forceUpdateEquipmentDetail = false;
            }

            if(forceUpdateFailDetail)
            {
                FailDetailText.text = updateFailText + " ";
                forceUpdateFailDetail = false;
            }
        }

        void SetupUI()
        {
            // Default hide sidebar canvas
            SidebarMenuCanvas.enabled = false;
            CheckListCanvas.enabled = false;
            TutorialCanvas.enabled = false;
            resultCanvas.enabled = false;

            // Disable Stir & Finish Course Button
            StirButton.gameObject.SetActive(false);
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
            // Stir button is set in EventTrigger         

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
            resultCanvas.enabled = false;
        }

        public void ShowFinishButton()
        {
            HideAllCanvas();
            resultCanvas.enabled = true;
        }

        public void HideFinishButton()
        {
            TogglePlayCanvas(true);
            resultCanvas.enabled = false;
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
            InstructionText.text = text;

            forceUpdateInstruction = true;
            updateInstructionText = text;
        }

        public void ShowEquipmentDetail(string text)
        {
            EquipmentDetailText.enabled = true;
            EquipmentDetailText.GetComponentInParent<Image>().enabled = true;

            EquipmentDetailText.text = text;

            forceUpdateEquipmentDetail = true;
            updateEquipmentText = text;
        }

        public void HideEquipmentDetail()
        {
            EquipmentDetailText.enabled = false;
            EquipmentDetailText.GetComponentInParent<Image>().enabled = false;

            EquipmentDetailText.text = " ";

            forceUpdateEquipmentDetail = true;
            updateEquipmentText = " ";
        }

        public void ShowFailDetail(string text)
        {
            FailDetailText.enabled = true;
            FailDetailText.GetComponentInParent<Image>().enabled = true;

            FailDetailText.text = text;

            forceUpdateFailDetail = true;
            updateFailText = text;
        }

        public void HideFailDetail()
        {
            FailDetailText.enabled = false;
            FailDetailText.GetComponentInParent<Image>().enabled = false;

            FailDetailText.text = " ";

            forceUpdateFailDetail = true;
            updateFailText = " ";
        }

        public void ShowTutorial() {
            HideAllCanvas();
            TutorialCanvas.enabled = true;
        }

        public void OnStirButtonDown()
        {
            isHoldStirButton = true;
        }

        public void OnStirButtonUp()
        {
            isHoldStirButton = false;
        }

        public void CallStirButtonAction()
        {
            Vibration.Vibrate(130);
            if (currentHitEquipment != null)
            {
                currentHitEquipment.Stir();
            }
        }
    }
}
