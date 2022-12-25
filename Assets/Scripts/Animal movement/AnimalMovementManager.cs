using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalMovementManager : MonoBehaviour
{
    public Animator animator;
    public float speed = 2.25f;
    public float timeToNextPoint = 10f;
    public Transform[] wayPoints;
    public float radiusOfSight = 15f;
    public bool hasSpecialRun;
    public bool aggressive;

    private NavMeshAgent _agent;
    private int _wayID;
    private float _localSpeed;
    private Vector3 _oldPosition;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = speed;
        StartCoroutine("StartRun");
        _oldPosition = transform.position;
        gameObject.GetComponent<SphereCollider>().radius = radiusOfSight;
    }

    private void FixedUpdate()
    {
        CheckLocalSpeed();
    }

    private void MovementInTheArea()
    {
        int i = Random.Range(0, wayPoints.Length-1);
        if (i != _wayID)
        {
            _wayID = i;
            var dist = Vector3.Distance(transform.position, wayPoints[_wayID].position);
            if (dist > 0.1f)  _agent.SetDestination(wayPoints[i].position);
        }
    }
    
    private void CheckLocalSpeed()
    {
        _localSpeed = (transform.position.magnitude - _oldPosition.magnitude) / Time.fixedDeltaTime;
        _oldPosition = transform.position;
        if (Mathf.Abs(_localSpeed) >= 0.001f)
        {
            if (hasSpecialRun && animator.GetBool("IsSpecial")) 
                    animator.SetBool("IsRun", false);
            else 
                animator.SetBool("IsRun", true);
        }
        else 
            animator.SetBool("IsRun", false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && radiusOfSight > 0)
        {
            if (hasSpecialRun)
            {
                animator.SetBool("IsSpecial", true);
                animator.SetBool("IsRun", false);
            }
            var runningDirection = transform.position - other.transform.position;
            if (!aggressive) 
                _agent.SetDestination(runningDirection * 1.2f);
            else 
                _agent.SetDestination(other.transform.position * 0.8f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasSpecialRun && other.tag == "Player") 
            animator.SetBool("IsSpecial", false);
    }

    IEnumerator StartRun()
    {
        while (true)
        {
            MovementInTheArea();
            yield return new WaitForSeconds(timeToNextPoint);
        }
    }

    
}
