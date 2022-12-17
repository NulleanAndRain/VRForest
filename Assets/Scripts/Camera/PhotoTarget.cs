using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoTarget : MonoBehaviour
{
    [SerializeField] private string AnimalName = "aboba";
    [SerializeField] private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }

    private void OnRenderObject()
    {
        if (CameraManager.IsTakingShot)
        {
            var tex = CameraManager.OutputTexture;
            var screenPos = CameraManager.CurrentCamera.Camera
                .WorldToScreenPoint(_renderer.bounds.center, Camera.MonoOrStereoscopicEye.Mono);

            if (screenPos.z < CameraManager.CurrentCamera.Camera.nearClipPlane) 
                return;

            var screenCenter = new Vector2(tex.width/2, tex.height/2);

            var dist = Vector2.Distance(screenCenter, screenPos);
            if (Mathf.Abs(dist) > screenCenter.magnitude/2)
            {
                return;
            }

            GetPhotographed();
        }
    }

    public void GetPhotographed()
    {
        Debug.Log($"{AnimalName} got photographed");
    }
}
