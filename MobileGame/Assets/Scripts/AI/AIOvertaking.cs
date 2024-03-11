using System.Collections;
using UnityEngine;

public class AIOvertaking : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Collider2D colliderLeft;
    [SerializeField] private Collider2D colliderRight;
    [SerializeField] private Animator animator;

    private AIBraking braking;

    private static bool isOvertaking = false; // Global overtaking state
    private static float globalCooldownDuration = 10f; // Global cooldown period after overtaking in seconds
    private static float globalCooldownTimer; // Timer for the global cooldown period
    private bool canChangeLane = true; // Allow lane change at the start

    private void Awake()
    {
        braking = GetComponent<AIBraking>();
        animator = FindChildAnimatorRecursive(transform); // Start the recursive search from the root
    }

    private Animator FindChildAnimatorRecursive(Transform parent)
    {
        Animator foundAnimator = null;

        foreach (Transform child in parent)
        {
            // Check if the child has an Animator component
            Animator childAnimator = child.GetComponent<Animator>();
            if (childAnimator != null)
            {
                foundAnimator = childAnimator;
                break; // Found the Animator, no need to continue searching
            }

            // Recursively search in the child's children
            Animator recursiveResult = FindChildAnimatorRecursive(child);
            if (recursiveResult != null)
            {
                foundAnimator = recursiveResult;
                break; // Found the Animator, no need to continue searching
            }
        }

        return foundAnimator;
    }

    private void OnEnable()
    {
        globalCooldownTimer = globalCooldownDuration; // Initialize the global cooldown timer when object is enabled
    }

    private void Update()
    {
        if (globalCooldownTimer > 0)
        {
            globalCooldownTimer -= Time.deltaTime;
        }

        if (braking.hasSomethingInFront && canChangeLane && !isOvertaking && globalCooldownTimer <= 0)
        {
            ChangeLane();
        }
    }

    private void ChangeLane()
    {
        float startLanePosition = transform.position.x;
        float laneWidth = 1.35f; // Distance between lanes

        float targetLanePosition = (startLanePosition < 0) ? startLanePosition + laneWidth : startLanePosition - laneWidth;

        bool isBlockedRight = colliderRight.IsTouchingLayers(layerMask);
        bool isBlockedLeft = colliderLeft.IsTouchingLayers(layerMask);

        if ((!isBlockedRight && targetLanePosition > 0) || (!isBlockedLeft && targetLanePosition < 0))
        {
            if(!isBlockedRight && targetLanePosition > 0 && !isOvertaking)
            {
                animator.SetTrigger("SignalRight");
            }
            if(!isBlockedLeft && targetLanePosition < 0 && !isOvertaking)
            {
                animator.SetTrigger("SignalLeft");
            }    
            StartCoroutine(MoveToLane(targetLanePosition));
        }
    }

    private IEnumerator MoveToLane(float targetLanePosition)
    {
        isOvertaking = true;

        yield return new WaitForSeconds(1.5f);

        canChangeLane = false;
        globalCooldownTimer = globalCooldownDuration; // Reset the global cooldown timer

        float startLanePosition = transform.position.x;
        float elapsedTime = 0f;
        float laneChangeDuration = 1f; // Set the duration for lane change

        while (elapsedTime < laneChangeDuration)
        {
            float t = elapsedTime / laneChangeDuration;
            float newLanePosition = Mathf.Lerp(startLanePosition, targetLanePosition, t);
            transform.position = new Vector3(newLanePosition, transform.position.y, transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetLanePosition, transform.position.y, transform.position.z);
        canChangeLane = true;
        isOvertaking = false;
    }
}
