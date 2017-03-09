using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideInGame : MonoBehaviour {

    public Button nextButton;
    public Button backToGameButton;
    public Button backButton;
    public Image slider;
    public List<Sprite> imageList;
    public Canvas tutorialCanvas;
    public Canvas playCanvas;
    int index = 0;

    private GlobalObject _Global;
    
    // Use this for initialization
    void Start()
    {
        string path = "Sprites/Tutorial/card";
        int number = 1;
        while (number < 9)
        {
            imageList.Add(Resources.Load(path + number, typeof(Sprite)) as Sprite);
            number++;
        }
        slider.GetComponent<Image>().sprite = imageList[index];
    }

    public void next()
    {
        if (index < 7)
        {
            index++;
        }
        else if (index == 8)
        {
            nextButton.GetComponentInChildren<Text>().text = "Done";
            index++;
        }
        else
        {
            //back to game
        }
        slider.GetComponent<Image>().sprite = imageList[index];
    }

    public void back()
    {
        if (index > 0)
        {
            index--;
        }
        else
        {
            //back to game
        }
        slider.GetComponent<Image>().sprite = imageList[index];
    }

    public void backToGame() {
        tutorialCanvas.enabled = false;
        playCanvas.enabled = true;
    }
}
