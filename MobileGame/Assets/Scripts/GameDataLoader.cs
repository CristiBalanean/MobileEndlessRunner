using System;
using UnityEngine;

public class GameDataLoader : MonoBehaviour
{
    [SerializeField] private Car[] cars;
    [SerializeField] private Powerups[] powerups;

    void Start()
    {
        LoadFileCars();
        LoadFilePowerups();

        if (PlayerPrefs.HasKey("CarDataIndex"))
            CarData.Instance.currentCar = cars[PlayerPrefs.GetInt("CarDataIndex")];
        else
            CarData.Instance.currentCar = cars[0];

        if (PlayerPrefs.HasKey("PowerupDataIndex"))
        {
            PowerUpData.Instance.currentPowerUp = powerups[PlayerPrefs.GetInt("PowerupDataIndex")].powerup;
            PowerUpData.Instance.currentPowerUpImage = powerups[PlayerPrefs.GetInt("PowerupDataIndex")].powerupImage;
        }
    }

    private void LoadFileCars()
    {
        CarUnlockedDataWrapper wrapper = JsonHandler.instance.LoadJson();

        if (wrapper != null)
        {
            foreach (var carData in wrapper.carDataList)
            {
                // Find the car by name in the cars array
                var car = Array.Find(cars, c => c.GetName() == carData.carName);

                if (car != null && carData.unlocked)
                {
                    car.Unlock();
                }
            }
        }
    }

    private void LoadFilePowerups()
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
