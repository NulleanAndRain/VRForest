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

    [SerializeField] private float _maxDistToObject = 25f;
    public static float MaxDistToObject => Instance._maxDistToObject;

    [Header("ImageSaving (pc only)")]
    [SerializeField] private string _saveFolder = "/_Images";
    [SerializeField] private string _galleryName = "PhotoHunting";
    private static string SaveFolder => Instance._saveFolder;
    private const string SaveFormat = ".jpg";

    [Header("Camera params")]
    [SerializeField] private float _zoomSpeed = 10f;
    public static float ZoomSpeed => Instance._zoomSpeed;

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
            value!.SetRenderTex(OutputTexture);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static bool IsTakingShot { get; private set; }

    public static void MakeCameraShot()
    {
        IEnumerator takeShot()
        {
            yield return new WaitForEndOfFrame();
            IsTakingShot = true;
            var Cam = CurrentCamera.Camera;

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = OutputTexture;
            Cam.Render();

            Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height, TextureFormat.RGB24, mipCount: -1, linear: true);
            Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0, false);
            Image.filterMode = FilterMode.Trilinear;

            Image.Apply();

            RenderTexture.active = currentRT;

            var bytes = Image.EncodeToJPG();
            Destroy(Image);

            var fileName = DateTime.Now.ToString("G")
                .Replace(" ", "_")
                .Replace(":", ".");
            var i = 1;
#if UNITY_EDITOR
            var folder = Application.dataPath + SaveFolder;

            var fileNameNew = fileName;
            while (File.Exists(folder + fileNameNew + SaveFormat))
            {
                fileNameNew = $"{fileName}_{i}";
                i++;
            }

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var filePath = folder + fileNameNew + SaveFormat;
            File.WriteAllBytes(filePath, bytes);

            Debug.Log($"Saved to {filePath}");
#elif UNITY_ANDROID
            var fileNameNew = fileName;

            NativeGallery.SaveImageToGallery(bytes, Instance._galleryName, fileNameNew);
#endif

            IsTakingShot = false;
        }
        Instance.StartCoroutine(takeShot());
    }

    public static void Zoom(float speed)
    {
        CurrentCamera!.Zoom += speed;
    }
}
