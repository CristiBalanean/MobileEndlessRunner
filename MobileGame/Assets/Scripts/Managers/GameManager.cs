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
    [SerializeField] private TutorialManager tutorialManager;

    private bool isPlaying = false;

    private void Awake()
    {
        Application.targetFrameRate = 240;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        AudioListener.pause = false;

        Time.timeScale = 0;

        SoundManager.instance.Play("Ambience");
        SoundManager.instance.Play("Main Theme");

        AdsManager.instance.bannerAds.HideBannerAd();
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

        audioMixer.SetFloat("SoundParam", -80);
    }

    private void Update()
    {
        if((Input.GetMouseButton(0) || Input.touchCount > 0) && !isPlaying)
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        isPlaying = true;
        tapToPlayText.SetActive(false);

        if (PlayerPrefs.HasKey("Sound"))
            audioMixer.SetFloat("SoundParam", Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20);
        else
            audioMixer.SetFloat("SoundParam", 0);

        Time.timeScale = 1;

        yield return new WaitForSeconds(2f);

        tutorialManager.gameObject.SetActive(true);
        if(PowerUpData.Instance.currentPowerUp != null)
            tutorialManager.ShowPopupPowerups();
    }

    public void ChangePostProcessingSettings()
    {
        if (PlayerPrefs.GetInt("PostProcessing") == 1)
            postProcessingVolume.SetActive(true);
        else
            postProcessingVolume.SetActive(false);
    }
}
