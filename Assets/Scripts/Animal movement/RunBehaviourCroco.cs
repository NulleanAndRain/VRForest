using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunBehaviourCroco : StateMachineBehaviour
{
    float timer;
    List<Transform> points = new List<Transform>();
    private NavMeshAgent agent;
    Transform croco;
    Transform player;
    float DistanseRange = 10;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        Transform pointsObject = GameObject.FindGameObjectWithTag("PointsForCroco").transform;
        foreach (Transform t in pointsObject)
            points.Add(t);

        croco = GameObject.FindGameObjectWithTag("croco").transform;
        agent = croco.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.SetDestination(points[0].position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(points[Random.Range(0, points.Count)].position);

        timer += Time.deltaTime;
        if (timer > 10)
            animator.SetBool("CrocoRun", false);

        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance < DistanseRange)
            animator.SetBool("CrocoRunAway", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}

