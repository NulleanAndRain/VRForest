using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleAnimateManager : MonoBehaviour
{
    public Animator animator;
    public float soaringTime = 5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine("Timer");
    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(soaringTime);
            animator.SetBool("isStay", false);
            yield return new WaitForSeconds(soaringTime);
            animator.SetBool("isStay", true);
        }
    }
}
