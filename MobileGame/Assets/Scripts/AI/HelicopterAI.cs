using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterAI : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rigidBody;
    private Rigidbody2D targetRigidbody;

    [SerializeField] private float kd;
    [SerializeField] private float kp;

    private void Awake()
    {
        target = CarMovement.Instance.transform;
        targetRigidbody = target.GetComponent<Rigidbody2D>();
        rigidBody = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        InvokeRepeating("CheckForDespawn", 10f, 1f);
    }

    private void FixedUpdate()
    {
        if (PoliceEvent.instance.hasStarted)
        {
            FollowTarget();
        }
        else
        {
            rigidBody.AddForce(Vector2.down * 10);
        }
    }

    private void FollowTarget()
    {
        Vector2 targetPosition = (Vector2)target.position - new Vector2(0, 1.5f);
        Vector2 targetVelocity = targetRigidbody.velocity;

        Vector2 positionError = targetPosition - (Vector2)transform.position;
        Vector2 velocityError = targetVelocity - rigidBody.velocity;
        Vector2 desiredAcceleration = kp * positionError + kd * velocityError;

        rigidBody.AddForce(desiredAcceleration);
    }

    private void CheckForDespawn()
    {
        float distance = Vector2.Distance(transform.position, CarMovement.Instance.transform.position);

        if (distance > 15)
            Destroy(gameObject);
    }
}
