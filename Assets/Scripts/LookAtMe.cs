using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    private GameObject target;

    private void Start()
    {
        PlayerLoader.OnPlayersCameraLoaded += (player) => target = player;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            transform.LookAt(target.transform);
            var angles = transform.eulerAngles;
            angles.x = 0;
            angles.z = 0;
            transform.eulerAngles = angles;
        }
    }
}
