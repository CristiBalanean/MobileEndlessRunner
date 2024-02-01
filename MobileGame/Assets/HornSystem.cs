using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornSystem : MonoBehaviour
{
    private PlayerHeadlight headlight;

    private void Awake()
    {
        headlight = GetComponentInChildren<PlayerHeadlight>();
    }

    public void HornButtonPressed()
    {
        SoundManager.instance.Play("Horn");
        if(TimeManager.instance.isDay)
            headlight.TurnOnHeadlight();
    }

    public void HornButtonReleased()
    {
        SoundManager.instance.Stop("Horn");
        if(TimeManager.instance.isDay)
            headlight.TurnOffHeadlight();
    }
}
