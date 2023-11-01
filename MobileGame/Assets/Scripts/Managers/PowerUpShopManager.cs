using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpShopManager : MonoBehaviour
{
    [SerializeField] private PowerUps[] powerUps;
    [SerializeField] private Image powerUpSprite;
    [SerializeField] private TMP_Text powerUpName;

    private int index;

    private void OnEnable()
    {
        index = 0;

        powerUpSprite.sprite = powerUps[index].powerUpImage;
        powerUpName.text = powerUps[index].powerUpName;
        PowerUpData.Instance.currentPowerUp = powerUps[index].powerUp;
        PowerUpData.Instance.currentPowerUpImage = powerUps[index].powerUpImage;
    }

    public void Next()
    {
        if (index < powerUps.Length - 1) { index++; }
        else { index = 0; }

        powerUpSprite.sprite = powerUps[index].powerUpImage;
        powerUpName.text = powerUps[index].powerUpName;
        PowerUpData.Instance.currentPowerUp = powerUps[index].powerUp;
        PowerUpData.Instance.currentPowerUpImage = powerUps[index].powerUpImage;
    }

    public void Previous()
    {
        if (index > 0) { index--; }
        else { index = powerUps.Length - 1; }

        powerUpSprite.sprite = powerUps[index].powerUpImage;
        powerUpName.text = powerUps[index].powerUpName;
        PowerUpData.Instance.currentPowerUp = powerUps[index].powerUp;
        PowerUpData.Instance.currentPowerUpImage = powerUps[index].powerUpImage;
    }
}

[System.Serializable]
public class PowerUps
{
    public string powerUpName;
    public PowerUp powerUp;
    public Sprite powerUpImage;
}
