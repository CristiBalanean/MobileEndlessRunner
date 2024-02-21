using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMovement : MonoBehaviour
{
    public static CarMovement Instance;

    public delegate void SpeedChangeHandler(float speedChangeEvent);
    public static event SpeedChangeHandler OnSpeedChange;

    private Rigidbody2D rigidBody;
    public float speedMultiplier;
    private float currentSpeed;
    public float currentHandling;
    private Vector2 input;
    public bool hasDied = false; //put it in the car health script
    private PowerupManager powerupManager;

    [SerializeField] private CameraShake cameraShake;
    public Car player;
    [SerializeField] private AnimationCurve accelerationCurve;

    private float topSpeed;
    public float acceleration;
    private float brakePower;
    private float lowSpeedHandling;
    private float highSpeedHandling;

    private float frictionCoefficient = 2f; // Adjust this value to control friction strength
    private float handlingMultiplier = 5f; // Adjust this value to control handling strength
    private float brakeHoldDuration = 0f;
    private float maxBrakeHoldDuration = 1f; // Adjust this value for the maximum duration the brake is held
    private float accelerationHoldDuration = 0;
    private float maxAccelerationHoldDuration = .75f;
    public float maxYVelocity;
    private float minYVelocity;

    private void Awake()
    {
        Instance = this;

        rigidBody = GetComponent<Rigidbody2D>();
        powerupManager = GetComponent<PowerupManager>();
        if (SceneManager.GetActiveScene().name != "MonsterTruckGameMode")
            player = CarData.Instance.currentCar;

        InitializeCar();
    }

    private void Start()
    {
        //float speedLerp = Mathf.Clamp01((topSpeed - 100) / 200);
        //speedMultiplier = Mathf.Lerp(0, 175, speedLerp);
        SoundManager.instance.Play("Engine");
        float speedRatio = (topSpeed - 100f) / 210f;
        maxYVelocity = Mathf.Lerp(30, 50, speedRatio);
        minYVelocity = 5;
        Debug.Log(maxYVelocity);
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

        if (SceneManager.GetActiveScene().name != "MonsterTruckGameMode")
        {
            GameObject collider = Instantiate(player.GetColliderPrefab(), transform.position, Quaternion.identity);
            collider.transform.parent = transform;
        }
    }

    private void ClampCarPositionHorizontal()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -2.0f, 2.0f);
        transform.position = pos;
    }

    private void UpdateCurrentSpeed()
    {
        float speedRatio = (rigidBody.velocity.y - 5) / (maxYVelocity-5);
        Debug.Log(speedRatio);
        float displayedSpeed = Mathf.Lerp(25, topSpeed, speedRatio);

        currentSpeed = Mathf.Max(displayedSpeed, 25);

        if (currentSpeed > topSpeed)
        {
            currentSpeed = topSpeed;
        }

        OnSpeedChange?.Invoke(currentSpeed);
    }

    private void CarHandling()
    {
        float adjustedTimeScale = 1f / Time.timeScale;

        currentHandling = Mathf.Lerp(lowSpeedHandling, highSpeedHandling, currentSpeed / topSpeed);

        // Calculate the target horizontal velocity based on input and handling
        float targetVelocityX = input.x * currentHandling * adjustedTimeScale;

        // Calculate the horizontal velocity change required to reach the target
        float velocityChangeX = targetVelocityX - rigidBody.velocity.x;

        // Applying friction in the horizontal direction (only)
        Vector2 frictionForce = -rigidBody.velocity.normalized * (frictionCoefficient * adjustedTimeScale);
        frictionForce.y = 0f; // Ensure no y-component
        rigidBody.AddForce(frictionForce, ForceMode2D.Force);

        // Applying the handling force only in the horizontal direction
        Vector2 handlingForce = new Vector2(velocityChangeX * handlingMultiplier, 0f) * adjustedTimeScale;
        rigidBody.AddForce(handlingForce, ForceMode2D.Force);
    }

    private void TiltCar()
    {
        if (transform.position.x == -2f || transform.position.x == 2f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedUnscaledDeltaTime * 10f);
        }
        else
        {
            float tiltAngle = Mathf.Lerp(-2.5f, 2.5f, (rigidBody.velocity.x + 1f) / 2f);
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, -tiltAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedUnscaledDeltaTime * 5f);
        }
    }
    private void ShakeCamera()
    {
        float shakeMagnitude = Mathf.Clamp((GetSpeed() - 75f) * 0.01f, 0f, 0.0125f);
        cameraShake.Shake(0.1f, shakeMagnitude * 1.5f, 1.5f);
    }

    private void CarVerticalMovement()
    {
        float currentVelocityY = rigidBody.velocity.y;

        // Calculate the ratio of current speed to top speed
        float speedRatio = Mathf.Clamp01(currentVelocityY / (topSpeed / 5f));

        // Calculate effective acceleration with friction using an exponential function
        float decelerationFactor = accelerationCurve.Evaluate(speedRatio);
        decelerationFactor = Mathf.Clamp(decelerationFactor, 0.05f, 1);
        float effectiveAcceleration = acceleration * decelerationFactor;

        // Set the Y velocity to the maximum between the current velocity and minYVelocity
        float targetVelocityY = Mathf.Clamp(currentVelocityY, minYVelocity, maxYVelocity);

        // Applying engine acceleration or brake force based on input.y
        if (input.y == 1 && currentVelocityY < maxYVelocity)
        {
            accelerationHoldDuration += Time.deltaTime;

            float accelerationForce = Mathf.Lerp(0, effectiveAcceleration, accelerationHoldDuration / maxAccelerationHoldDuration);

            // Apply engine acceleration with friction only in the vertical direction
            Vector2 engineAccelerationForce = Vector2.up * accelerationForce;
            rigidBody.AddForce(engineAccelerationForce, ForceMode2D.Force);

            // Reset brake hold duration when accelerating
            brakeHoldDuration = 0f;
        }
        else if (input.y == -1 && currentSpeed > 25)
        {
            if (SceneManager.GetActiveScene().name != "MonsterTruckGameMode")
            {
                if (powerupManager.currentPowerup is Nitro && powerupManager.isActive)
                {
                    powerupManager.Deactivate();
                }
            }

            // Increase brake hold duration
            brakeHoldDuration += Time.deltaTime;

            // Calculate brake force based on the hold duration (linear increase)
            float brakeForce = Mathf.Lerp(0, brakePower, brakeHoldDuration / maxBrakeHoldDuration);

            // Apply brake force only in the vertical direction
            Vector2 engineBrakeForce = -transform.up * brakeForce;
            rigidBody.AddForce(engineBrakeForce, ForceMode2D.Force);

            accelerationHoldDuration = 0;
        }
        else
        {
            // Reset brake hold duration when not braking
            brakeHoldDuration = 0f;
            accelerationHoldDuration = 0;
        }

        // Set the Y velocity to the target velocity to ensure it doesn't go below minYVelocity or exceed maxYVelocity
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, targetVelocityY);
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
