using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CarMovement : MonoBehaviour
{
    public static CarMovement Instance;

    public delegate void SpeedChangeHandler(float speedChangeEvent);
    public static event SpeedChangeHandler OnSpeedChange;

    public float speedMultiplier;

    private Rigidbody2D rigidBody;

    private float dirX;

    private float topSpeed;
    private float baseAcceleration;
    public float currentAcceleration;
    private float brakePower;
    private float lowSpeedHandling;
    private float highSpeedHandling;
    private float[] gearRatios = { 0.25f, 0.45f, 0.65f, 0.85f, 1f };
    public float[] gearAccelerations;

    public int currentGearIndex;

    public bool accelerating = false;
    private bool braking = false;
    private bool canAccelerate = true;

    [SerializeField] private Car player;

    [SerializeField] private float currentSpeed;

    [SerializeField] private CameraShake cameraShake;

    [SerializeField] private GameObject tilt;
    [SerializeField] private GameObject touch;

    private void Awake()
    {
        Instance = this;

        rigidBody = GetComponent<Rigidbody2D>();

        player = CarData.Instance.currentCar;

        topSpeed = player.GetTopSpeed();
        baseAcceleration = player.GetAcceleration();
        brakePower = player.GetBrakingPower();
        lowSpeedHandling = player.GetLowSpeedHandling();
        highSpeedHandling = player.GetHighSpeedHandling();
        GetComponent<SpriteRenderer>().sprite = player.GetSprite();
    }

    private void Start()
    {
        if (InputManager.Instance.currentInput == InputTypes.TOUCH)
            accelerating = true;

        float speedLerp = Mathf.Clamp01((topSpeed - 100) / 200);
        speedMultiplier = Mathf.Lerp(0, 175, speedLerp);

        currentGearIndex = 0;
        currentAcceleration = baseAcceleration;
        gearAccelerations = new float[gearRatios.Length];

        for (int i = 1; i < gearRatios.Length; i++)
        {
            gearAccelerations[i] = baseAcceleration - baseAcceleration * gearRatios[i];
        }
        gearAccelerations[0] = baseAcceleration;
    }

    void Update()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -2f, 2f), transform.position.y);
        dirX = InputManager.Instance.HandleInput();
        dirX = Mathf.Clamp(dirX, -Handling(), Handling());

        ShakeCamera();

        if (currentSpeed > topSpeed)
            currentSpeed = topSpeed;
        else if (currentSpeed < 25f)
            currentSpeed = 25f;

        OnSpeedChange?.Invoke(currentSpeed);
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(dirX, currentSpeed/3.6f);

        TiltCar();
        ManageMovement();
    }

    private float Handling()
    {
        float normalizedSpeed = Mathf.Clamp01((currentSpeed - 25f) / (topSpeed - 25f));
        float currentHandling = Mathf.Lerp(lowSpeedHandling, highSpeedHandling, normalizedSpeed);
        return currentHandling;
    }

    private void ShakeCamera()
    {
        float shakeMagnitude = Mathf.Clamp((GetSpeed() - 75f) * 0.01f, 0f, 0.0125f);
        cameraShake.Shake(0.1f, shakeMagnitude, 1.5f);
    }

    private void TiltCar()
    {
        if (transform.position.x == -2f || transform.position.x == 2f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 10f);
        }
        else
        {
            float tiltAngle = Mathf.Lerp(-1.5f, 1.5f, (dirX + 1f) / 2f);
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, -tiltAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
    }

    private void ManageMovement()
    {
        if (accelerating && !braking && canAccelerate)
        {
            if (currentSpeed < topSpeed)
            {
                currentSpeed += currentAcceleration * Time.fixedDeltaTime;

                // Check if the next gear should be engaged
                if (currentGearIndex < gearRatios.Length - 1 && currentSpeed / topSpeed >= gearRatios[currentGearIndex + 1])
                {
                    // Prevent further acceleration during gear change
                    canAccelerate = false;
                    StartCoroutine(ChangeToHigherGear());
                }
            }
        }

        if (braking)
        {
            if (currentSpeed > 25)
                currentSpeed -= brakePower * Time.fixedDeltaTime;

            ChangeToLowerGear();
        }

        if (!accelerating && !braking && canAccelerate)
        {
            if (currentSpeed > 25)
                currentSpeed -= Time.fixedDeltaTime;

            ChangeToLowerGear();
        }
    }

    private IEnumerator ChangeToHigherGear()
    {
        currentGearIndex++;

        yield return new WaitForSeconds(1f);
        currentAcceleration = gearAccelerations[currentGearIndex];
        canAccelerate = true;
    }

    private void ChangeToLowerGear()
    {
        if (currentGearIndex > 0 && currentSpeed / topSpeed <= gearRatios[currentGearIndex])
        {
            currentGearIndex--;
            currentAcceleration = gearAccelerations[currentGearIndex];
        }
    }

    public void ChangeInputType()
    {
        if (InputManager.Instance.currentInput == InputTypes.TILT)
            accelerating = false;
        else
            accelerating = true;
    }

    public void Accelerate()
    {
        accelerating = true;
    }

    public void AccelerateOff()
    {
        accelerating = false;
    }

    public void Brake()
    {
        braking = true;
    }

    public void BrakeOff()
    {
        braking = false;
    }

    public float GetSpeed()
    {
        return currentSpeed;
    }

    public float GetAcceleration()
    {
        return baseAcceleration;
    }

    public void SetTopSpeed(float newSpeed)
    {
        topSpeed = newSpeed;
    }

    public float GetTopSpeed()
    {
        return topSpeed;
    }

    public void SetAcceleration(float newAcceleration)
    {
        baseAcceleration = newAcceleration;
    }

    public float GetCameraNormalizedSpeed()
    {
        return Mathf.Clamp01((currentSpeed) / 200f);
    }
}
