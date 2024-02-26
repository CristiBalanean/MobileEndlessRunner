using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterLightFollow : MonoBehaviour
{
    Transform helicopterTransform;

    [SerializeField] private float radius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float followSpeed = 5f;

    void Start()
    {
        helicopterTransform = transform.root;
    }

    void Update()
    {
        var playerCollider = Physics2D.OverlapCircle(helicopterTransform.position, radius, playerLayer);

        if(playerCollider != null) 
        {
            FollowTarget(playerCollider.transform);
        }
    }

    private void FollowTarget(Transform target)
    {
        // Calculate the position to move towards using lerp
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
