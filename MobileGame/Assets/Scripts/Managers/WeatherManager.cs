using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem rainParticle;

    private bool isRaining;

    private void Start()
    {
        // Introduce a chance for rain when the game begins
        float chanceForRainAtStart = Random.Range(0, 100);
        if (chanceForRainAtStart < 300)
        {
            StartRain();
        }

        // Start checking for rain periodically
        InvokeRepeating("CheckForRain", 10f, 2.5f);
    }

    private void CheckForRain()
    {
        if (!isRaining)
        {
            float chance = Random.Range(0, 100);
            if (chance < 30)
            {
                StartRain();
            }
        }
    }

    private void StartRain()
    {
        SoundManager.instance.Play("Rain");
        rainParticle.Play();
        rainParticle.GetComponent<StopRain>().RainStarted();
        isRaining = true;
    }

    public void StopRain()
    {
        SoundManager.instance.Stop("Rain");
        rainParticle.Stop();
        isRaining = false;
    }
}
