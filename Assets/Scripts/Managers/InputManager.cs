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

    private static InputManager _instance;

    [Header("Camera")]
    [SerializeField] private float camSensitivity = 10;
    [SerializeField] private float camSensitivityMouse = 10;

    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";
    private static Vector2 rotation = Vector2.zero;

    [Header("Pause")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private OVRInput.Button pauseKeyVR = OVRInput.Button.Start;

    private void Awake()
    {
        _instance = this;
    }

    public static float horizontal
    {
        get
        {
            var i = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(i) >= Mathf.Epsilon)
            {
                return Mathf.SmoothDamp(horCurrent, i, ref horVel, moveSmooth);
            }
            return Mathf.SmoothDamp(horCurrent, 0, ref horVel, stopSmooth);
        }
    }
    public static float vertical
    {
        get
        {
            var i = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(i) >= Mathf.Epsilon)
            {
                return Mathf.SmoothDamp(vertCurrent, i, ref vertVel, moveSmooth);
            }
            return Mathf.SmoothDamp(vertCurrent, 0, ref vertVel, stopSmooth);
        }
    }

    public static Vector3 moveInput
    {
        get
        {
            var i = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            return new Vector3(-vertical + i.y, 0, horizontal + i.x).normalized;
        }
    }

    public static Vector2 cameraInput
    {
        get
        {
            rotation.x += Input.GetAxis(xAxis) * _instance.camSensitivityMouse;
            rotation.y += Input.GetAxis(yAxis) * _instance.camSensitivityMouse;
            rotation += OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick) * _instance.camSensitivity;
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
            return Input.GetKeyDown(KeyCode.Mouse0);
        }
    }
}
