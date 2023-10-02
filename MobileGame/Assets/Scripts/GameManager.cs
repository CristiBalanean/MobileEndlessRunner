using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject restartScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;

    [SerializeField] private TMP_Text finalScoreText;

    [SerializeField] private Slider slider;

    [SerializeField] private CarMovement player;

    [SerializeField] private Button controlsButton;

    public float deltaTime;

    private void Awake()
    {
        Application.targetFrameRate = 240;

        Time.timeScale = 1f;
    }

    private void Start()
    {
        if(Instance == null)
            Instance = this;   

        restartScreen.SetActive(false);
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(false);

        if (PlayerPrefs.GetInt("Tilt") == 1)
        {
            controlsButton.GetComponentInChildren<TMP_Text>().text = "TILT";
        }
        else
        {
            controlsButton.GetComponentInChildren<TMP_Text>().text = "TOUCH";
        }
    }

    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
    }

    public void ActivateRestartScreen()
    {
        restartScreen.SetActive(true);
        finalScoreText.text = "YOUR SCORE: " + ScoreManager.Instance.GetFinalScore().ToString();
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ReturnToGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");

        if (Time.timeScale == 0)
            Time.timeScale = 1;
    }

    public void ActivateSettingsScreen()
    {
        settingsScreen.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void BackButton()
    {
        settingsScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Controls()
    {
        if (PlayerPrefs.GetInt("Tilt") == 1)
        {
            PlayerPrefs.SetInt("Tilt", 0);
            controlsButton.GetComponentInChildren<TMP_Text>().text = "TOUCH";
        }
        else
        {
            PlayerPrefs.SetInt("Tilt", 1);
            controlsButton.GetComponentInChildren<TMP_Text>().text = "TILT";
        }

        player.ChangeControlsUI();
    }
}
