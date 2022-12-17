using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [SerializeField] private CustomRenderTexture outputTex;
    public static CustomRenderTexture OutputTexture => Instance?.outputTex ?? null;

    private CameraController _camera;

    [Header("ImageSaving")]
    [SerializeField] private string _saveFolder = "/_Images";
    private static string SaveFolder => Instance._saveFolder;
    private const string SaveFormat = ".jpg";

    public static CameraController CurrentCamera
    {
        get => Instance?._camera;
        set
        {
            if (Instance?._camera != null)
            {
                Instance._camera.ResetCamera();
            }
            Instance._camera = value;
            value.SetRenderTex(OutputTexture);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public static void MakeCameraShot()
    {
        var Cam = CurrentCamera.Camera;

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = OutputTexture;
        // Cam.Render();

        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height, TextureFormat.RGB24, mipCount: -1, linear: true);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0, false);
        Image.filterMode = FilterMode.Trilinear;

        Image.Apply();

        RenderTexture.active = currentRT;

        var Bytes = Image.EncodeToJPG();
        Destroy(Image);

        var folder = Application.dataPath + SaveFolder;
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        var fileName = DateTime.Now.ToString("G")
            .Replace(" ", "_")
            .Replace(":", ".");
        var i = 1;
        var fileNameNew = fileName;
        while (File.Exists(folder + fileNameNew + SaveFormat))
        {
            fileNameNew = $"{fileName}_{i}";
            i++;
        }

        var filePath = folder + fileNameNew + SaveFormat;
        File.WriteAllBytes(filePath, Bytes);

        Debug.Log($"Saved to {filePath}");
    }
}
