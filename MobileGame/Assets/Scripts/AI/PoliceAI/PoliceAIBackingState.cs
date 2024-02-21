using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAIBackingState : PoliceAIBaseState
{
    public override void EnterState(PoliceAIStateManager police)
    {
        Debug.Log("Backing State");
        police.rigidBody.mass = 1;
        DistanceJoint2D hookJoint = police.gameObject.GetComponent<DistanceJoint2D>();
        LineRenderer lineRenderer = police.gameObject.AddComponent<LineRenderer>();
        if (hookJoint != null && lineRenderer != null)
        {
            hookJoint.enabled = false;
            lineRenderer.enabled = false;
        }
    }

    public override void UpdateState(PoliceAIStateManager police)
    {
        police.rigidBody.AddForce(Vector2.down * 10);
    }
}
