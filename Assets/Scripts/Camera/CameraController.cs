using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    public Camera Camera => _camera;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
