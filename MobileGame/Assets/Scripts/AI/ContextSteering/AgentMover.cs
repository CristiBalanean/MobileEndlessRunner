using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField]
    private float maxSpeed = 2, acceleration = 50, deacceleration = 100;
    [SerializeField]
    private float currentSpeed = 0;
    private Vector2 oldMovementInput;
    public Vector2 MovementInput { get; set; }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        maxSpeed = CarMovement.Instance.GetTopSpeed() / 3.6f + 2.5f;
        acceleration = CarMovement.Instance.acceleration;
        deacceleration = acceleration * 2;
    }

    private void Start()
    {
        rb2d.velocity = CarMovement.Instance.transform.GetComponent<Rigidbody2D>().velocity;
    }

    private void Update()
    {
        ClampXPosition();

        float distance = Vector2.Distance(GameObject.Find("Player").transform.position, transform.position);
        if (distance > 30)
        {
            if(SwatSpawner.instance != null)
                SwatSpawner.instance.RemovePoliceCar(gameObject);
            else if(PoliceSpawning.instance != null)
                PoliceSpawning.instance.RemovePoliceCar(gameObject);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector2 playerPosition = GameObject.Find("Player").transform.position;
        Vector2 directionToPlayer = (playerPosition - (Vector2)transform.position).normalized;

        // If the AI is too far from the player, adjust the movement input to move closer
        float distanceToPlayer = Vector2.Distance(playerPosition, transform.position);
        if (distanceToPlayer > 2.0f)
        {
            MovementInput = directionToPlayer;
        }
        else
        {
            // If close to the player, maintain the current movement input
            MovementInput = oldMovementInput;
        }

        // Acceleration and deceleration logic remains the same
        if (MovementInput.magnitude > 0)
        {
            oldMovementInput = MovementInput.normalized; // Normalize to retain direction
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

        // Use Rigidbody2D velocity instead of AddForce for smoother movement
        rb2d.velocity = oldMovementInput * currentSpeed;

        ClampXPosition();
    }


    private void ClampXPosition()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -2.0f, 2.0f);
        transform.position = pos;
    }
}