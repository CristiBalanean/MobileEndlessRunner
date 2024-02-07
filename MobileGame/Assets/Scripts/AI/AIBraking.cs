using UnityEngine;

public class AIBraking : MonoBehaviour
{
    [SerializeField] float safeDistance;
    [SerializeField] float acceleration = 5f;
    [SerializeField] float brakingForce = 10f;
    [SerializeField] float proportionalGain = 1f;
    [SerializeField] float integralGain = 0.1f;
    [SerializeField] float derivativeGain = 0.1f;
    [SerializeField] LayerMask obstacleLayer;

    private Rigidbody2D rigidBody;
    private float integral = 0f;
    private float previousError = 0f;

    public bool hasSomethingInFront = false;

    private void OnEnable()
    {
        safeDistance = Random.Range(1.75f, 2.5f);
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        DetectObstaclesInFront();
    }

    private void DetectObstaclesInFront()
    {
        RaycastHit2D[] hitInfos = Physics2D.RaycastAll(transform.position, transform.up, 7.5f, obstacleLayer);

        // Reset the flag before checking for obstacles
        hasSomethingInFront = false;

        foreach (RaycastHit2D hit in hitInfos)
        {
            if (hit.transform != transform) // Exclude the current car from the detected obstacles
            {
                float distanceToObstacle = Vector2.Distance(transform.position, hit.point);
                float targetSpeed = hit.rigidbody.velocity.magnitude;

                // Check if the obstacle is moving slower or at the same speed
                if (targetSpeed <= rigidBody.velocity.magnitude)
                {
                    float speedError = targetSpeed - rigidBody.velocity.magnitude;
                    float accelerationForce = 0f;

                    if (distanceToObstacle < safeDistance)
                    {
                        // Apply braking force to maintain safe distance
                        accelerationForce = -brakingForce;
                    }
                    else
                    {
                        // Use PID controller to match the velocity of the car in front
                        float proportional = speedError * proportionalGain;
                        integral += speedError * Time.fixedDeltaTime;
                        float derivative = (speedError - previousError) / Time.fixedDeltaTime;

                        // Calculate acceleration using PID control
                        accelerationForce = proportional + integral * integralGain + derivative * derivativeGain;

                        // Clamp the acceleration to prevent exceeding the maximum speed
                        accelerationForce = Mathf.Clamp(accelerationForce, -brakingForce, acceleration);
                    }

                    // Apply the calculated acceleration
                    rigidBody.AddForce(transform.up * accelerationForce);
                    if (GetComponent<Obstacle>() != null)
                        GetComponent<Obstacle>().currentSpeed = rigidBody.velocity.magnitude;
                    previousError = speedError;

                    // Set the flag to indicate that there's something in front
                    hasSomethingInFront = true;
                    return;
                }
            }
        }
    }
}
