using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private string menuScene;

    [SerializeField] private TMP_Dropdown inputTypeDropdown;

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

    private void InputTypeChanged()
    {
        if (inputTypeDropdown.value == 0)
        {
            Debug.Log("TILT");
            PlayerPrefs.SetInt("Tilt", 1);
        }
        else
        {
            Debug.Log("TOUCH");
            PlayerPrefs.SetInt("Tilt", 0);
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene(menuScene);
    }
}
