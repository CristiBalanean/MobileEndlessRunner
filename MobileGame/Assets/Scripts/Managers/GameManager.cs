using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject tapToPlayText;

    private bool isPlaying = false;

    private void Awake()
    {
        Application.targetFrameRate = 240;
    }

    private void Start()
    {
        if(Instance == null)
            Instance = this;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if(Input.GetMouseButton(0) || Input.touchCount > 0 && !isPlaying)
        {
            tapToPlayText.SetActive(false);
            Time.timeScale = 1f;
            isPlaying = true;
        }
    }
}
