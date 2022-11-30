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
            var i = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
            return new Vector3(-vertical + i.y, 0, horizontal + i.x).normalized;
        }
    }

    public static Vector2 cameraInput => OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
}
