using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAwayCroco : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Transform savePoint;
    float DistanseRange = 10;
    Transform croco;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        croco = GameObject.FindGameObjectWithTag("croco").transform;
        agent = croco.GetComponent<NavMeshAgent>();
        agent.speed = 7;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        savePoint = GameObject.FindGameObjectWithTag("CrocoSavePoint").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(savePoint.position);
        float distance = Vector3.Distance(animator.transform.position, player.position);

        if (distance > 50)
            animator.SetBool("CrocoRunAway", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        agent.speed = 2;
    }
}

