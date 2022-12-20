using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleAnimateManager : MonoBehaviour
{
    public Animator animator;
    public float soaring_time = 5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine("SoaringTimer");
    }

    IEnumerator SoaringTimer()
    {
        animator.SetBool("isStay", true);
        yield return new WaitForSeconds(soaring_time);
        StopCoroutine("SoaringTimer");
        StartCoroutine("SwipeTimer");
    }

    IEnumerator SwipeTimer()
    {
        animator.SetBool("isStay", false);
        yield return new WaitForSeconds(soaring_time);
        StopCoroutine("SwipeTimer");
        StartCoroutine("SoaringTimer");
    }
}
