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
        playerHeadlight.intensity = 1;
        gameObject.SetActive(false);
    }

    public void TurnOffHeadlight()
    {
        gameObject.SetActive(false);
    }

    public void TurnOnHeadlight()
    {
        gameObject.SetActive(true);
    }
}
