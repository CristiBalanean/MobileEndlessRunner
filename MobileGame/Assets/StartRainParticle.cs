using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRainParticle : MonoBehaviour
{
    private ParticleSystem particle;
    private WeatherManager weatherManager;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        weatherManager = GameObject.Find("WeatherManager").GetComponent<WeatherManager>();

        if (transform.name == "Player")
        {
            weatherManager.StartParticles.AddListener(StartParticle);
            weatherManager.StopParticles.AddListener(StopParticle);
        }
    }

    private void OnEnable()
    {
        if (weatherManager.isRaining)
            StartParticle();

        weatherManager.StartParticles.AddListener(StartParticle);
    }

    private void OnDisable()
    {
        weatherManager.StartParticles.RemoveListener(StartParticle);
    }

    public void StartParticle()
    {
        particle.Play();
    }

    public void StopParticle()
    {
        particle.Stop();
    }
}
