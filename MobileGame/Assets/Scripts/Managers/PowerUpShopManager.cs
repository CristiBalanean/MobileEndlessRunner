using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerUpShopManager : MonoBehaviour
{
    public UnityEvent updateMoney;

    [SerializeField] private PowerUp[] powerups;
    [SerializeField] private Image powerupSprite;
    [SerializeField] private Image lockImage;
    [SerializeField] private TMP_Text powerupName;
    [SerializeField] private TMP_Text powerupDescription;
    [SerializeField] private TMP_Text powerupPrice;

    [SerializeField] private Button unlockButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button deselectButton;

    [SerializeField] private Animator unlockAnimator;

    private int index;

    private void Start()
    {
        index = 0;

        if (PlayerPrefs.HasKey("PowerupDataIndex"))
        {
            index = PlayerPrefs.GetInt("PowerupDataIndex");

            powerupSprite.sprite = powerups[PlayerPrefs.GetInt("PowerupDataIndex")].powerupImage;
            powerupSprite.sprite = powerups[PlayerPrefs.GetInt("PowerupDataIndex")].powerupImage;
            powerupName.text = powerups[PlayerPrefs.GetInt("PowerupDataIndex")].powerUpName;
            powerupDescription.text = powerups[PlayerPrefs.GetInt("PowerupDataIndex")].powerUpDescription;
            powerupPrice.text = "OWNED";

            deselectButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(false);
        }
        else
        {
            powerupSprite.sprite = powerups[index].powerupImage;
            powerupName.text = powerups[index].powerUpName;
            powerupDescription.text = powerups[index].powerUpDescription;

            if (!powerups[index].unlocked)
            {
                selectButton.gameObject.SetActive(false);
                unlockButton.gameObject.SetActive(true);
                lockImage.gameObject.SetActive(true);
                powerupPrice.text = "<sprite=0> " + powerups[index].powerupPrice.ToString();
            }
            else
            {
                selectButton.gameObject.SetActive(true);
                deselectButton.gameObject.SetActive(false);
                unlockButton.gameObject.SetActive(false);
                lockImage.gameObject.SetActive(false);
                powerupPrice.text = "OWNED";
            }
        }
    }

    public void Next()
    {
        if (index < powerups.Length - 1) { index++; }
        else { index = 0; }

        powerupSprite.sprite = powerups[index].powerupImage;
        powerupName.text = powerups[index].powerUpName;
        powerupDescription.text = powerups[index].powerUpDescription;
        powerupPrice.text = "<sprite=0> " + powerups[index].powerupPrice.ToString();

        if (!powerups[index].unlocked)
        {
            selectButton.gameObject.SetActive(false);
            deselectButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(true);
            lockImage.gameObject.SetActive(true);
        }
        else
        {
            if (PlayerPrefs.HasKey("PowerupDataIndex") && PlayerPrefs.GetInt("PowerupDataIndex") == index)
            {
                selectButton.gameObject.SetActive(false);
                deselectButton.gameObject.SetActive(true);
            }
            else
            {
                selectButton.gameObject.SetActive(true);
                deselectButton.gameObject.SetActive(false);
            }
            unlockButton.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(false);
            powerupPrice.text = "OWNED";
        }
    }

    public void Previous()
    {
        if (index > 0) { index--; }
        else { index = powerups.Length - 1; }

        powerupSprite.sprite = powerups[index].powerupImage;
        powerupName.text = powerups[index].powerUpName;
        powerupDescription.text = powerups[index].powerUpDescription;
        powerupPrice.text = "<sprite=0> " + powerups[index].powerupPrice.ToString();

        if (!powerups[index].unlocked)
        {
            selectButton.gameObject.SetActive(false);
            deselectButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(true);
            lockImage.gameObject.SetActive(true);
        }
        else
        {
            if(PlayerPrefs.HasKey("PowerupDataIndex") && PlayerPrefs.GetInt("PowerupDataIndex") == index)
            {
                selectButton.gameObject.SetActive(false);
                deselectButton.gameObject.SetActive(true);
            }
            else
            {
                selectButton.gameObject.SetActive(true);
                deselectButton.gameObject.SetActive(false);
            }
            unlockButton.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(false);
            powerupPrice.text = "OWNED";
        }
    }

    public void Select()
    {
        PowerUpData.Instance.currentPowerUp = powerups[index].powerup;
        PowerUpData.Instance.currentPowerUpImage = powerups[index].powerupImage;
        PlayerPrefs.SetInt("PowerupDataIndex", index);
        selectButton.gameObject.SetActive(false);
        deselectButton.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        PowerUpData.Instance.currentPowerUp = null;
        PowerUpData.Instance.currentPowerUpImage = null;
        PlayerPrefs.DeleteKey("PowerupDataIndex");
        selectButton.gameObject.SetActive(true);
        deselectButton.gameObject.SetActive(false);
    }

    public void Unlock()
    {
        if (MoneyManager.Instance.currentMoney >= powerups[index].powerupPrice)
        {
            PlayGamesPlatform.Instance.UnlockAchievement("CgkIvZqi8NgeEAIQDw", (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked successfully!");
                    // Do any additional actions you want upon achievement unlock
                    PlayerPrefs.SetInt("FirstCar", 1);
                }
                else
                {
                    Debug.LogWarning("Failed to unlock achievement.");
                    // Handle the case where unlocking the achievement failed
                }
            });

            powerups[index].unlocked = true;
            selectButton.gameObject.SetActive(true);
            unlockButton.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(false);
            MoneyManager.Instance.currentMoney -= powerups[index].powerupPrice;
        }
        else
        {
            Debug.LogError("Not Enough Money!");
            unlockAnimator.SetTrigger("NotEnoughMoney");
        }

        updateMoney.Invoke();
    }
}
