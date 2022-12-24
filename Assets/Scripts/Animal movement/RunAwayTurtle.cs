using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAwayTurtle : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Transform savePoint;
    float DistanseRange = 10;
    Transform Turtle;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Turtle = GameObject.FindGameObjectWithTag("Turtle").transform;
        agent = Turtle.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        savePoint = GameObject.FindGameObjectWithTag("TurtleSavePoint").transform;
        agent.speed = 4;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(savePoint.position);
        float distance = Vector3.Distance(animator.transform.position, player.position);

        if (distance > 50)
            animator.SetBool("turtleRunAway", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        agent.speed = 2;
    }
}

