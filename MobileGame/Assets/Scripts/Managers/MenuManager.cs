using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text highscore;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject chooseControls;

    private const string FirstTimeKey = "IsFirstTime";

    private void Start()
    {
        if (PlayerPrefs.GetInt(FirstTimeKey, 1) == 1)
        {
            chooseControls.SetActive(true);
            menu.SetActive(false);
            PlayerPrefs.SetInt(FirstTimeKey, 0);
            PlayerPrefs.Save();
        }
        else
        {
            chooseControls.SetActive(false);
        }

        highscore.text = "HighScore: " + PlayerPrefs.GetInt("HighScore").ToString();

        shop.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Shop()
    {
        shop.SetActive(true);
        menu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        shop.SetActive(false);
        menu.SetActive(true);
    }

    public void Tilt()
    {
        PlayerPrefs.SetInt("Tilt", 1);
        chooseControls.SetActive(false);
        menu.SetActive(true);
    }

    public void Touch()
    {
        PlayerPrefs.SetInt("Tilt", 0);
        chooseControls.SetActive(false);
        menu.SetActive(true);
    }
}
