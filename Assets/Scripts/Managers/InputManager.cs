using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static float horizontal 
    {
        get
        {
            var i = Input.GetAxis("Horizontal");
            if (Mathf.Abs(i) >= Mathf.Epsilon)
            {
                return i;
            }
            return 0;
        }
    }
    public static float vertical
    {
        get
        {
            var i = Input.GetAxis("Vertical");
            if (Mathf.Abs(i) >= Mathf.Epsilon)
            {
                return i;
            }
            return 0;
        }
    }

    public static Vector3 moveInput => new Vector3(-vertical, 0, horizontal);
}
