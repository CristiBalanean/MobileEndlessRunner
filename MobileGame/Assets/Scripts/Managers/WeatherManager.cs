using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WeatherManager : MonoBehaviour
{
    public UnityEvent StartParticles;
    public UnityEvent StopParticles;

    [SerializeField] private ParticleSystem[] particles;

    public bool isRaining;

    private void Start()
    {
        // Introduce a chance for rain when the game begins
        float chanceForRainAtStart = Random.Range(0, 100);
        if (chanceForRainAtStart < 20)
        {
            StartRain();
        }
    }

    private void StartRain()
    {
        if(GameModeData.instance.map != 1)
            StartParticles?.Invoke();

        switch(GameModeData.instance.map)
        {
            case 0:
                SoundManager.instance.Play("Rain");
                break;

            case 1:
                break;

            case 2:
                SoundManager.instance.Play("Snow");
                break;
        }

        particles[GameModeData.instance.map].Play();
        isRaining = true;
        CancelInvoke("CheckForRain");
    }
}
