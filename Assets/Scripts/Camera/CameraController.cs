using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RenderTexture _defaultTex;
    private Material _screenMat;
    [SerializeField] private Material _screenDefaultMat;
    [SerializeField] private MeshRenderer _screenPlane;
    public Camera Camera => _camera;
    public RenderTexture DefaultTex => _defaultTex;
    [SerializeField] private DepthOfField _dof;

    [SerializeField] private float _focalRangeMin;
    [SerializeField] private float _focalRangeMax;
    

    private float _focalRange;
    public float FocalRange
    {
        get => _focalRange;
        set
        {
            _focalRange = Mathf.Clamp(value, _focalRangeMin, _focalRangeMax);
            Camera.focalLength = _focalRange;
        }
    }


    void Start()
    {
        // CameraManager.CurrentCamera = this;
        _screenMat = Instantiate(_screenDefaultMat);
        _screenPlane.material = _screenMat;
    }

    public void SetRenderTex(RenderTexture tex)
    {
        _screenMat.mainTexture = tex;
        _camera.targetTexture = tex;
        _screenPlane.material = _screenMat;
    }

    public void ResetCamera()
    {
        _camera.targetTexture = _defaultTex;
        _screenMat = Instantiate(_screenDefaultMat);
        _screenPlane.material = _screenMat;
    }
}
