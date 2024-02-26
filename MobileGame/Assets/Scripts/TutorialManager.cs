using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [SerializeField] private GameObject[] popUps;
    [SerializeField] private AudioMixer audioMixer;

    private int index = 0;
    private bool anyPopupActive = false;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("ControlsTutorial") == false)
        {
            popUps[0].SetActive(true);
            Time.timeScale = 0;
            audioMixer.SetFloat("SoundParam", -80);
        }
        else
            ShowPopupBarMode();
    }

    public void HidePopup()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (popUps[i].activeSelf)
            {
                index = i;
                anyPopupActive = true;
                break;
            }
        }

        if (anyPopupActive)
        {
            popUps[index].SetActive(false);
            Time.timeScale = 1;
            anyPopupActive = false;

            if (PlayerPrefs.HasKey("Sound"))
                audioMixer.SetFloat("SoundParam", Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20);
            else
                audioMixer.SetFloat("SoundParam", 0);

            TaskCompleted(index);
        }
    }

    public void ShowPopUpCars()
    {
        if (PlayerPrefs.HasKey("CarsTutorial") == false)
        {
            popUps[1].SetActive(true);
            Time.timeScale = 0;
            audioMixer.SetFloat("SoundParam", -80);
        }
    }

    public void ShowPopUpPolice()
    {
        if (PlayerPrefs.HasKey("PoliceTutorial") == false)
        {
            popUps[2].SetActive(true);
            Time.timeScale = 0;
            audioMixer.SetFloat("SoundParam", -80);
        }
    }

    public void ShowPopupPowerups()
    {
        if(PlayerPrefs.HasKey("PowerupsTutorial") == false)
        {
            popUps[3].SetActive(true);
            Time.timeScale = 0;
            audioMixer.SetFloat("SoundParam", -80);
        }
    }

    public void ShowPopupBarMode()
    {
        if(PlayerPrefs.HasKey("BarmodeTutorial") == false)
        {
            popUps[4].SetActive(true);
            Time.timeScale = 0;
            audioMixer.SetFloat("SoundParam", -80);
        }
    }

    private void TaskCompleted(int i)
    {
        switch (i) 
        {
            case 0:
                PlayerPrefs.SetInt("ControlsTutorial", 1);
                if(SceneManager.GetActiveScene().name == "TimeGameMode" || SceneManager.GetActiveScene().name == "MonsterTruckGameMode")
                    ShowPopupBarMode();
                break;

            case 1:
                PlayerPrefs.SetInt("CarsTutorial", 1);
                break;

            case 2:
                PlayerPrefs.SetInt("PoliceTutorial", 1);
                break;

            case 3:
                PlayerPrefs.SetInt("PowerupsTutorial", 1);
                break;

            case 4:
                PlayerPrefs.SetInt("BarmodeTutorial", 1);
                break;
        }
    }
}
