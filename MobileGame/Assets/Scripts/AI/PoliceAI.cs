using System.Collections;
using UnityEngine;

public class PoliceAI : MonoBehaviour
{
    private Transform playerCarTransform;
    private Rigidbody2D rigidBody;
    private float topSpeed;
    private float acceleration;
    private bool startBraking = false;

    private float maxTiltAngle = 5f; // The maximum tilt angle in degrees
    private float tiltSpeed = 3f; // The speed at which the AI tilts

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        topSpeed = CarMovement.Instance.GetTopSpeed() + 7.5f;
        acceleration = CarMovement.Instance.acceleration + 5f;
        playerCarTransform = CarMovement.Instance.transform;
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

        if (Vector2.Distance(transform.position, playerCarTransform.position) > 12)
        {
            PoliceSpawning.instance.RemovePoliceCar(gameObject);
            Destroy(gameObject, 1.5f);
        }
    }

    private void FixedUpdate()
    {
        TiltCar();

        if(startBraking)
        {
            Vector2 directionToPlayer = (playerCarTransform.position - transform.position).normalized;
            Vector2 stoppingForce = new Vector2(directionToPlayer.x * 5f, -100);

            if (rigidBody.velocity.y > 0)
            {
                rigidBody.AddForce(stoppingForce, ForceMode2D.Force);
            }
            else
                rigidBody.velocity = Vector2.zero;
            return;
        }

        if (playerCarTransform.position.y < transform.position.y - .5f)
        {
            startBraking = true;
        }

        // If the distance is below the ramming distance, try to ram the player
        if (playerCarTransform.position.y < transform.position.y + 1.5f)
        {
            Vector2 directionToPlayer = (playerCarTransform.position - transform.position).normalized;
            float rammingForceX = directionToPlayer.x * 50f;
            Vector2 rammingForce = new Vector2(rammingForceX, 0);

            // Apply the ramming force
            rigidBody.AddForce(rammingForce, ForceMode2D.Force);
        }
        // If the AI is not too close, apply normal acceleration
        else if (rigidBody.velocity.y < topSpeed / 3.6f)
        {
            Vector3 accelerationForce = transform.up * acceleration;
            rigidBody.AddForce(accelerationForce, ForceMode2D.Force);
        }
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
