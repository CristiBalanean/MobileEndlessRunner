using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Application.targetFrameRate = 240;

        Time.timeScale = 1f;
    }

    private void Start()
    {
        if(Instance == null)
            Instance = this;   
    }
}
