using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Shield")]
public class Shield : PowerUp
{
    public override void ActivatePowerUp(GameObject target)
    {
        SoundManager.instance.Play("ForceField");
        CarHealth.Instance.shield = true;
    }

    public override void DeactivatePowerUp(GameObject target)
    {
        SoundManager.instance.Stop("ForceField");
        CarHealth.Instance.shield = false;
    }
}
