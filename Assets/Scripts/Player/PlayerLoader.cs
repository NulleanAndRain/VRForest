using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerLoader : MonoBehaviour
{
    [SerializeField] private Transform _playerStartPos;
    public static event UnityAction<GameObject> OnPlayersCameraLoaded = delegate { };

    [SerializeField] private bool _doRotateGlobalLight;
    [SerializeField] private Transform _globalLight;
    [SerializeField] private Vector3 _globalLightRotation;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        UpdatePlayer();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        IEnumerator awaitLoad()
        {
            yield return new WaitForEndOfFrame();
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p == null) yield break;
            p.transform.position = _playerStartPos.position;
            p.transform.rotation = _playerStartPos.rotation;
        }
        StartCoroutine(awaitLoad());
        UpdatePlayer();

        if (_doRotateGlobalLight)
        {
            _globalLight!.localEulerAngles = _globalLightRotation;
        }
    }

    private void UpdatePlayer()
    {
        IEnumerator awaitLoad()
        {
            yield return new WaitForEndOfFrame();
            var camObj = Camera.main.gameObject;
            OnPlayersCameraLoaded(camObj);
        }
        StartCoroutine(awaitLoad());
    }
}
