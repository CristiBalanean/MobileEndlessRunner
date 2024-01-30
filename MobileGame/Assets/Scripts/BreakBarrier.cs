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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.transform.tag == "Player" || collision.transform.tag == "AiCollider") && !IsBreakAnimationPlaying())
        {
            animator.SetTrigger("Break");
            SoundManager.instance.Play("Barrier");
        }
    }

    private bool IsBreakAnimationPlaying()
    {
        // Check if the "Break" animation is currently playing
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Break");
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
