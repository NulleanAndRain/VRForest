using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static float moveSmooth = 0.2f;
    private static float stopSmooth = 0.05f;

    private static float horCurrent = 0;
    private static float vertCurrent = 0;

    private static float horVel = 0;
    private static float vertVel = 0;

    private static float _camParamCurrent = 0;
    private static float _camParamVel = 0;

    private static InputManager _instance;

    [Header("Camera")]
    [SerializeField] private float camSensitivity = 10;
    [SerializeField] private float camSensitivityMouse = 10;
    [SerializeField] private OVRInput.RawButton _camShotBtn = OVRInput.RawButton.LIndexTrigger;

    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";
    private static Vector2 rotation = Vector2.zero;

    [Header("Pause")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private OVRInput.Button pauseKeyVR = OVRInput.Button.Start;

    [Header("Camera Params menu")]
    [SerializeField] private KeyCode _paramUpKey = KeyCode.R;
    [SerializeField] private KeyCode _paramDownKey = KeyCode.F;

    [SerializeField] private OVRInput.RawButton _paramUpKeyVR = OVRInput.RawButton.B;
    [SerializeField] private OVRInput.RawButton _paramDownKeyVR = OVRInput.RawButton.A;

    [Header("Debug")]
    [SerializeField] private bool usePcInput = false;
    public static bool UsePcInput => _instance.usePcInput;

    private void Awake()
    {
        _instance = this;
    }

    public static float horizontal
    {
        get
        {
            float r = 0;
            if (UsePcInput)
            {
                r = Input.GetAxisRaw("Horizontal");
            }
            r += OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
            if (Mathf.Abs(r) >= Mathf.Epsilon)
            {
                return Mathf.SmoothDamp(horCurrent, r, ref horVel, moveSmooth);
            }
            return Mathf.SmoothDamp(horCurrent, 0, ref horVel, stopSmooth);
        }
    }
    public static float vertical
    {
        get
        {
            float r = 0;
            if (UsePcInput)
            {
                r = Input.GetAxisRaw("Vertical");
            }
            r += OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
            if (Mathf.Abs(r) >= Mathf.Epsilon)
            {
                return Mathf.SmoothDamp(vertCurrent, r, ref vertVel, moveSmooth);
            }
            return Mathf.SmoothDamp(vertCurrent, 0, ref vertVel, stopSmooth);
        }
    }

    public static Vector3 moveInput
    {
        get
        {
            var i = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
            return new Vector3(-vertical + i.y, 0, horizontal + i.x).normalized;
        }
    }

    public static Vector2 cameraInput
    {
        get
        {
            if (UsePcInput)
            {
                rotation.x += Input.GetAxis(xAxis) * _instance.camSensitivityMouse;
                rotation.y += Input.GetAxis(yAxis) * _instance.camSensitivityMouse;
            }
            rotation += OVRInput.Get(OVRInput.RawAxis2D.RThumbstick) * _instance.camSensitivity;
            rotation.y = Mathf.Clamp(rotation.y, -_instance.yRotationLimit, _instance.yRotationLimit);
            return rotation;
        }
    }

    public static bool pauseInput
    {
        get
        {
            return Input.GetKeyDown(_instance.pauseKey) ||
                OVRInput.GetDown(_instance.pauseKeyVR);
        }
    }

    public static bool uiClickPressed
    {
        get
        {
            return UsePcInput && Input.GetKeyDown(KeyCode.Mouse0) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
        }
    }

    public static bool cameraTriggerPressed
    {
        get
        {
            return UsePcInput && Input.GetKeyDown(KeyCode.Mouse1) || OVRInput.GetDown(_instance._camShotBtn);
        }
    }

    public static float cameraParamSliderValue
    {
        get
        {
            float r = 0;
            if (UsePcInput)
            {
                r = Input.GetKey(_instance._paramUpKey) ? 1 : 
                        (Input.GetKey(_instance._paramDownKey) ? -1 : 0
                );
            }
            r += OVRInput.Get(_instance._paramUpKeyVR) ? 1 : 
                    OVRInput.Get(_instance._paramDownKeyVR) ? -1 : 
                    0;

            if (Mathf.Abs(r) >= Mathf.Epsilon)
            {
                return Mathf.SmoothDamp(_camParamCurrent, r, ref _camParamVel, moveSmooth);
            }
            return 0;
        }
    }

    public static bool touchedAnyCameraParamControll =>
        Input.GetKey(_instance._paramUpKey) ||
        Input.GetKey(_instance._paramDownKey) ||
        OVRInput.Get(_instance._paramUpKeyVR) ||
        OVRInput.Get(_instance._paramDownKeyVR);
}
