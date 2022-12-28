using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string _sceneNameForest = "ForestMain";
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        OVRInput.Update();
    }

    private void FixedUpdate()
    {
        OVRInput.FixedUpdate();
    }

    public void LoadForestScene()
    {
        Debug.Log("forest scene open");
        // open load screen
        // load
        //SceneManager.LoadScene(_sceneNameForest, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        //save state
        //exit
        Application.Quit();
    }
}
