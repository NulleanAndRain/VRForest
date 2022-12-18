using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class LoadSceneOnTrigger : MonoBehaviour
{
    [SerializeField] private Scene _targetScene;
    [SerializeField] private LoadSceneMode _loadMode;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            SceneManager.LoadScene((int)_targetScene, _loadMode);
    }

    [Serializable]
    public enum Scene
    {
        SampleScene = 0,
        MainForest = 1
    }
}
