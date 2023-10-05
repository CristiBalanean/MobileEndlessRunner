using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum InputTypes
{
    TILT,
    TOUCH
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public InputTypes currentInput;

    public delegate void OnInputTypeChanges(InputTypes inputTypes);
    public event OnInputTypeChanges onInputTypeChanges;

    [SerializeField] private float smoothingFactor = 0.75f;
    [SerializeField] private float movementSensitivityX = 240f;

    [Header("Tilt")]
    [SerializeField] private float deadZone = 0.2f;
    [SerializeField] private float filterStrength = 0.3f;
    Vector3 filteredAcceleration;

    [Header("Touch")]
    private float touchDirection;
    private bool left;
    private bool right;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Tilt") == 1)
        {
            currentInput = InputTypes.TILT;
        }
        else
        {
            currentInput = InputTypes.TOUCH;
        }
    }

    public float HandleInput()
    {
        switch (currentInput)
        {
            case InputTypes.TILT:
                return HandleTiltInput();

            case InputTypes.TOUCH:
                return HandleTouchInput();
        }

        return 0;
    }

    private float HandleTiltInput()
    {
        Vector3 rawAcceleration = GetAveragedAcceleration();

        filteredAcceleration = Vector3.Lerp(filteredAcceleration, rawAcceleration, filterStrength);

        float dirX = filteredAcceleration.x * smoothingFactor * movementSensitivityX * Time.deltaTime;

        if (Mathf.Abs(rawAcceleration.x) < deadZone)
        {
            rawAcceleration.x = 0f;
        }

        return dirX;
    }

    private float HandleTouchInput()
    {
        if (left)
            touchDirection = -1;
        else if(right)
            touchDirection = 1;
        else if(!left && !right)
            touchDirection = 0;

        float dirX = touchDirection * movementSensitivityX * smoothingFactor * Time.deltaTime;

        return dirX;
    }

    private Vector3 GetAveragedAcceleration()
    {
        float period = 0.0f;
        Vector3 acc = Vector3.zero;

        // Iterate through all accelerometer events in the last frame
        foreach (var evnt in Input.accelerationEvents)
        {
            acc += evnt.acceleration * evnt.deltaTime;
            period += evnt.deltaTime;
        }

        // Ensure that at least one reading was recorded
        if (period > 0)
        {
            // Compute the weighted average
            acc *= 1.0f / period;
        }

        return acc;
    }

    public void ChangeInputType(InputTypes newInputType)
    {
        currentInput = newInputType;

        onInputTypeChanges?.Invoke(newInputType);
    }

    public void ChangeDirectionLeft()
    {
        left = true;
        right = false;
    }

    public void ChangeDirectionLeftOff()
    {
        left = false;
    }

    public void ChangeDirectionRight()
    {
        right = true;
        left = false;
    }

    public void ChangeDirectionRightOff()
    {
        right = false;
    }
}
