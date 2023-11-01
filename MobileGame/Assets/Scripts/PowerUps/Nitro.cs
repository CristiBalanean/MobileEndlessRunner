using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Nitro")]
public class Nitro : PowerUp
{
    private float nitroAmount = 30;
    private bool isUsed = false;

    public override void ApplyPowerUp(GameObject target)
    {
        if (!isUsed)
        {
            CarMovement.Instance.acceleration += nitroAmount;
            isUsed = true;
        }
    }

    public override void FinishPowerUp(GameObject target)
    {
        if (isUsed)
        {
            CarMovement.Instance.acceleration -= nitroAmount;
            isUsed = false;
        }
    }
}
