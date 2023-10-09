using UnityEngine;

public class AIBraking : MonoBehaviour
{
    [SerializeField] float safeDistance;
    [SerializeField] float brakeForce = 20f;
    [SerializeField] Transform detector;
    [SerializeField] LayerMask obstacleLayer;

    private Rigidbody2D rigidBody;

    public bool braking;

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
        RaycastHit2D[] hitInfos = Physics2D.RaycastAll(transform.position, Vector2.up, 15f, obstacleLayer);

        foreach (RaycastHit2D hit in hitInfos)
        {
            if (hit.transform != transform) // Exclude the current car from the detected obstacles
            {
                float distanceToObstacle = Vector2.Distance(transform.position, hit.point);

                braking = distanceToObstacle < safeDistance;
                ApplyBrakingForce(distanceToObstacle);
                return; // Exit the method after detecting the first obstacle
            }
        }

        braking = false; // No obstacles detected
    }

    private void ApplyBrakingForce(float distanceToObstacle)
    {
        Vector2 brakeDirection = -transform.up; // Get the opposite direction of the car
        Vector2 brakeForceVector = brakeDirection * brakeForce; // Calculate the maximum braking force vector

        if (distanceToObstacle < safeDistance)
        {
            float brakingFactor = 1f - (distanceToObstacle / safeDistance); // Calculate a braking factor based on the distance
            Vector2 actualBrakeForce = brakeForceVector * brakingFactor; // Apply reduced braking force based on the distance
            rigidBody.AddForce(actualBrakeForce); // Apply the reduced braking force
        }
        else if (distanceToObstacle < safeDistance * 1.25f)
        {
            // Maintain current speed when tailing (optional: you can add a speed factor if needed)
            // You can also add logic here to match the speed of the car in front
            float currentSpeed = Vector2.Dot(rigidBody.velocity, brakeDirection);
            rigidBody.velocity = brakeDirection * currentSpeed;
        }
        else
        {
            rigidBody.AddForce(brakeForceVector); // Apply maximum braking force if the distance is greater than safeDistance * 2
        }
    }
}
