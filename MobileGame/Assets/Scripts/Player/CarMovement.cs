using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    private float dirX;
    private float smoothingFactor = 0.75f; // Adjust this value to control smoothing intensity

    private float touchDirection;

    private float topSpeed;
    private float acceleration;
    private float brakePower;
    private float lowSpeedHandling;
    private float highSpeedHandling;

    private bool accelerating = false;
    private bool braking = false;

    private int controls;

    private Vector3 filteredAcceleration;

    [SerializeField] private Car player;

    [SerializeField] private float currentSpeed;

    [Header("Movement")]
    [SerializeField] private float tiltSpeed = 240f;
    [SerializeField] private float deadZone = 0.2f;
    [SerializeField] private float filterStrength = 0.3f;

    [SerializeField] private TMP_Text speedText;

    [SerializeField] private CameraShake cameraShake;

    [SerializeField] private GameObject tilt;
    [SerializeField] private GameObject touch;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        player = CarData.Instance.currentCar;

        topSpeed = player.GetTopSpeed();
        acceleration = player.GetAcceleration();
        brakePower = player.GetBrakingPower();
        lowSpeedHandling = player.GetLowSpeedHandling();
        highSpeedHandling = player.GetHighSpeedHandling();

        GetComponent<SpriteRenderer>().sprite = player.GetSprite();

        controls = PlayerPrefs.GetInt("Tilt");
        if(controls == 1)
            touch.SetActive(false);
        else
            tilt.SetActive(false);
    }

    void Update()
    {
        if (controls == 1)
        {
            Vector3 rawAcceleration = GetAveragedAcceleration();

            // Apply the low-pass filter
            filteredAcceleration = Vector3.Lerp(filteredAcceleration, rawAcceleration, filterStrength);

            // Use the filtered acceleration for controls
            dirX = filteredAcceleration.x * smoothingFactor * tiltSpeed * Time.deltaTime;

            if (Mathf.Abs(rawAcceleration.x) < deadZone)
            {
                rawAcceleration.x = 0f;
            }
        }
        else
        {
            dirX = touchDirection * 1000f * Time.deltaTime;
        }

        float normalizedSpeed = Mathf.Clamp01((currentSpeed - 25f) / (topSpeed - 25f));
        float currentHandling = Mathf.Lerp(lowSpeedHandling, highSpeedHandling, normalizedSpeed);
        dirX = Mathf.Clamp(dirX, -currentHandling, currentHandling);

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -1.25f, 1.25f), transform.position.y);

        speedText.text = "SPEED: " + GetSpeed().ToString("F1") + " KM/H";

        float shakeMagnitude = Mathf.Clamp((GetSpeed() - 75f) * 0.01f, 0f, 0.0125f);

        if (currentSpeed > topSpeed)
            currentSpeed = topSpeed;
        else if (currentSpeed < 25f)
            currentSpeed = 25f;

        cameraShake.Shake(0.1f, shakeMagnitude, 1.5f); // Adjust duration and speed as needed
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(dirX, currentSpeed/3.6f);

        if(controls == 0 && !braking)
        {
            if (currentSpeed < topSpeed)
                currentSpeed += acceleration * Time.fixedDeltaTime;
        }

        if(accelerating && !braking) 
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

    public static Vector3 GetAveragedAcceleration()
    {
        float period = 0.0f;
        Vector3 acc = Vector3.zero;

        // Iterate through all accelerometer events in the last frame
        foreach (var evnt in Input.accelerationEvents)
        {
            acc += evnt.acceleration * evnt.deltaTime;
            period += evnt.deltaTime;
        }

        // Ensure that at least one reading was recorded
        if (period > 0)
        {
            // Compute the weighted average
            acc *= 1.0f / period;
        }

        return acc;
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

    public void SetSpeedAtDeath()
    {
        currentSpeed = 0;
    }

    public void SetTiltSpeed(float speed)
    {
        tiltSpeed = speed;
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

    public void DirectionRight() { touchDirection = 1; }
    
    public void DirectionLeft() { touchDirection = -1; }

    public void Straight() { touchDirection = 0; }

    public void ChangeControlsUI()
    {
        controls = PlayerPrefs.GetInt("Tilt");
        if (controls == 1)
        {
            touch.SetActive(false);
            tilt.SetActive(true);
        }
        else
        {
            tilt.SetActive(false);
            touch.SetActive(true);
        }
    }
}
