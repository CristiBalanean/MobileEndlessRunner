using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAIHookState : PoliceAIBaseState
{
    private float desiredDistance = 2.5f;
    private float distanceLerpSpeed = 2f;
    private float stateDuration = 2f; // Duration of the Hook State in seconds
    private float elapsedTime = 0f;
    private float hookForce;

    public override void EnterState(PoliceAIStateManager police)
    {
        Debug.Log("Hook State");

        police.hookJoint = police.gameObject.AddComponent<DistanceJoint2D>();
        police.lineRenderer = police.gameObject.AddComponent<LineRenderer>();

        // Add and configure DistanceJoint2D
        police.hookJoint.enabled = true;
        police.hookJoint.connectedBody = police.targetRigidbody;
        police.hookJoint.autoConfigureDistance = false;
        police.hookJoint.enabled = true;
        police.rigidBody.mass = 2;
        hookForce = CarMovement.Instance.acceleration + 3.5f;

        // Add LineRenderer to visualize the joint
        police.lineRenderer.enabled = true;
        police.lineRenderer.positionCount = 2;
        police.lineRenderer.startWidth = 0.1f;
        police.lineRenderer.endWidth = 0.1f;
        police.lineRenderer.sortingOrder = 1;
        police.lineRenderer.material = police.hookMaterial;

        // Reset the timer
        elapsedTime = 0f;
    }

    public override void UpdateState(PoliceAIStateManager police)
    {
        // Gradually approach the desired distance using lerp
        police.hookJoint.distance = Mathf.Lerp(police.hookJoint.distance, desiredDistance, Time.deltaTime * distanceLerpSpeed);

        // Update LineRenderer positions
        police.lineRenderer.SetPosition(0, police.transform.position);
        police.lineRenderer.SetPosition(1, police.targetRigidbody.position);

        if (police.rigidBody.velocity.y > 0)
        {
            police.rigidBody.AddForce(Vector2.down * hookForce); // Add any other logic you need for the AI's behavior
        }
        else
        {
            // If the AI is not moving upward, stop applying the downward force
            // This prevents the AI from continuously adding the force once the velocity becomes non-positive
            police.rigidBody.velocity = new Vector2(police.rigidBody.velocity.x, 0f);
        }

        // Update the timer
        elapsedTime += Time.deltaTime;

        // Check if the state should transition to another state after 2 seconds
        if (elapsedTime >= stateDuration)
        {
            police.hookJoint.enabled = false;
            police.lineRenderer.enabled = false;
            police.SwitchState(police.followState); // Replace 'anotherState' with the state you want to transition to
        }
    }
}
