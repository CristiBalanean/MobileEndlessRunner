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

    private Vector3 smoothAcceleration;
    private Vector3 filteredAcceleration;

    [SerializeField] private float currentSpeed;

    [SerializeField] private float tiltSpeed = 240f;
    [SerializeField] private float deadZone = 0.2f; // Dead zone to avoid jitter
    [SerializeField] private float filterStrength = 0.3f;

    [SerializeField] private float topSpeed;
    [SerializeField] private float acceleration;

    [SerializeField] private float brakeSpeed;

    [SerializeField] private bool accelerating = false;
    [SerializeField] private bool braking = false;

    [SerializeField] private TMP_Text speedText;

    [SerializeField] private CameraShake cameraShake; // Reference to the CameraShake script

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 rawAcceleration = GetAveragedAcceleration();

        // Apply the low-pass filter
        filteredAcceleration = Vector3.Lerp(filteredAcceleration, rawAcceleration, filterStrength);

        // Use the filtered acceleration for controls
        dirX = filteredAcceleration.x * smoothingFactor * tiltSpeed * Time.deltaTime;

        dirX = Mathf.Clamp(dirX, -7.5f, 7.5f);

        if (Mathf.Abs(rawAcceleration.x) < deadZone)
        {
            rawAcceleration.x = 0f;
        }

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

        if(accelerating && !braking) 
        {
            if(currentSpeed < topSpeed)
                currentSpeed += acceleration * Time.fixedDeltaTime;
        }

        if(braking)
        {
            if(currentSpeed > 25)
                currentSpeed -= brakeSpeed * Time.fixedDeltaTime;
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
}
