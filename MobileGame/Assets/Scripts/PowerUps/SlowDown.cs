using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SlowDown")]
public class SlowDown : PowerUp
{
    public override void ActivatePowerUp(GameObject target)
    {
        Time.timeScale = 0.35f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        SoundManager.instance.Play("Slowdown");
    }

    public override void DeactivatePowerUp(GameObject target)
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SoundManager.instance.Stop("Slowdown");
    }
}
