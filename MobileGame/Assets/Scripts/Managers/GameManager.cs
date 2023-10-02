using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
