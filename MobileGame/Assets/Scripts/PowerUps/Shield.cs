using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Shield")]
public class Shield : PowerUp
{
    private bool isUsed = false;

    public override void ApplyPowerUp(GameObject target)
    {
        if (!isUsed)
        {
            Collider2D[] colliders = target.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D collider in colliders)
            { 
                collider.enabled = false;
            }
            isUsed = true;
        }
    }

    public override void FinishPowerUp(GameObject target)
    {
        if (isUsed)
        {
            Collider2D[] colliders = target.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = true;
            }
            isUsed = false;
        }
    }
}
