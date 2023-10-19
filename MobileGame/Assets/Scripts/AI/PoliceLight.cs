using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceLight : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.enabled = false;
        StartCoroutine(StartPoliceLights());
    }

    private IEnumerator StartPoliceLights()
    {
        float delay = Random.Range(0.1f, 0.4f);
        yield return new WaitForSeconds(delay);
        animator.enabled = true;
    }
}
