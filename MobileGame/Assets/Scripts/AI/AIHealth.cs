using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    private Animator aiAnimator;
    private Collider2D[] aiCollider;

    private void Awake()
    {
        aiAnimator = GetComponent<Animator>();
        aiCollider = GameObject.FindGameObjectWithTag("AiCollider").GetComponents<Collider2D>();
        aiAnimator.enabled = false;
    }

    private void OnEnable()
    {
        aiAnimator.enabled = false;
        aiAnimator.Rebind();
        aiAnimator.Update(0f);
        foreach (var collider in aiCollider)
            collider.isTrigger = false;
    }

    public IEnumerator DeathTrigger()
    {
        yield return new WaitForSeconds(.1f);
        aiAnimator.enabled = true;
        foreach (var collider in aiCollider)
            collider.isTrigger = true;
        yield return new WaitForSeconds(1.25f);
        gameObject.SetActive(false);
    }
}
