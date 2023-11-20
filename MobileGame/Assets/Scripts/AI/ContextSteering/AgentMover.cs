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
        acceleration = CarMovement.Instance.acceleration + 15f;
        deacceleration = acceleration * 2;
    }

    private void Start()
    {
        rb2d.velocity = CarMovement.Instance.transform.GetComponent<Rigidbody2D>().velocity;
    }

    private void Update()
    {
        ClampXPosition();   
    }

    private void FixedUpdate()
    {
        if (MovementInput.magnitude > 0 && currentSpeed >= 0 && rb2d.velocity.y < maxSpeed)
        {
            oldMovementInput = MovementInput;
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        rb2d.AddForce(oldMovementInput * currentSpeed, ForceMode2D.Force);
    }

    private void ClampXPosition()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -2.0f, 2.0f);
        transform.position = pos;
    }
}