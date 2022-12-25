using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EagleMovement : MonoBehaviour
{
    public Transform Eagle;
    public Transform[] wayPoint = new Transform[4];
    private int currentWayPoint = 0;
    private float rotationSpeed = 6.0f; 
    public float accelerate = 3f;
    private Vector3 here;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(wayPoint[currentWayPoint].position - transform.position);
        transform.rotation = Quaternion.Slerp(Eagle.rotation, rotation, Time.deltaTime * rotationSpeed);
        Vector3 wayPointDirection = wayPoint[currentWayPoint].position - transform.position;
        float speedElement = Vector3.Dot(wayPointDirection.normalized, transform.forward);
        float speed = accelerate * speedElement;

        for (int i = 0; i < 3; i++)
        {
            //here
        }
        if (transform.position != Vector3.Lerp(Eagle.position, wayPoint[currentWayPoint].position, Time.deltaTime * speed))
        {
            //transform.Translate(0, 0, Time.deltaTime * speed);
            agent.SetDestination(here);
        }
        else if (currentWayPoint != 3)
        {
            currentWayPoint++;
        }
        else currentWayPoint = 0;
    }

}

