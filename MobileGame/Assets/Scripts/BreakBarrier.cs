using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBarrier : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" || collision.transform.tag == "Police")
            animator.SetTrigger("Break");
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
