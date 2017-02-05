using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slide : MonoBehaviour {

    public Button nextButton;
    public Button backButton;
    public Image slider;
    public List<Sprite> imageList;
    int index = 0;

    // Use this for initialization
    void Start () {
        string path = "Sprites/Tutorial/card";
        int number = 1;
        while (number < 9) {
            imageList.Add(Resources.Load("Sprites/Tutorial/card" + number, typeof(Sprite)) as Sprite);
            number++;
        }
        slider.GetComponent<Image>().sprite = imageList[index];
    }

    public void next() {
        if (index < 7)
        {
            index++;
        }
        else if (index == 8)
        {
            nextButton.GetComponentInChildren<Text>().text = "Done";
            index++;
        }
        else {
            SceneManager.LoadScene(0);
        }
        slider.GetComponent<Image>().sprite = imageList[index];
    }

    public void back() {
        if (index > 0)
        {
            index--;
        }
        else {
            SceneManager.LoadScene(0);
        }
        slider.GetComponent<Image>().sprite = imageList[index];
    }


}
