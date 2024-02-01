using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WeatherManager : MonoBehaviour
{
    public UnityEvent StartParticles;
    public UnityEvent StopParticles;

    [SerializeField] private ParticleSystem rainParticle;

    public bool isRaining;

    private void Start()
    {
        // Introduce a chance for rain when the game begins
        float chanceForRainAtStart = Random.Range(0, 100);
        if (chanceForRainAtStart < 20)
        {
            StartRain();
        }

        // Start checking for rain periodically
        InvokeRepeating("CheckForRain", 10f, 10f);
    }

    private void CheckForRain()
    {
        if (!isRaining)
        {
            float chance = Random.Range(0, 100);
            if (chance < 10)
            {
                StartRain();
            }
        }
    }

    private void StartRain()
    {
        StartParticles?.Invoke();
        SoundManager.instance.Play("Rain");
        rainParticle.Play();
        rainParticle.GetComponent<StopRain>().RainStarted();
        isRaining = true;
        CancelInvoke("CheckForRain");
    }

    public void StopRain()
    {
        StopParticles?.Invoke();
        SoundManager.instance.Stop("Rain");
        rainParticle.Stop();
        isRaining = false;
        InvokeRepeating("CheckForRain", 10f, 10f);
    }
}
