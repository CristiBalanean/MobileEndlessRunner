using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoliceAIBaseState
{
    public abstract void EnterState(PoliceAIStateManager police);

    public abstract void UpdateState(PoliceAIStateManager police);
}
