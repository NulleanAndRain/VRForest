using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrocoMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 here;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3(100f, 0f, 100f));
    }

    void Update()
    {
        here = new Vector3(Random.Range(1, 100), 0f, Random.Range(-50, -20));
        agent.SetDestination(here);
    }
}
