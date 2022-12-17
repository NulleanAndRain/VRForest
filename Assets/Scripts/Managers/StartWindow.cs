using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartWindow : MonoBehaviour
{
    public GameObject startButton;
    public GameObject exitButton;
    public int sceneId;
   
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneId);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
