using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceAIFollowState : PoliceAIBaseState
{
    public override void EnterState(PoliceAIStateManager police)
    {
        Debug.Log("Follow State");
        police.rigidBody.mass = 1;
        DistanceJoint2D hookJoint = police.gameObject.GetComponent<DistanceJoint2D>();
        LineRenderer lineRenderer = police.gameObject.GetComponent<LineRenderer>();
        if (hookJoint!= null && lineRenderer != null) 
        {
            hookJoint.enabled = false;
            lineRenderer.enabled = false;
            police.hookJoint.connectedBody = null;
        }
    }

    public override void UpdateState(PoliceAIStateManager police)
    {
        FollowTarget(police);
    }

    private void FollowTarget(PoliceAIStateManager police)
    {
        Vector2 targetPosition = (Vector2)police.target.transform.position - new Vector2(0, 1.5f);
        Vector2 targetVelocity = police.targetRigidbody.velocity;

        Vector2 positionError = targetPosition - (Vector2)police.transform.position;
        Vector2 velocityError = targetVelocity - police.rigidBody.velocity;
        Vector2 desiredAcceleration = police.kp * positionError + police.kd * velocityError;

        if (police.rigidBody.velocity.y > 0)
        {
            police.rigidBody.AddForce(desiredAcceleration);
        }
        else
        {
            // If the AI is not moving upward, stop applying the downward force
            // This prevents the AI from continuously adding the force once the velocity becomes non-positive
            police.rigidBody.velocity = new Vector2(police.rigidBody.velocity.x, 0f);
        }
    }
}