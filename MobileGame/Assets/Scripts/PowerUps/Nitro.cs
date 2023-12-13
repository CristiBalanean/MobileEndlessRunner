using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "PowerUps/Nitro")]
public class Nitro : PowerUp
{
    private float nitroAmount = 35;

    public override void ActivatePowerUp(GameObject target)
    {
        GameObject volumeGO = GameObject.Find("Volume");
        if (volumeGO != null)
        {
            Volume volume = volumeGO.GetComponent<Volume>();
            MotionBlur motionBlur;
            if (volume.profile.TryGet(out motionBlur))
            {
                motionBlur.intensity.value = 0.275f;
                motionBlur.clamp.value = 0.2f;
            }
        }
        CarMovement.Instance.acceleration += nitroAmount;
        SoundManager.instance.Play("Nitro");
    }

    public override void DeactivatePowerUp(GameObject target)
    {
        GameObject volumeGO = GameObject.Find("Volume");
        if (volumeGO != null)
        {
            Volume volume = GameObject.Find("Volume").GetComponent<Volume>();
            MotionBlur motionBlur;
            if (volume.profile.TryGet(out motionBlur))
            {
                motionBlur.intensity.value = 0.035f;
                motionBlur.clamp.value = 0.025f;
            }
        }
        CarMovement.Instance.acceleration = CarMovement.Instance.player.GetAcceleration();
        SoundManager.instance.Stop("Nitro");
    }
}
