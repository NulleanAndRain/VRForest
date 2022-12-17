using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 here;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        here = new Vector3(Random.Range(-100, 500), 0f, Random.Range(-100, 500));
        agent.SetDestination(here); 
    }
}
