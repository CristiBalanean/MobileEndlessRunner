using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceBehavior : MonoBehaviour
{
    [SerializeField] private float separationRadius = 2f;
    [SerializeField] private float separationForce = 5f;
    [SerializeField] private float damping = 0.1f; // Adjust damping factor
    [SerializeField] private LayerMask layerMask;

    private Rigidbody2D rigidBody;
    private Vector2 currentVelocity;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 separationDirection = CalculateSeparationDirection();
        Vector2 separationForceVector = separationDirection * separationForce;

        // Smooth out the force application using damping
        currentVelocity = Vector2.Lerp(currentVelocity, separationForceVector, damping);
        rigidBody.AddForce(currentVelocity);
    }

    private Vector2 CalculateSeparationDirection()
    {
        Vector2 separationDirection = Vector2.zero;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, separationRadius, layerMask);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject) // Exclude self
            {
                Vector2 directionToOther = transform.position - collider.transform.position;
                float distance = directionToOther.magnitude;

                float separationWeight = 1f - (distance / separationRadius);
                separationDirection += directionToOther.normalized * separationWeight;
            }
        }

        return separationDirection.normalized;
    }
}
