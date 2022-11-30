using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _speed;
    private Rigidbody _rb;

    [SerializeField] private Transform CamHolder;

    private Vector3 rotation = Vector3.zero;

    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";
    Vector3 noYAxis = new Vector3(1, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
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
        dir = dir.normalized;

        _rb.MovePosition(
            transform.position +
            dir *
            _speed *
            Time.deltaTime
        );
    }
}
