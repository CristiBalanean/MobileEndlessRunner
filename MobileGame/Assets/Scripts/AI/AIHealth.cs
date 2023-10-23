using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    private Animator aiAnimator;
    private BoxCollider2D aiCollider;

    private void Awake()
    {
        aiAnimator = GetComponent<Animator>();
        aiCollider = GetComponent<BoxCollider2D>();
        aiAnimator.enabled = false;
    }

    private void OnEnable()
    {
        aiAnimator.enabled = false;
        aiAnimator.Rebind();
        aiAnimator.Update(0f);
        aiCollider.isTrigger = false;
    }

    public IEnumerator DeathTrigger()
    {
        yield return new WaitForSeconds(.1f);
        aiAnimator.enabled = true;
        yield return new WaitForSeconds(1.25f);
        gameObject.SetActive(false);
    }
}
