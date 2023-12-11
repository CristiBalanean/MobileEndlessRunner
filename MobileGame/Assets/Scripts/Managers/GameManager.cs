using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject tapToPlayText;
    [SerializeField] private GameObject postProcessingVolume;
    [SerializeField] private AudioMixer audioMixer;

    private bool isPlaying = false;

    private void Awake()
    {
        Application.targetFrameRate = 240;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        AudioListener.pause = false;
        GameState newGameState = GameState.Gameplay;
        GameStateManager.Instance.SetState(newGameState);
        newGameState = GameState.Paused;
        GameStateManager.Instance.SetState(newGameState);

        SoundManager.instance.Play("Ambience");
        SoundManager.instance.Play("Main Theme");
    }

    private void Start()
    {
        if(Instance == null)
            Instance = this;

        if (PlayerPrefs.HasKey("PostProcessing"))
        {
            if (PlayerPrefs.GetInt("PostProcessing") == 1)
                postProcessingVolume.SetActive(true);
            else
                postProcessingVolume.SetActive(false);     
        }

        //music and sound
        if (PlayerPrefs.HasKey("Music"))
            audioMixer.SetFloat("MusicParam", Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20);
        else
            audioMixer.SetFloat("MusicParam", 0);

        if (PlayerPrefs.HasKey("Sound"))
            audioMixer.SetFloat("SoundParam", Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20);
        else
            audioMixer.SetFloat("SoundParam", 0);
    }

    private void Update()
    {
        if((Input.GetMouseButton(0) || Input.touchCount > 0) && !isPlaying)
        {
            isPlaying = true;
            tapToPlayText.SetActive(false);

            GameState newGameState = GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);
        }
    }

    public void ChangePostProcessingSettings()
    {
        if (PlayerPrefs.GetInt("PostProcessing") == 1)
            postProcessingVolume.SetActive(true);
        else
            postProcessingVolume.SetActive(false);
    }
}
