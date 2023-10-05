using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUI : MonoBehaviour
{
    [SerializeField] private GameObject tiltUI;
    [SerializeField] private GameObject touchUI;

    private void Start()
    {
        InputManager.Instance.onInputTypeChanges += HandleInputUIChanged;

        tiltUI.SetActive(false);
        touchUI.SetActive(false);

        if (InputManager.Instance.currentInput == InputTypes.TILT)
            tiltUI.SetActive(true);
        else
            touchUI.SetActive(true);
    }

    private void HandleInputUIChanged(InputTypes newInputType)
    {
        switch (newInputType)
        {
            case InputTypes.TILT:
                touchUI.SetActive(false);
                tiltUI.SetActive(true);
                break;

            case InputTypes.TOUCH:
                tiltUI.SetActive(false);
                touchUI.SetActive(true);
                break;
        }
    }
}
