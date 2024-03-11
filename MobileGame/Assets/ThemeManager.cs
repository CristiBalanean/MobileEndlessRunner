using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager instance;

    [SerializeField] private AudioSource[] audioSources;

    private float fadeDuration = 2.0f; // Adjust this value to control the fade duration

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        audioSources[1].Play();
    }

    public IEnumerator SwitchThemePolice()
    {
        float startVolume = audioSources[1].volume;

        while (audioSources[1].volume > 0)
        {
            audioSources[1].volume -= startVolume * Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }
        audioSources[1].volume = 0;
        audioSources[1].Stop();

        audioSources[0].volume = 0;
        audioSources[0].Play();

        while (audioSources[0].volume < 0.8f)
        {
            audioSources[0].volume += Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }
        audioSources[0].volume = 0.8f;
    }

    public IEnumerator SwitchThemeNormal()
    {
        float startVolume = audioSources[0].volume;

        while (audioSources[0].volume > 0)
        {
            audioSources[0].volume -= startVolume * Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }
        audioSources[0].volume = 0;
        audioSources[0].Stop();

        audioSources[1].volume = 0;
        audioSources[1].Play();

        while (audioSources[1].volume < 0.8f)
        {
            audioSources[1].volume += Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }
        audioSources[1].volume = 0.8f;
    }
}
