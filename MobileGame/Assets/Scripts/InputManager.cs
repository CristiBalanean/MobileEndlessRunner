using UnityEngine;

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

    private float xAxisDirection;
    private float yAxisDirection;
    private bool left;
    private bool right;
    private bool accelerating;
    private bool braking;

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

    public Vector2 HandleInput()
    {
        switch (currentInput)
        {
            case InputTypes.TILT:
                return HandleTiltInput();

            case InputTypes.TOUCH:
                return HandleTouchInput();
        }

        return Vector2.zero;
    }

    private Vector2 HandleTiltInput()
    {
        xAxisDirection = Input.acceleration.x * 6f;
        if (xAxisDirection > -0.1f && xAxisDirection < 0.1f)
            xAxisDirection = 0;
        xAxisDirection = Mathf.Clamp(xAxisDirection, -1, 1);

        // Determine the y-axis input based on accelerating and braking states
        float yAxisDirection = 0;
        if (accelerating && !braking)
            yAxisDirection = 1;
        else if (braking)
            yAxisDirection = -1;

        return new Vector2(xAxisDirection, yAxisDirection);
    }

    private Vector2 HandleTouchInput()
    {
        if (left)
            xAxisDirection = -1;
        else if(right)
            xAxisDirection = 1;
        else if(!left && !right)
            xAxisDirection = 0;

        if (braking)
            yAxisDirection = -1;
        else
            yAxisDirection = 1;

        return new Vector2(xAxisDirection, yAxisDirection);
    }


    public void ChangeInputType(InputTypes newInputType)
    {
        currentInput = newInputType;

        onInputTypeChanges?.Invoke(newInputType);
    }

    public void ChangeAcceleratingOn()
    {
        accelerating = true;
    }

    public void ChangeAcceleratingOff()
    {
        accelerating = false;
    }

    public void ChangeBrakingOn()
    {
        braking = true;
    }

    public void ChangeBrakingOff()
    {
        braking = false;
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
