using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SlowDown")]
public class SlowDown : PowerUp
{
    private bool isUsed = false;

    public override void ApplyPowerUp(GameObject target)
    {
        if (!isUsed)
        {
            Time.timeScale = 0.25f;
            isUsed = true;
        }
    }

    public override void FinishPowerUp(GameObject target)
    {
        if (isUsed)
        {
            Time.timeScale = 1f;
            isUsed = false;
        }
    }
}
