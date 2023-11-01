using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    public abstract void ApplyPowerUp(GameObject target);

    public abstract void FinishPowerUp(GameObject target);
}
