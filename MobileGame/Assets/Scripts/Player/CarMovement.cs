using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public static CarMovement Instance;

    public delegate void SpeedChangeHandler(float speedChangeEvent);
    public static event SpeedChangeHandler OnSpeedChange;

    private Rigidbody2D rigidBody;
    public float speedMultiplier;
    private float currentSpeed;
    private float currentHandling;
    private Vector2 input;
    public bool hasDied = false; //put it in the car health script

    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private Car player;

    private float topSpeed;
    private float acceleration;
    private float brakePower;
    private float lowSpeedHandling;
    private float highSpeedHandling;

    private float frictionCoefficient = 2f; // Adjust this value to control friction strength
    private float handlingMultiplier = 5f; // Adjust this value to control handling strength
    private float decelerationExponent = 2f;
    private float frictionCoefficientBrake = .75f;

    private void Awake()
    {
        Instance = this;

        rigidBody = GetComponent<Rigidbody2D>();
        player = CarData.Instance.currentCar;

        InitializeCar();
    }

    private void Start()
    {
        float speedLerp = Mathf.Clamp01((topSpeed - 100) / 200);
        speedMultiplier = Mathf.Lerp(0, 175, speedLerp);
    }

    private void Update()
    {
        input = InputManager.Instance.HandleInput();
        
        ClampCarPositionHorizontal();
        UpdateCurrentSpeed();
    }

    private void FixedUpdate()
    {
        if (!hasDied)
        {
            CarVerticalMovement();
            TiltCar();
            CarHandling();
            ShakeCamera();
        }
        else
            rigidBody.velocity = Vector2.zero;
    }


    private void InitializeCar()
    {
        topSpeed = player.GetTopSpeed();
        acceleration = player.GetAcceleration();
        brakePower = player.GetBrakingPower();
        lowSpeedHandling = player.GetLowSpeedHandling();
        highSpeedHandling = player.GetHighSpeedHandling();
        GetComponent<SpriteRenderer>().sprite = player.GetSprite();
        GameObject collider = Instantiate(player.GetColliderPrefab(), transform.position, Quaternion.identity);
        collider.transform.parent = transform;
    }

    private void ClampCarPositionHorizontal()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -2.0f, 2.0f);
        transform.position = pos;
    }

    private void UpdateCurrentSpeed()
    {
        currentSpeed = rigidBody.velocity.magnitude * 3.6f;

        if (currentSpeed > topSpeed)
            currentSpeed = topSpeed;
        else if (currentSpeed < 25)
        {
            currentSpeed = 25;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 25 / 3.6f);
        }

        OnSpeedChange?.Invoke(currentSpeed);
    }

    private void CarHandling()
    {
        currentHandling = Mathf.Lerp(lowSpeedHandling, highSpeedHandling, currentSpeed / topSpeed);

        // Calculate the target horizontal velocity based on input and handling
        float targetVelocityX = input.x * currentHandling;

        // Calculate the horizontal velocity change required to reach the target
        float velocityChangeX = targetVelocityX - rigidBody.velocity.x;

        // Applying friction in the horizontal direction
        Vector2 frictionForce = -rigidBody.velocity.normalized * frictionCoefficient;
        rigidBody.AddForce(frictionForce, ForceMode2D.Force);

        // Applying the handling force with friction only in the horizontal direction
        Vector2 handlingForce = new Vector2(velocityChangeX * handlingMultiplier, 0f);
        rigidBody.AddForce(handlingForce, ForceMode2D.Force);
    }

    private void TiltCar()
    {
        if (transform.position.x == -2f || transform.position.x == 2f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 10f);
        }
        else
        {
            float tiltAngle = Mathf.Lerp(-2.5f, 2.5f, (rigidBody.velocity.x + 1f) / 2f);
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, -tiltAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
    }
    private void ShakeCamera()
    {
        float shakeMagnitude = Mathf.Clamp((GetSpeed() - 75f) * 0.01f, 0f, 0.0125f);
        cameraShake.Shake(0.1f, shakeMagnitude, 1.5f);
    }

    private void CarVerticalMovement()
    {
        float currentVelocityY = rigidBody.velocity.y;

        // Calculate the ratio of current speed to top speed
        float speedRatio = Mathf.Clamp01(currentVelocityY / (topSpeed / 3.6f));

        // Calculate effective acceleration with friction using an exponential function
        float decelerationFactor = 1 - Mathf.Pow(speedRatio, decelerationExponent);
        decelerationFactor = Mathf.Clamp(decelerationFactor, 0.275f, 1);
        float effectiveAcceleration = acceleration * decelerationFactor;

        // Calculate brake friction based on the current velocity (adjust the frictionCoefficientBrake value)
        float brakeFriction = Mathf.Abs(currentVelocityY) * frictionCoefficientBrake;

        // Applying engine acceleration or brake force based on input.y
        if (input.y == 1 && currentVelocityY < topSpeed / 3.6f)
        {
            // Apply engine acceleration with friction
            Vector2 engineAccelerationForce = transform.up * input.y * effectiveAcceleration;
            rigidBody.AddForce(engineAccelerationForce, ForceMode2D.Force);
        }
        else if (input.y == -1 && currentVelocityY > 25 / 3.6f)
        {
            // Calculate the total brake force, including brake power and brake friction
            float totalBrakeForce = brakePower - brakeFriction;
            totalBrakeForce = Mathf.Max(totalBrakeForce, 0f); // Ensure brake force is non-negative

            // Apply brake force with friction
            Vector2 engineBrakeForce = transform.up * input.y * totalBrakeForce;
            rigidBody.AddForce(engineBrakeForce, ForceMode2D.Force);
        }
    }

    public float GetSpeed()
    {
        return currentSpeed;
    }

    public float GetTopSpeed()
    {
        return topSpeed;
    }
}
