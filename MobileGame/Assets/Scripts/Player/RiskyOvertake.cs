using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiskyOvertake : MonoBehaviour
{
    [SerializeField] private float raycastLength;
    [SerializeField] private int raycastNumber;
    [SerializeField] private LayerMask obstacleLayer;  // Specify the layer you want to interact with

    // Track which rays initially hit something
    private HashSet<int> rightInitiallyHitRays = new HashSet<int>();
    private HashSet<int> leftInitiallyHitRays = new HashSet<int>();

    private int rightLastStoppedRay = -1;
    private int leftLastStoppedRay = -1;

    private void Update()
    {
        // Loop for right side
        for (int i = 0; i <= raycastNumber; i++)
        {
            Vector3 rayStart;

            switch (i)
            {
                case 0:
                    rayStart = transform.position + Vector3.up * 0.5f;
                    break;

                case 1:
                    rayStart = transform.position;
                    break;

                case 2:
                    rayStart = transform.position - Vector3.up * 0.5f;  // Use negative offset
                    break;

                default:
                    rayStart = transform.position;  // Default to the transform position
                    break;
            }

            // Use the LayerMask to filter interactions with specific layers
            RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.right, raycastLength, obstacleLayer);
            Debug.DrawRay(rayStart, Vector2.right * raycastLength, hit ? Color.red : Color.green);

            // Update the set if this ray hits something initially
            if (hit)
            {
                rightInitiallyHitRays.Add(i);
            }
            else
            {
                // Update the index of the last ray that stopped hitting
                rightLastStoppedRay = i;
            }
        }

        // Loop for left side
        for (int i = 0; i <= raycastNumber; i++)
        {
            Vector3 rayStart;

            switch (i)
            {
                case 0:
                    rayStart = transform.position + Vector3.up * 0.5f;
                    break;

                case 1:
                    rayStart = transform.position;
                    break;

                case 2:
                    rayStart = transform.position - Vector3.up * 0.5f;  // Use negative offset
                    break;

                default:
                    rayStart = transform.position;  // Default to the transform position
                    break;
            }

            // Use the LayerMask to filter interactions with specific layers
            RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.left, raycastLength, obstacleLayer);
            Debug.DrawRay(rayStart, Vector2.left * raycastLength, hit ? Color.red : Color.green);

            // Update the set if this ray hits something initially
            if (hit)
            {
                leftInitiallyHitRays.Add(i);
            }
            else
            {
                // Update the index of the last ray that stopped hitting
                leftLastStoppedRay = i;
            }
        }

        // Check if all rays on the right side initially hit something
        bool rightSideOvertaken = rightInitiallyHitRays.Count == raycastNumber + 1;

        // Check if all rays on the left side initially hit something
        bool leftSideOvertaken = leftInitiallyHitRays.Count == raycastNumber + 1;

        // Check if the lowest ray from the right side stopped hitting
        bool rightLowestRayStoppedHitting = !Physics2D.Raycast(transform.position + Vector3.up * 0.5f, Vector2.right, raycastLength, obstacleLayer);

        // Check if the lowest ray from the left side stopped hitting
        bool leftLowestRayStoppedHitting = !Physics2D.Raycast(transform.position + Vector3.up * 0.5f, Vector2.left, raycastLength, obstacleLayer);

        // Check if it's a close overtake
        bool closelyOvertaken = (rightSideOvertaken && rightLastStoppedRay == 0 && rightLowestRayStoppedHitting) ||
                     (leftSideOvertaken && leftLastStoppedRay == 0 && leftLowestRayStoppedHitting);

        // Do something with the closelyOvertaken boolean (e.g., reward the player)
        if (closelyOvertaken)
        {
            Debug.Log("Closely overtaken!");
            // Add your close overtake multiplier logic or other rewards here
            if(CarMovement.Instance.GetSpeed() > 75 && !PoliceEvent.instance.hasStarted)
                ScoreManager.Instance.IncrementOvertakeCounter();
            // Reset the initially hit rays sets and lowest hit rays for the next frame
            rightInitiallyHitRays.Clear();
            leftInitiallyHitRays.Clear();
        }
    }
}
