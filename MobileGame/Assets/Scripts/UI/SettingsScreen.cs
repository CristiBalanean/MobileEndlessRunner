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

    [SerializeField] private TMP_Dropdown inputTypeDropdown;

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
}
