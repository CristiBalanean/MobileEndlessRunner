using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatRammerAI : MonoBehaviour
{
    private Transform playerCarTransform;
    private Rigidbody2D rigidBody;
    private PoliceAiHealth health;
    private float speed = 10f;

    private float tiltSpeed = 2f; // The speed at which the AI tilts

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerCarTransform = CarMovement.Instance.transform;
        health = GetComponent<PoliceAiHealth>();
    }

    private void Start()
    {
        rigidBody.velocity = transform.up * speed;
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

            float distance = Vector2.Distance(transform.position, playerCarTransform.position);

            if (distance < 7.5f)
            {
                Vector2 directionToPlayer = (playerCarTransform.position - transform.position).normalized;
                float rammingForceX = directionToPlayer.x * 100f;
                Vector2 rammingForce = new Vector2(rammingForceX, 0);

                rigidBody.AddForce(rammingForce, ForceMode2D.Force);
            }

            if (transform.position.y < playerCarTransform.position.y && distance > 15f)
                Destroy(gameObject);
        }
        else
            rigidBody.velocity = Vector2.zero;
    }

    private void TiltCar()
    {
        // Get the angle between the car's current up direction and its velocity direction
        float angle = Vector2.SignedAngle(transform.up, rigidBody.velocity);

        // Negate the angle to correct the rotation direction
        angle = -angle;

        // Calculate the target tilt angle based on the speed and tilt speed
        float targetTiltAngle = Mathf.LerpAngle(transform.eulerAngles.z, angle, Time.deltaTime * tiltSpeed);

        // Set the target tilt angle directly without clamping
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTiltAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
    }

}
