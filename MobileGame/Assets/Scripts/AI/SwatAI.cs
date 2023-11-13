using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatAI : MonoBehaviour
{
    private float topSpeed;
    private float acceleration;

    private Transform playerTransform;
    private Rigidbody2D rigidBody;
    private PoliceAiHealth health;

    private float maxTiltAngle = 5f; // The maximum tilt angle in degrees
    private float tiltSpeed = 3f; // The speed at which the AI tilts

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        rigidBody = GetComponent<Rigidbody2D>();
        health = GetComponent<PoliceAiHealth>();

        topSpeed = CarMovement.Instance.GetTopSpeed() + 7.5f;
        acceleration = CarMovement.Instance.acceleration + 5f;
    }

    private void Start()
    {
        rigidBody.velocity = CarMovement.Instance.GetSpeed() / 3.6f * Vector2.up;
    }

    private void Update()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -2.0f, 2.0f);
        transform.position = pos;
    }

    private void FixedUpdate()
    {
        if (!health.hasDied)
        {
            TiltCar();

            float distance = Vector2.Distance(transform.position, playerTransform.position);
            if (distance < 1.5f)
            {
                //We start ramming the player
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                float rammingForceX = directionToPlayer.x * 100f;
                Vector2 rammingForce = new Vector2(rammingForceX, 0);

                rigidBody.AddForce(rammingForce, ForceMode2D.Force);
            }
            else
            {
                //we accelerate is ai is behind player
                if (transform.position.y < playerTransform.position.y && rigidBody.velocity.y < topSpeed / 3.6f)
                {
                    Vector3 accelerationForce = transform.up * acceleration;
                    rigidBody.AddForce(accelerationForce, ForceMode2D.Force);
                }
                //we brake if ai is in front of player
                else if (playerTransform.position.y < transform.position.y - .5f)
                {
                    Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                    Vector2 stoppingForce = new Vector2(directionToPlayer.x * 5f, -50);

                    if (rigidBody.velocity.y > 0)
                    {
                        rigidBody.AddForce(stoppingForce, ForceMode2D.Force);
                    }
                    else
                        rigidBody.velocity = Vector2.zero;
                    return;
                }
            }
        }
        else
            rigidBody.velocity = Vector2.zero;
    }

    private void TiltCar()
    {
        float targetTiltAngle = -rigidBody.velocity.x * maxTiltAngle;

        // Smoothly tilt the AI towards the target tilt angle
        float currentTiltAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetTiltAngle, ref tiltVelocity, tiltSpeed * Time.fixedDeltaTime);

        // Apply the tilt rotation
        transform.rotation = Quaternion.Euler(0f, 0f, currentTiltAngle);
    }

    private float tiltVelocity; // Variable to store the tilt velocity for SmoothDampAngle
}
