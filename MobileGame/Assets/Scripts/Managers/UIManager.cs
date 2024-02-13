using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject restartScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text fpsCounterText;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        CarMovement.OnSpeedChange += UpdateSpeedText;
    }

    private void OnDisable()
    {
        CarMovement.OnSpeedChange -= UpdateSpeedText;
    }

    void Start()
    {
        restartScreen.SetActive(false);
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(false);

        InvokeRepeating("UpdateScoreText", 0f, 0.25f);
    }

    private void Update()
    {
        int currentFPS = (int)(1f / Time.deltaTime);
        fpsCounterText.text = "FPS: " + currentFPS.ToString();
    }

    public void ShowRestartScreen()
    {
        HideAllScreens();
        restartScreen.SetActive(true);
    }

    public void ShowPauseScreen()
    {
        HideAllScreens();
        pauseScreen.SetActive(true);
    }

    public void ShowSettingsScreen()
    {
        HideAllScreens();
        settingsScreen.SetActive(true);
    }

    private void HideAllScreens()
    {
        restartScreen.SetActive(false);
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(false);
    }

    private void UpdateScoreText()
    {
        scoreText.text = "SCORE: " + ScoreManager.Instance.score.ToString();
    }

    private void UpdateSpeedText(float speed)
    {
        speedText.text = "SPEED: " + speed.ToString("F1") + " kph";
    }
}
