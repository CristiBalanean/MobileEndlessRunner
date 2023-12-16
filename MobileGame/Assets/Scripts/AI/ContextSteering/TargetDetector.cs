using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField] private float detectionRange;
    [SerializeField] private LayerMask obstacleLayerMask, playerLayerMask;
    [SerializeField] private bool showGizmos = true;

    private List<Transform> colliders;

    public override void Detect(AIData aiData)
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayerMask);

        if(playerCollider != null)
        {
            Vector2 playerDirection = (playerCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, detectionRange, obstacleLayerMask);

            //we check if what we hit is the player
            if(hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.DrawRay(transform.position, playerDirection * detectionRange, Color.magenta);
                colliders = new List<Transform>() { playerCollider.transform };
            }
            else
            {
                colliders = null;
            }
        }
        else
        {
            colliders = null;
        }

        aiData.targets = colliders;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos == false)
            return;

        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (colliders == null)
            return;
        Gizmos.color = Color.magenta;
        foreach (var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}
