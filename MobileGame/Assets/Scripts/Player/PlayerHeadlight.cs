using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class PlayerHeadlight : MonoBehaviour
{
    private Light2D playerHeadlight;

    private void Awake()
    {
        playerHeadlight = GetComponent<Light2D>();
    }

    public void TurnOffHeadlight()
    {
        playerHeadlight.intensity = 0;
    }

    public void TurnOnHeadlight()
    {
        playerHeadlight.intensity = 1;
    }
}
