using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuThemeManager : MonoBehaviour
{
    public static MenuThemeManager Instance;

    private AudioSource audioSource;
    public float fadeDuration = 1.0f; // Adjust this value to control the fade duration


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the one where you want to fade the theme
        if (scene.name == "MonsterTruckGameMode" || scene.name == "SampleScene" || scene.name == "TimeGameMode" || scene.name == "TwoWaysGameMode")
        {
            StartCoroutine(FadeOutTheme());
        }
        else if(scene.name == "Menu" && !audioSource.isPlaying)
        {
            audioSource.Play();
            audioSource.volume = 0.8f;
        }
    }

    private IEnumerator FadeOutTheme()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }

        // Ensure the volume is set to 0 to avoid any small leftover volume
        audioSource.volume = 0;

        // Stop the audio playback
        audioSource.Stop();
    }
}
