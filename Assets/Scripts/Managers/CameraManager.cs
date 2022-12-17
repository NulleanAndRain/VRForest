using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [SerializeField] private CustomRenderTexture outputTex;
    public static CustomRenderTexture OutputTexture => Instance?.outputTex ?? null;

    private CameraController _camera;

    [Header("ImageSaving")]
    private static int fliesCount = 0;
    [SerializeField] private string _saveFolder = "/_Images/";
    private static string SaveFolder => Instance._saveFolder;

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

        Cam.Render();

        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        var Bytes = Image.EncodeToPNG();
        Destroy(Image);

        var folder = Application.dataPath + SaveFolder;
        if (!Directory.Exists(folder)) 
            Directory.CreateDirectory(folder);
        var filePath = folder + fliesCount + ".png";
        File.WriteAllBytes(filePath, Bytes);
        fliesCount++;

        Debug.Log($"Saved to {filePath}");
    }
}
