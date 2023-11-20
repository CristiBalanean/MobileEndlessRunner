using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField] private bool showGizmos = true;

    private Vector2 targetPositionCached;

    bool reachedLastTarget;
    float[] interestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        if (aiData.targets == null || aiData.targets.Count <= 0)
        {
            aiData.currentTarget = null;
            return (danger, interest);
        }
        else
        {
            aiData.currentTarget = aiData.targets.OrderBy(target => Vector2.Distance(target.position, transform.position)).FirstOrDefault();
        }

        if (aiData.currentTarget != null && aiData.targets != null && aiData.targets.Contains(aiData.currentTarget))
        {
            targetPositionCached = (Vector2)aiData.currentTarget.position;
        }

        Vector2 directionToTarget = (targetPositionCached - (Vector2)transform.position);

        float distanceToTarget = directionToTarget.magnitude;
        float minDistanceToPlayer = 3.0f; // Adjust this value based on your requirements

        if (distanceToTarget < minDistanceToPlayer)
        {
            // If too close, move away from the player
            Vector2 avoidanceDirection = -directionToTarget.normalized;

            // Adjust interest based on avoidance direction
            for (int i = 0; i < interest.Length; i++)
            {
                float result = Vector2.Dot(avoidanceDirection, Directions.eightDirections[i]);
                float valueToPutIn = result;

                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }
        else
        {
            // If not too close, continue with regular behavior
            for (int i = 0; i < interest.Length; i++)
            {
                float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);

                // Accept all directions, not just those less than 90 degrees to the target direction
                float valueToPutIn = result;
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }

        interestsTemp = interest;

        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (showGizmos == false)
            return;
        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if (Application.isPlaying && interestsTemp != null)
        {
            if (interestsTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i] * 2);
                }
                if (reachedLastTarget == false)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(targetPositionCached, 0.1f);
                }
            }
        }
    }
}
