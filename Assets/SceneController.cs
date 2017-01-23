using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    public Canvas sidebarCanvas;
    public Canvas checkListCanvas;
    public Canvas canvas;

    public Button menuButton;
    public Button resumeButton;
    public Button checkListButton;
    public Button saveCheckListButton;
    // Use this for initialization
    void Start () {
        SetupUI();
	}

    void SetupUI()
    {
        menuButton.onClick.AddListener(ShowSidebar);
        resumeButton.onClick.AddListener(HideSidebar);
        checkListButton.onClick.AddListener(ShowCheckList);
        saveCheckListButton.onClick.AddListener(HideCheckList);
        sidebarCanvas.GetComponent<Canvas>().enabled = false;
        checkListCanvas.GetComponent<Canvas>().enabled = false;
    }

    void ShowSidebar()
    {
        sidebarCanvas.GetComponent<Canvas>().enabled = true;
        canvas.GetComponent<Canvas>().enabled = false;
    }

    void HideSidebar()
    {
        sidebarCanvas.GetComponent<Canvas>().enabled = false;
        canvas.GetComponent<Canvas>().enabled = true;
    }

    void ShowCheckList()
    {
        sidebarCanvas.GetComponent<Canvas>().enabled = false;
        checkListCanvas.GetComponent<Canvas>().enabled = true;
    }

    void HideCheckList()
    {
        checkListCanvas.GetComponent<Canvas>().enabled = false;
        canvas.GetComponent<Canvas>().enabled = true;
    }
}
