using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleMovement : MonoBehaviour
{
    public Transform Eagle;
    public Transform[] wayPoint = new Transform[10];
    private int currentWayPoint = 0;
    private float rotationSpeed = 6.0f; 
    public float accelerate = 3f;

    void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(wayPoint[currentWayPoint].position - transform.position);
        transform.rotation = Quaternion.Slerp(Eagle.rotation, rotation, Time.deltaTime * rotationSpeed);
        Vector3 wayPointDirection = wayPoint[currentWayPoint].position - transform.position;
        float speedElement = Vector3.Dot(wayPointDirection.normalized, transform.forward);
        float speed = accelerate * speedElement;
       

        if (transform.position != Vector3.Lerp(Eagle.position, wayPoint[currentWayPoint].position, Time.deltaTime * speed))
        {
            transform.Translate(0, 0, Time.deltaTime * speed);
        }
        else if (currentWayPoint != 9)
        {
            currentWayPoint++;
        }
        else currentWayPoint = 0;
    }

}

