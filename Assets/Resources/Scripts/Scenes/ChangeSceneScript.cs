using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneScript : MonoBehaviour {

	public void changeScene()
    {
        InputField inputField = GameObject.Find("InputField").GetComponent<InputField>();

        GlobalObject globalObject = GameObject.Find("_Global").GetComponent<GlobalObject>();

        globalObject.message = inputField.text;
        
        SceneManager.LoadScene("Resources/Scenes/Test/Objects_Collection");
    }

}
