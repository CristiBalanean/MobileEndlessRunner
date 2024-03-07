using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAICrashingState : PoliceAIBaseState
{
    public override void EnterState(PoliceAIStateManager police)
    {
        Debug.Log("Crashing State");

        police.rigidBody.drag = 0.5f;
        police.rigidBody.angularDrag = 0.5f;
        police.rigidBody.freezeRotation = false;
        police.aiCollider.isTrigger = true;
        police.state.enabled = false;
        police.avoidance.enabled = false;
        GameObject.Destroy(police.hookJoint);
        GameObject.Destroy(police.lineRenderer);
    }

    public override void UpdateState(PoliceAIStateManager police)
    {
        // Add a random angular velocity to simulate spinning out of control
        float randomAngularVelocity = Random.Range(-10f, 10f);
        police.rigidBody.angularVelocity = randomAngularVelocity;

        police.rigidBody.AddForce(Vector2.down * 150);
    }
}
