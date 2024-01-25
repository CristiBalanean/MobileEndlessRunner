using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAIFollowState : PoliceAIBaseState
{
    public override void EnterState(PoliceAIStateManager police)
    {
        Debug.Log("Follow State");
    }

    public override void UpdateState(PoliceAIStateManager police)
    {
        if (PoliceEvent.instance.hasStarted)
        {
            FollowTarget(police);
        }
        else
        {
            police.rigidBody.velocity = Vector2.zero;
        }
    }

    private void FollowTarget(PoliceAIStateManager police)
    {
        Vector2 targetPosition = (Vector2)police.target.transform.position - new Vector2(0, 1.5f);
        Vector2 targetVelocity = police.targetRigidbody.velocity;

        Vector2 positionError = targetPosition - (Vector2)police.transform.position;
        Vector2 velocityError = targetVelocity - police.rigidBody.velocity;
        Vector2 desiredAcceleration = police.kp * positionError + police.kd * velocityError;

        police.rigidBody.AddForce(desiredAcceleration);
    }
}