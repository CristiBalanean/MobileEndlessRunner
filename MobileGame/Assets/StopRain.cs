using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRain : MonoBehaviour
{
    [SerializeField] private WeatherManager weatherManager;

    public void RainStarted()
    {
        float timer = Random.Range(20, 40);
        StartCoroutine(StopRainTimer(timer));
    }

    private IEnumerator StopRainTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        weatherManager.StopRain();
    }
}
