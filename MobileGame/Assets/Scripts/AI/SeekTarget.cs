using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTarget : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask targetLayer;

    public Transform Seek()
    {
        Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, radius, targetLayer);

        if(targetCollider != null)
            return targetCollider.transform;
        else
            return null;
    }
}
