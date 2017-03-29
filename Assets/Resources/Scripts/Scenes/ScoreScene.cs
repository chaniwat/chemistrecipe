using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreScene : MonoBehaviour {

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartCourse()
    {
        SceneManager.LoadScene(1);
    }

}
