using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;

    [Header("Movement")]
    [SerializeField] private float _speed;
    private Rigidbody _rb;
    [SerializeField] private float _yStepOffset = 0.3f;
    [SerializeField] private float _slopeMaxAngle = 45f;
    [SerializeField] private float _stepCheckDistance = 0.3f;
    [SerializeField] private float _wallCollisionCheckOffset = 1f;
    [SerializeField] private float _wallCollisionCheckDist = 0.5f;

    [Header("Mouse camera moving")]
    [SerializeField] private Transform CamHolder;

    private Vector3 rotation = Vector3.zero;
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";
    private Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {

        rotation.x += Input.GetAxis(xAxis) * sensitivity;
        rotation.y += Input.GetAxis(yAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        CamHolder.localRotation = xQuat * yQuat;

        var dir =
            (_playerCamera.transform.forward * InputManager.vertical +
            _playerCamera.transform.right * InputManager.horizontal);
        dir.y = 0;
        moveDir = dir.normalized;
    }

    void FixedUpdate()
    {
        var rayWalls = new Ray(_rb.position + new Vector3(0, _wallCollisionCheckOffset, 0), moveDir);
       
        if (moveDir.magnitude >= 0.05f && !Physics.Raycast(rayWalls, _wallCollisionCheckDist))
        {
            var offset = new Vector3(0, _yStepOffset, 0);
            var pos = _rb.position + offset;

            var ray = new Ray(pos, moveDir);

            Debug.DrawLine(
                pos,
                pos + moveDir * _stepCheckDistance, 
                Color.blue, 
                0.5f
            );

            if (Physics.Raycast(ray, out var hitInfo, _stepCheckDistance))
            {
                Debug.DrawLine(hitInfo.point, hitInfo.point + hitInfo.normal, Color.red, 0.5f);
                var normalCos = new Vector3(hitInfo.normal.x, 0, hitInfo.normal.z);
                var angle = Vector3.Angle(normalCos, hitInfo.normal);

                if (angle <= _slopeMaxAngle)
                    return;
            } 
            
            _rb.MovePosition(_rb.position + moveDir * _speed * Time.deltaTime);
            
        }
    }
}
