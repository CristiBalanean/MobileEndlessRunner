using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] private UnityEvent inputTypeTrigger;
    [SerializeField] private UnityEvent postProcessingTrigger;

    [SerializeField] private TMP_Dropdown inputTypeDropdown;
    [SerializeField] private Toggle postProcessingToggle;
    [SerializeField] private TMP_Text toggleText;

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void Start()
    {
        inputTypeDropdown.onValueChanged.AddListener(delegate { InputTypeChanged(); });

        if (PlayerPrefs.GetInt("Tilt") == 1)
        {
            inputTypeDropdown.value = 0;
        }
        else
        {
            inputTypeDropdown.value = 1;
        }

        if(PlayerPrefs.HasKey("PostProcessing"))
        {
            if (PlayerPrefs.GetInt("PostProcessing") == 1)
            {
                postProcessingToggle.isOn = true;
                toggleText.text = "ON";
            }
            else
            {
                postProcessingToggle.isOn = false;
                toggleText.text = "OFF";
            }
        }
        else
        {
            PlayerPrefs.SetInt("PostProcessing", 1);
            postProcessingToggle.isOn = true;
            toggleText.text = "ON";
        }
    }

    public void BackButton()
    {
        UIManager.Instance.ShowPauseScreen();
    }

    private void InputTypeChanged()
    {
        if (inputTypeDropdown.value == 0)
        {
            Debug.Log("TILT");
            PlayerPrefs.SetInt("Tilt", 1);
            InputManager.Instance.ChangeInputType(InputTypes.TILT);
        }
        else
        {
            Debug.Log("TOUCH");
            PlayerPrefs.SetInt("Tilt", 0);
            InputManager.Instance.ChangeInputType(InputTypes.TOUCH);
        }

        inputTypeTrigger.Invoke();
    }

    public void OnValueChangedToggle()
    {
        if (postProcessingToggle.isOn)
        {
            PlayerPrefs.SetInt("PostProcessing", 1);
            toggleText.text = "ON";
        }
        else
        {
            PlayerPrefs.SetInt("PostProcessing", 0);
            toggleText.text = "OFF";
        }

        postProcessingTrigger.Invoke();
    }
}
