using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public Camera Camera => _camera;
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

    private Vector3 _startPos;
    private Quaternion _rotationStrart;

    void Start()
    {
        CameraManager.CurrentCamera = this;
        _rotationStrart = transform.localRotation;
        _startPos = transform.localPosition;
    }

    private void FixedUpdate()
    {
        transform.localRotation = _rotationStrart;
        transform.localPosition = _startPos;
    }
}
