using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    private Animator objectAnimator;

    private void Awake()
    {
        objectAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        objectAnimator.Rebind();
        objectAnimator.Update(0f);
    }

    public void Hide()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void HideCurrentObject()
    {
        transform.gameObject.SetActive(false);
    }
}
