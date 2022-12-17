using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixObject : MonoBehaviour
{
    private Vector3 _startPos;
    private Quaternion _rotationStrart;

    void Start()
    {
        _rotationStrart = transform.localRotation;
        _startPos = transform.localPosition;
    }

    private void FixedUpdate()
    {
        transform.localRotation = _rotationStrart;
        transform.localPosition = _startPos;
    }
}
