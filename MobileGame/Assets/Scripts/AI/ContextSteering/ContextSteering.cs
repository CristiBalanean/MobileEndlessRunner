using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ContextSteering : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer, playerLayer;
    [SerializeField] private float targetDetectionRange;
    [SerializeField] private float obstacleDetectionRange;
    [SerializeField] private Transform target;
    [SerializeField] private Collider2D[] obstacles;

    public float[] interest;
    public float[] danger;

    public List<Vector2> directions;

    private Rigidbody2D rigidBody;
    private float topSpeed;
    private float acceleration;

    [SerializeField] private float kpSpeed = 5f;
    [SerializeField] private float kiSpeed = 0.1f;
    [SerializeField] private float kdSpeed = 0.1f;
    [SerializeField] private float maxIntegral = 5f;

    private float integralSpeed = 0f;
    private float previousErrorSpeed = 0f;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        topSpeed = CarMovement.Instance.GetTopSpeed() / 3.6f + 25f;
        acceleration = CarMovement.Instance.acceleration + 20f;
        rigidBody.velocity = Vector2.up * (CarMovement.Instance.GetSpeed() / 3.6f + 10f);

        float rand = Random.Range(0, 2);
        AudioSource audioSource = GetComponentInChildren<AudioSource>();
        audioSource.PlayDelayed(rand);
    }

    private void Start()
    {
        directions = new List<Vector2> { new Vector2 (0, 1).normalized,
                                         new Vector2 (1, 1).normalized,
                                         new Vector2 (-1, 1).normalized,
                                         new Vector2 (0, -1).normalized,
                                         new Vector2 (1, -1).normalized,
                                         new Vector2 (-1, -1).normalized};
        //InvokeRepeating("DespawnCheck", 0f, 1f);
    }

    private void Update()
    {
        Detect();
        ClampXPosition();
    }

    private void FixedUpdate()
    {
        if (PoliceEvent.instance.hasStarted)
        {
            MoveAgent();
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
        }
    }

    private void Detect()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayer);
        if (playerCollider != null)
        {
            target = playerCollider.transform.root;
        }
        else
        {
            target = null;
        }

        // Detect other obstacles excluding the agent itself
        obstacles = Physics2D.OverlapCircleAll(transform.position, obstacleDetectionRange, obstacleLayer)
            .Where(collider => collider.transform.root != transform)  // Filter out the agent's colliders
            .ToArray();
    }

    private (float[] danger, float[] interest) CalculateSteering(float[] danger, float[] interest, Transform target)
    {
        if (target == null)
            return (danger, interest);

        Vector2 targetDirection = (target.position - transform.position).normalized;

        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(targetDirection, directions[i]);
            float valueToPutIn = result;

            if (valueToPutIn > interest[i])
                interest[i] = valueToPutIn;
        }
        return (danger, interest);
    }

    private (float[] danger, float[] interest) CalculateAvoidance(float[] danger, float[] interest, Collider2D[] obstacles)
    {
        float maxAvoidanceWeight = 100000.0f; // Set a high value for maximum avoidance weight

        foreach (var obstacle in obstacles)
        {
            Vector2 obstacleDirection = (obstacle.transform.position - transform.position).normalized;

            for (int i = 0; i < danger.Length; i++)
            {
                float result = Vector2.Dot(obstacleDirection, directions[i]);
                float valueToPutIn = result * maxAvoidanceWeight; // Set danger to a high value

                if (valueToPutIn > danger[i])
                    danger[i] = valueToPutIn;
            }
        }
        return (danger, interest);
    }

    private Vector2 DirectionToMove()
    {
        interest = new float[6];
        danger = new float[6];

        (danger, interest) = CalculateSteering(danger, interest, target);
        (danger, interest) = CalculateAvoidance(danger, interest, obstacles);

        Vector2 outputDirection = Vector2.zero;
        for (int i = 0; i < interest.Length; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        for (int i = 0; i < directions.Count; i++)
        {
            outputDirection += directions[i] * interest[i];
        }
        return outputDirection.normalized;
    }

    private void MoveAgent()
    {
        if (target == null)
            return;

        // Get the desired direction from the context steering
        Vector2 desiredDirection = DirectionToMove();

        // Distance error for PID speed control
        float distanceErrorSpeed = Vector2.Distance(transform.position, target.position);

        // Update integral term for speed control with anti-windup
        integralSpeed += distanceErrorSpeed * Time.fixedDeltaTime;
        integralSpeed = Mathf.Clamp(integralSpeed, -maxIntegral, maxIntegral);

        // Calculate PID output for speed control
        float pidOutputSpeed = kpSpeed * distanceErrorSpeed + kiSpeed * integralSpeed + kdSpeed * (distanceErrorSpeed - previousErrorSpeed);

        // Update previous error for speed control
        previousErrorSpeed = distanceErrorSpeed;

        // Apply PID output to control speed
        float desiredSpeed = topSpeed * Mathf.Clamp01(pidOutputSpeed); // Assuming topSpeed is the maximum allowed speed
        float currentSpeed = Vector2.Dot(rigidBody.velocity, transform.up);

        // Adjust the speed based on the difference between the current and desired speeds
        float speedError = desiredSpeed - currentSpeed;
        float acceleration = Mathf.Clamp(speedError, -this.acceleration, this.acceleration);

        // Apply acceleration to control speed while considering the desired direction
        rigidBody.AddForce(desiredDirection * acceleration);
    }

    private void ClampXPosition()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -2.0f, 2.0f);
        transform.position = pos;
    }

    private void DespawnCheck()
    {
        if (target == null)
            return;

        if (Vector2.Distance(transform.position, target.position) > 30f)
        {
            Debug.Log("Too far!");

            Destroy(transform.root.gameObject);
        }
    }
}