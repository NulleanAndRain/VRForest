using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [SerializeField] private RenderTexture outputTex;
    public static RenderTexture OutputTexture => Instance?.outputTex ?? null;

    private CameraController _camera;
    public static CameraController CurrentCamera
    {
        get => Instance?._camera;
        set
        {
            if (Instance?._camera != null)
            {
                Instance._camera.Camera.targetTexture = null;
            }
            if (Instance == null) return;
            Instance._camera = value;
            value.Camera.targetTexture = OutputTexture;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }
}
