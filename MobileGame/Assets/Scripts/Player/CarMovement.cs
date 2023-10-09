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
    private float acceleration;
    private float brakePower;
    private float lowSpeedHandling;
    private float highSpeedHandling;

    private bool accelerating = false;
    private bool braking = false;

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
        acceleration = player.GetAcceleration();
        brakePower = player.GetBrakingPower();
        lowSpeedHandling = player.GetLowSpeedHandling();
        highSpeedHandling = player.GetHighSpeedHandling();
        GetComponent<SpriteRenderer>().sprite = player.GetSprite();
    }

    private void Start()
    {
        speedMultiplier = Mathf.Lerp(1f, 1.75f, topSpeed / 200);
    }

    void Update()
    {
        OnSpeedChange?.Invoke(currentSpeed);

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -1.25f, 1.25f), transform.position.y);

        float normalizedSpeed = Mathf.Clamp01((currentSpeed - 25f) / (topSpeed - 25f));
        float currentHandling = Mathf.Lerp(lowSpeedHandling, highSpeedHandling, normalizedSpeed);

        dirX = InputManager.Instance.HandleInput();
        dirX = Mathf.Clamp(dirX, -currentHandling, currentHandling);

        float shakeMagnitude = Mathf.Clamp((GetSpeed() - 75f) * 0.01f, 0f, 0.0125f);
        cameraShake.Shake(0.1f, shakeMagnitude, 1.5f);

        if (currentSpeed > topSpeed)
            currentSpeed = topSpeed;
        else if (currentSpeed < 25f)
            currentSpeed = 25f;
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(dirX, currentSpeed/3.6f);

        if (transform.position.x == -1.25f || transform.position.x == 1.25f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 10f);
        }
        else
        {
            float tiltAngle = Mathf.Lerp(-1.5f, 1.5f, (dirX + 1f) / 2f);
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, -tiltAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }

        if ((accelerating || InputManager.Instance.currentInput == InputTypes.TOUCH) && !braking) 
        {
            if(currentSpeed < topSpeed)
                currentSpeed += acceleration * Time.fixedDeltaTime;
        }

        if(braking)
        {
            if(currentSpeed > 25)
                currentSpeed -= brakePower * Time.fixedDeltaTime;
        }

        if(!accelerating && !braking)
        {
            if(currentSpeed > 25)
                currentSpeed -= Time.fixedDeltaTime;
        }
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
        return acceleration;
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
        acceleration = newAcceleration;
    }

    public float GetCameraNormalizedSpeed()
    {
        return Mathf.Clamp01((currentSpeed) / 200f);
    }
}
