using System.Collections;
using UnityEngine;

public class AIOvertaking : MonoBehaviour
{
    [SerializeField] private bool aggressiveDriver = false;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Collider2D colliderLeft;
    [SerializeField] private Collider2D colliderRight;

    private AIBraking braking;

    private static bool isOvertaking = false; // Global overtaking state

    private float overtakingCooldown = 10f; // Cooldown period for overtaking in seconds
    private float overtakingCooldownTimer = 0f; // Timer for overtaking cooldown
    private bool canChangeLane = false;
    private bool isChangingLane = false; // New variable to track lane-changing state
    private float laneChangeCooldown = 1f;

    private void Awake()
    {
        braking = GetComponent<AIBraking>();
    }

    private void OnEnable()
    {
        int rand = Random.Range(0, 2);
        aggressiveDriver = rand == 1;
        aggressiveDriver = true;

        Invoke("CanChangeLane", 5f);
    }

    private void Update()
    {
        if (overtakingCooldownTimer > 0)
        {
            overtakingCooldownTimer -= Time.deltaTime;
            if (overtakingCooldownTimer <= 0)
            {
                // Reset the overtaking state and cooldown timer
                isOvertaking = false;
                overtakingCooldownTimer = 0f;
            }
        }

        if (aggressiveDriver && braking.hasSomethingInFront && canChangeLane && !isChangingLane && !isOvertaking)
        {
            StartCoroutine(ChangeLaneWithCooldown());
        }
    }

    private IEnumerator ChangeLaneWithCooldown()
    {
        canChangeLane = false;
        isChangingLane = true; // Set the lane-changing state to true
        isOvertaking = true; // Set the global overtaking state to true

        float elapsedTime = 0f;
        float startLanePosition = transform.position.x;
        float laneWidth = 1.65f; // Distance between lanes

        overtakingCooldownTimer = overtakingCooldown;

        // Calculate the target lane position based on the current lane position
        float targetLanePosition = (startLanePosition < 0) ? startLanePosition + laneWidth : startLanePosition - laneWidth;

        // Check if there is something in the right lane
        bool isBlockedRight = colliderRight.IsTouchingLayers(layerMask);

        // Check if there is something in the left lane
        bool isBlockedLeft = colliderLeft.IsTouchingLayers(layerMask);

        if (!isBlockedRight && targetLanePosition > 0)
        {
            targetLanePosition -= laneWidth; // Change to left lane
        }
        else if (!isBlockedLeft && targetLanePosition < 0)
        {
            targetLanePosition += laneWidth; // Change to right lane
        }

        while (elapsedTime < laneChangeCooldown)
        {
            float t = elapsedTime / laneChangeCooldown;
            float newLanePosition = Mathf.Lerp(startLanePosition, targetLanePosition, t);
            transform.position = new Vector3(newLanePosition, transform.position.y, transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetLanePosition, transform.position.y, transform.position.z);
        isChangingLane = false; // Reset the lane-changing state
        canChangeLane = true;
    }

    private void CanChangeLane()
    {
        canChangeLane = true;
    }
}
