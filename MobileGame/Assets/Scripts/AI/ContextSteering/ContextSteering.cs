using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        topSpeed = CarMovement.Instance.GetTopSpeed() / 3.6f + 5f;
        rigidBody.velocity = Vector2.up * CarMovement.Instance.GetSpeed() / 3.6f;

        float rand = Random.Range(0, 2);
        AudioSource audioSource = GetComponentInChildren<AudioSource>();
        audioSource.PlayDelayed(rand);
    }

    private void Start()
    {
        directions = new List<Vector2> { new Vector2 (0, 1).normalized,
                                         new Vector2 (1, 1).normalized,
                                         new Vector2 (-1, 1).normalized};

        InvokeRepeating("DespawnCheck", 0f, 1f);
    }

    private void Update()
    {
        Detect();
        ClampXPosition();
    }

    private void FixedUpdate()
    {
        MoveAgent();
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

        for(int i =0; i< interest.Length; i++) 
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
        float avoidanceWeight = 2.0f; // Adjust the weight as needed

        foreach (var obstacle in obstacles)
        {
            Vector2 obstacleDirection = (obstacle.transform.position - transform.position).normalized;

            for(int i =0; i< danger.Length; i++)
            {
                float result = Vector2.Dot(obstacleDirection, directions[i]);
                float valueToPutIn = result * avoidanceWeight;

                if (valueToPutIn > danger[i])
                    danger[i] = valueToPutIn;
            }
        }
        return (danger, interest);
    }

    private Vector2 DirectionToMove()
    {
        interest = new float[3];
        danger = new float[3];

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
            
        Vector2 direction = DirectionToMove();
        Vector2 yDirection = new Vector2(0, direction.y);
        Vector2 xDirection = new Vector2(direction.x, 0);
        rigidBody.AddForce(xDirection * 25f);

        float brakingForce = 25f; // Base braking force, adjust as needed

        if (transform.position.y > target.position.y && rigidBody.velocity.y > 25 / 3.6f)
        {
            Vector2 toPlayer = target.position - transform.position;

            // Calculate a scaling factor based on the distance to the player
            float distanceToPlayer = toPlayer.magnitude;
            float maxBrakingDistance = 5f; // Adjust as needed
            float brakingDistanceScale = Mathf.Clamp01(distanceToPlayer / maxBrakingDistance);

            // Apply braking force scaled by the distance to the player
            float scaledBrakingForce = brakingForce * brakingDistanceScale;


            rigidBody.AddForce(-rigidBody.velocity.normalized * scaledBrakingForce);
        }
        else
        {
            // Otherwise, apply forward and sideways forces
            if (rigidBody.velocity.y < topSpeed)
            {
                rigidBody.AddForce(yDirection * 25f);
            }
        }
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

        if (Vector2.Distance(transform.position, target.position) > 15f)
        {
            if (PoliceSpawning.instance != null)
                PoliceSpawning.instance.RemovePoliceCar(gameObject);
            Destroy(gameObject);
        }
    }
}
