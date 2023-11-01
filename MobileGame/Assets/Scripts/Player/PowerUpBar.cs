using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpBar : MonoBehaviour
{
    [SerializeField] private PowerUp currentPowerUp;

    [SerializeField] private Image powerUpBarUI;
    [SerializeField] private Image powerUpImage;
    [SerializeField] private float powerUpCountdown;

    private float currentTime;
    private bool isPressed = false;

    private void Start()
    {
        currentTime = powerUpCountdown;
        powerUpBarUI.fillAmount = currentTime / powerUpCountdown;
        powerUpImage.sprite = PowerUpData.Instance.currentPowerUpImage;
        currentPowerUp = PowerUpData.Instance.currentPowerUp;
    }

    private void Update()
    {
        powerUpBarUI.fillAmount = currentTime / powerUpCountdown;

        if (isPressed && currentTime > 0)
        {
            currentTime -= Time.unscaledDeltaTime;
            currentPowerUp.ApplyPowerUp(GameObject.Find("Player"));
        }
        else if(currentTime < 0 || currentTime < powerUpCountdown)
        {
            isPressed = false;
            currentTime += Time.unscaledDeltaTime;
            currentPowerUp.FinishPowerUp(GameObject.Find("Player"));
        }
        else if (currentTime > powerUpCountdown)
            currentTime = powerUpCountdown;
    }

    public void ButtonPressed()
    {
        isPressed = !isPressed;
    }
}
