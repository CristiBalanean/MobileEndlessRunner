using System.Collections;
using System.Linq;
using UnityEngine;

public class AIBraking : MonoBehaviour
{
    [SerializeField] float safeDistance;

    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] Transform detector;

    private Rigidbody2D rigidBody;
    private RaycastHit2D hitInfo;
    private float targetSpeed;
    private float speed;

    public bool braking;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        safeDistance = Random.Range(2.5f, 4.5f);
    }

    void Update()
    {
        DetectObstaclesInFront();
    }

    private void DetectObstaclesInFront()
    {
        hitInfo = Physics2D.Raycast(detector.position, Vector2.up, 15f, obstacleLayer);

        if (hitInfo)
        {
            float distanceToObstacle = CheckDistance(hitInfo.transform);

            braking = true;

            if (GetComponent<Obstacle>() != null)
            {
                speed = GetComponent<Obstacle>().topSpeed;
            }

            if (hitInfo.transform.GetComponent<Obstacle>() != null)
            {
                targetSpeed = hitInfo.transform.GetComponent<Obstacle>().topSpeed;
            }
            else if (hitInfo.transform.GetComponent<CarMovement>() != null)
            {
                targetSpeed = hitInfo.transform.GetComponent<CarMovement>().GetSpeed();
            }
            else
            {
                return;
            }

            if (distanceToObstacle > safeDistance && distanceToObstacle < safeDistance + 1f)
            {
                if (hitInfo.transform.GetComponent<CarMovement>() == null)
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Lerp(rigidBody.velocity.y, hitInfo.rigidbody.velocity.y, 1f));
                    return;
                }
            }
            else if (distanceToObstacle < safeDistance)
            {
                float speedDifference = targetSpeed - speed;


                float timeToMatch = speedDifference / (-speed);
                float deceleration = speedDifference / timeToMatch;

                float brakingForce = rigidBody.mass * deceleration * 9.8f;

                rigidBody.AddForce(Vector2.up * brakingForce * Time.deltaTime);
            }

            if (transform.GetComponent<Obstacle>() != null)
            {
                transform.GetComponent<Obstacle>().currentSpeed = rigidBody.velocity.magnitude;
            }
        }
        else
        {
            braking = false;
        }
    }

    private float CheckDistance(Transform target)
    {
        return Vector2.Distance(transform.position, target.position);
    }
}
