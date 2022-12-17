using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private Vector3 moveDir;

    [Header("UI")]
    [SerializeField] private GameObject MenuCanvas;
    [SerializeField] private GameObject UiPointer;
    [SerializeField] private MenuController menu;
    [SerializeField] private PlayerUiPointer pointer;
    private bool _menuUiOpened = false;
    private bool MenuUiOpened
    {
        get => _menuUiOpened;
        set
        {
            _menuUiOpened = value;
            MenuCanvas?.SetActive(_menuUiOpened);
            if (UiPointer != null)
                UiPointer.SetActive(_menuUiOpened);
            Cursor.visible = _menuUiOpened;
            Cursor.lockState = _menuUiOpened ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    [Header("Camera")]
    [SerializeField] private Transform _camHand;
    [SerializeField] private CameraController _startCamera;
    private bool _willTakeShot = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        MenuUiOpened = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        CameraManager.CurrentCamera = _startCamera;
    }

    private void Update()
    {

        var rotation = InputManager.cameraInput;
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        CamHolder.localRotation = yQuat;
        transform.rotation = xQuat;

        var dir =
            (_playerCamera.transform.forward * InputManager.vertical +
            _playerCamera.transform.right * InputManager.horizontal);
        dir.y = 0;
        moveDir = dir.normalized;

        if (InputManager.pauseInput)
        {
            MenuUiOpened = !MenuUiOpened;
        }

        if (MenuUiOpened)
        {
            if (InputManager.uiClickPressed)
            {
#if UNITY_EDITOR
                RaycastFromCursor();
#endif
                pointer.ClickUI();
            }
        }
        else
        {
            if (InputManager.cameraTriggerPressed)
            {
                if (CameraManager.CurrentCamera == null)
                    UpdateHandCamera();

                CameraManager.MakeCameraShot();
            }
        }
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

    private void UpdateHandCamera()
    {
        var cam = _camHand.GetComponentInChildren<CameraController>();
        CameraManager.CurrentCamera = cam;
    }

    private void RaycastFromCursor()
    {
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //linePointer.SetPosition(0, ray.origin);
        if (Physics.Raycast(ray, out hit, 2.5f))
        {
            var uiObject = hit.rigidbody?.GetComponent<OVRRaycaster>();
            if (uiObject == null) return;
            var hitPos = uiObject.transform.InverseTransformPoint(hit.point);

            var e = new PointerEventData(EventSystem.current);
            e.position = hitPos;

            var res = new List<RaycastResult>();
            uiObject.Raycast(e, res);

            var btn = res
                .Select(r => r.gameObject.GetComponent<Button>())
                .FirstOrDefault(b => b != null);
            if (btn != null)
                ExecuteEvents.Execute(btn.gameObject, e, ExecuteEvents.submitHandler);
        }
    }
}
