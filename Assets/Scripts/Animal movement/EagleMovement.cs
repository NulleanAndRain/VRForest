using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EagleMovement : MonoBehaviour
{
    private Animator animator;
    float timer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        timer = 0;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 6)
            animator.SetBool("flystay", false);
        if (timer > 6)
        {
            animator.SetBool("flystay", true);
            if (timer > 15)
            {
                timer = 0;
            }
        }
    }
   
}

