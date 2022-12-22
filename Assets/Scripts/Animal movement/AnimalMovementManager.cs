using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalMovementManager : MonoBehaviour
{
    public Animator animator;
    public bool hasSpecialAnim;
    public float timeToNextPoint = 10f;
    public Transform[] wayPoints;
    public float lineOfSight = 15f;

    private NavMeshAgent _agent;
    private int _wayID;
    private float _localSpeed;
    private Vector3 _oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        StartCoroutine("SetAnim");
        _oldPosition = transform.position;
    }

    private void FixedUpdate()
    {
        _localSpeed = (transform.position.magnitude - _oldPosition.magnitude) / Time.fixedDeltaTime; //запоминаем скорость;
        _oldPosition = transform.position; //берем новую "старую" позицию
        if (_localSpeed != 0) animator.SetBool("IsRun", true);
        else animator.SetBool("IsRun", false);
        Debug.Log(_localSpeed + " " + gameObject.name);
    }


    private void GoRun()
    {
        int i = Random.Range(0, wayPoints.Length-1);
        if (i != _wayID)
        {
            _wayID = i;
            var dist = Vector3.Distance(transform.position, wayPoints[_wayID].position);
            if (dist > 0.1f)
            {
                if (hasSpecialAnim) animator.SetBool("IsSpecial", false);
                
                _agent.SetDestination(wayPoints[i].position);
            }
        }
        //else animator.SetBool("IsRun", false);
        
    }
    IEnumerator SetAnim()
    {
        while (true)
        {
            //animator.SetBool("IsRun", false);
            //yield return new WaitForSeconds(timeToNextPoint/2);
            //animator.SetBool("IsRun", true);
            //yield return new WaitForSeconds(0.5f);
            GoRun();
            yield return new WaitForSeconds(timeToNextPoint);
        }
    }
}
