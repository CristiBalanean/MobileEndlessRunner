using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] private Button inputTypeButton;

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Tilt") == 1)
        {
            inputTypeButton.GetComponentInChildren<TMP_Text>().text = "TILT";
        }
        else
        {
            inputTypeButton.GetComponentInChildren<TMP_Text>().text = "TOUCH";
        }
    }

    public void BackButton()
    {
        UIManager.Instance.ShowPauseScreen();
    }

    public void InputTypeButton()
    {
        
        if (PlayerPrefs.GetInt("Tilt") == 1)
        {
            PlayerPrefs.SetInt("Tilt", 0);
            inputTypeButton.GetComponentInChildren<TMP_Text>().text = "TOUCH";
            InputManager.Instance.ChangeInputType(InputTypes.TOUCH);
        }
        else
        {
            PlayerPrefs.SetInt("Tilt", 1);
            inputTypeButton.GetComponentInChildren<TMP_Text>().text = "TILT";
            InputManager.Instance.ChangeInputType(InputTypes.TILT);
        }
    }
}
