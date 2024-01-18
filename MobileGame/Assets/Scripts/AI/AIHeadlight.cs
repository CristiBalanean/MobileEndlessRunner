using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class AIHeadlight : MonoBehaviour
{
    private Light2D headlight;

    private void Awake()
    {
        headlight = GetComponent<Light2D>();
    }

    private void Start()
    {
        if (!TimeManager.instance.isDay)
            TurnOnHeadlight();
        else
            TurnOffHeadlight();
    }

    private void OnEnable()
    {
        TimeManager.instance.notifyIsDay.AddListener(TurnOffHeadlight);
        TimeManager.instance.notifyIsNight.AddListener(TurnOnHeadlight);

        if (!TimeManager.instance.isDay)
            TurnOnHeadlight();
        else
            TurnOffHeadlight();
    }

    private void OnDisable()
    {
        TimeManager.instance.notifyIsDay.RemoveListener(TurnOffHeadlight);
        TimeManager.instance.notifyIsNight.RemoveListener(TurnOnHeadlight);
    }

    public void TurnOffHeadlight()
    {
        StartCoroutine(DelayHeadlightsOff());
    }

    public void TurnOnHeadlight()
    {
        StartCoroutine(DelayHeadlightsOn());
        
    }

    private IEnumerator DelayHeadlightsOff()
    {
        float randomSecondsDelay = Random.Range(0, 0.35f);

        yield return new WaitForSeconds(randomSecondsDelay);
        headlight.enabled = false;
    }

    private IEnumerator DelayHeadlightsOn()
    {
        float randomSecondsDelay = Random.Range(0f, 0.35f);

        yield return new WaitForSeconds(randomSecondsDelay);
        headlight.enabled = true;
    }
}
