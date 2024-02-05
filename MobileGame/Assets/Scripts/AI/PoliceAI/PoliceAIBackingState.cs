using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAIBackingState : PoliceAIBaseState
{
    public override void EnterState(PoliceAIStateManager police)
    {
        Debug.Log("Follow State");

        police.rigidBody.mass = 1;
    }

    public override void UpdateState(PoliceAIStateManager police)
    {
        police.rigidBody.AddForce(Vector2.down * 10);
    }
}
