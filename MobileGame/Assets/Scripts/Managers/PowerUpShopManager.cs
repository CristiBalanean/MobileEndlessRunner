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

    [SerializeField] private Powerups[] powerups;
    [SerializeField] private Image powerupSprite;
    [SerializeField] private Image lockImage;
    [SerializeField] private TMP_Text powerupName;
    [SerializeField] private TMP_Text powerupDescription;
    [SerializeField] private TMP_Text powerupPrice;

    [SerializeField] private Button unlockButton;
    [SerializeField] private Button selectButton;

    [SerializeField] private Animator unlockAnimator;

    private int index;

    private void Start()
    {
        index = 0;

        LoadFile();

        if (PlayerPrefs.HasKey("PowerupDataIndex"))
        {
            selectButton.interactable = false;

            index = PlayerPrefs.GetInt("PowerupDataIndex");

            powerupSprite.sprite = powerups[PlayerPrefs.GetInt("PowerupDataIndex")].powerupImage;
            powerupSprite.sprite = powerups[PlayerPrefs.GetInt("PowerupDataIndex")].powerupImage;
            powerupName.text = powerups[PlayerPrefs.GetInt("PowerupDataIndex")].powerUpName;
            powerupDescription.text = powerups[PlayerPrefs.GetInt("PowerupDataIndex")].powerUpDescription;
            powerupPrice.text = "OWNED";

            selectButton.gameObject.SetActive(true);
            unlockButton.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(false);
        }
        else
        {
            powerupSprite.sprite = powerups[index].powerupImage;
            powerupName.text = powerups[index].powerUpName;
            powerupDescription.text = powerups[index].powerUpDescription;
            powerupPrice.text = "<sprite=0> " + powerups[index].powerupPrice.ToString();

            if (!powerups[index].unlocked)
            {
                selectButton.gameObject.SetActive(false);
                unlockButton.gameObject.SetActive(true);
                lockImage.gameObject.SetActive(true);
            }
            else
            {
                selectButton.gameObject.SetActive(true);
                unlockButton.gameObject.SetActive(false);
                lockImage.gameObject.SetActive(false);
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
            unlockButton.gameObject.SetActive(true);
            lockImage.gameObject.SetActive(true);
        }
        else
        {
            selectButton.gameObject.SetActive(true);
            unlockButton.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(false);
            powerupPrice.text = "OWNED";
        }

        if (PowerUpData.Instance.currentPowerUp == powerups[index].powerup)
            selectButton.interactable = false;
        else
            selectButton.interactable = true;
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
            unlockButton.gameObject.SetActive(true);
            lockImage.gameObject.SetActive(true);
        }
        else
        {
            selectButton.gameObject.SetActive(true);
            unlockButton.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(false);
            powerupPrice.text = "OWNED";
        }

        if (PowerUpData.Instance.currentPowerUp == powerups[index].powerup)
            selectButton.interactable = false;
        else
            selectButton.interactable = true;
    }

    public void Select()
    {
        PowerUpData.Instance.currentPowerUp = powerups[index].powerup;
        PowerUpData.Instance.currentPowerUpImage = powerups[index].powerupImage;
        PlayerPrefs.SetInt("PowerupDataIndex", index);
        selectButton.interactable = false;
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
            PlayerPrefs.SetInt("Money", MoneyManager.Instance.currentMoney);

            SaveFile();
            LoadFile();
        }
        else
        {
            Debug.LogError("Not Enough Money!");
            unlockAnimator.SetTrigger("NotEnoughMoney");
        }

        updateMoney.Invoke();
    }

    private void SaveFile()
    {
        List<PowerupUnlockedData> powerupDataList = new List<PowerupUnlockedData>();

        // Populate carDataList with unlocked state of each car
        foreach (var powerup in powerups)
        {
            PowerupUnlockedData powerupData = new PowerupUnlockedData();
            powerupData.powerupName = powerup.powerUpName;
            powerupData.unlocked = powerup.unlocked;
            powerupDataList.Add(powerupData);
        }

        PowerupUnlockedDataWrapper wrapper = new PowerupUnlockedDataWrapper(powerupDataList.ToArray());

        JsonHandlerPowerups.instance.SaveJson(wrapper);
    }

    private void LoadFile()
    {
        PowerupUnlockedDataWrapper wrapper = JsonHandlerPowerups.instance.LoadJson();

        if (wrapper != null)
        {
            foreach (var powerupData in wrapper.powerupDataList)
            {
                // Find the car by name in the cars array
                var powerup = Array.Find(powerups, c => c.powerUpName == powerupData.powerupName);

                if (powerup != null && powerupData.unlocked)
                {
                    powerup.unlocked = true;
                }
            }
        }
    }
}
