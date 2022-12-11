using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoxMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 here;
    public Transform[] wayPoint = new Transform[10];

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        here = new Vector3(Random.Range(-10, 300), 0f, Random.Range(-10, 300));
        agent.SetDestination(here);
    }
}
