using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerUpBar : MonoBehaviour
{
    public UnityEvent powerupActivation;
    public UnityEvent powerupDeactivation;

    [SerializeField] private Image powerUpBarUI;
    [SerializeField] private Image powerUpImage;
    [SerializeField] private Image powerUpSprite;
    public float powerUpCountdown;

    public float currentTime;
    private bool isPressed = false;
    private bool isPaused = false;
    private PowerupManager powerupManager;

    private void Awake()
    {
        powerupManager = GameObject.Find("Player").GetComponent<PowerupManager>();

        powerupManager.EmptyBar += EmptyBar;
        powerupManager.FillBar += FillBar;
    }

    private void OnDestroy()
    {
        powerupManager.EmptyBar -= EmptyBar;
        powerupManager.FillBar -= FillBar;
    }

    private void Start()
    {
        if (PowerUpData.Instance.currentPowerUp == null)
        {
            powerUpBarUI.gameObject.SetActive(false);
            powerUpImage.gameObject.SetActive(false);
            powerUpSprite.gameObject.SetActive(false);
        }

        currentTime = powerUpCountdown;
        powerUpBarUI.fillAmount = currentTime / powerUpCountdown;
        powerUpImage.sprite = PowerUpData.Instance.currentPowerUpImage;
    }

    private void Update()
    {
        powerUpBarUI.fillAmount = currentTime / powerUpCountdown;
    }

    private IEnumerator EmptyBar()
    {
        while (currentTime > 0)
        {
            if (isPaused)
                yield break;

            yield return new WaitForSeconds(.1f);
            currentTime -= .1f;
        }
        currentTime = 0;
        StartCoroutine(FillBar());
        powerupDeactivation.Invoke();
    }

    private IEnumerator FillBar()
    {
        while (currentTime < powerUpCountdown)
        {
            if (isPaused)
                yield break;

            yield return new WaitForSeconds(.1f);
            currentTime += .001f;
        }
        currentTime = powerUpCountdown;
    }

    public void ButtonPressed()
    {
        StopAllCoroutines();

        isPressed = !isPressed;

        if (isPressed)
        {
            powerupActivation.Invoke();
        }
        else
        {
            powerupDeactivation.Invoke();
        }
    }
}
