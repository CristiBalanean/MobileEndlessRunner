using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAIHookState : PoliceAIBaseState
{
    private DistanceJoint2D hookJoint;
    private LineRenderer lineRenderer;
    private float desiredDistance = 2.5f;
    private float distanceLerpSpeed = 2f;
    private float stateDuration = 2f; // Duration of the Hook State in seconds
    private float elapsedTime = 0f;
    private float hookForce;

    public override void EnterState(PoliceAIStateManager police)
    {
        Debug.Log("Hook State");

        // Add and configure DistanceJoint2D
        hookJoint = police.gameObject.AddComponent<DistanceJoint2D>();
        hookJoint.connectedBody = police.targetRigidbody;
        hookJoint.autoConfigureDistance = false;
        hookJoint.enabled = true;
        police.rigidBody.mass = 2;
        hookForce = CarMovement.Instance.acceleration + 3.5f;

        // Add LineRenderer to visualize the joint
        lineRenderer = police.gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.sortingOrder = 1;
        lineRenderer.material = police.hookMaterial;

        // Reset the timer
        elapsedTime = 0f;
    }

    public override void UpdateState(PoliceAIStateManager police)
    {
        // Gradually approach the desired distance using lerp
        hookJoint.distance = Mathf.Lerp(hookJoint.distance, desiredDistance, Time.deltaTime * distanceLerpSpeed);

        // Update LineRenderer positions
        lineRenderer.SetPosition(0, police.transform.position);
        lineRenderer.SetPosition(1, police.targetRigidbody.position);

        // Add any other logic you need for the AI's behavior
        police.rigidBody.AddForce(Vector2.down * hookForce);

        // Update the timer
        elapsedTime += Time.deltaTime;

        // Check if the state should transition to another state after 2 seconds
        if (elapsedTime >= stateDuration)
        {
            hookJoint.enabled = false;
            lineRenderer.enabled = false;
            police.SwitchState(police.followState); // Replace 'anotherState' with the state you want to transition to
        }
    }
}
