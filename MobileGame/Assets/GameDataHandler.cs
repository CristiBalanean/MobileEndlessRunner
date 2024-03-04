using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.CloudSave;
using UnityEngine;

public class GameDataHandler : MonoBehaviour
{
    public static GameDataHandler instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private Car[] cars;
    [SerializeField] private PowerUp[] powerups;

    void Start()
    {
        LoadFileCars();
        LoadFilePowerups();

        Initialize();
    }

    private void Initialize()
    {
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

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            SaveOnQuit();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            SaveOnQuit();
        }
    }

    public void SaveOnQuit()
    {
        SaveFileCars();
        SaveFilePowerups();
        SaveMoney();
        PlayerPrefs.DeleteKey("Warning");
        PlayerPrefs.DeleteKey("Save");
    }

    #region local

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

    private void SaveFileCars()
    {
        List<CarUnlockedData> carDataList = new List<CarUnlockedData>();

        // Populate carDataList with unlocked state of each car
        foreach (var car in cars)
        {
            CarUnlockedData carData = new CarUnlockedData();
            carData.carName = car.GetName();
            carData.unlocked = car.IsUnlocked();
            carDataList.Add(carData);
        }

        CarUnlockedDataWrapper wrapper = new CarUnlockedDataWrapper(carDataList.ToArray());

        JsonHandler.instance.SaveJson(wrapper);
    }

    private void SaveFilePowerups()
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

    private void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", MoneyManager.Instance.currentMoney);
    }
    #endregion

    #region cloud
    public async void LoadDataCloud()
    {
        PlayerPrefs.DeleteKey("CarDataIndex");
        PlayerPrefs.DeleteKey("PowerupDataIndex");
        Initialize();

        byte[] fileCars = await CloudSaveService.Instance.Files.Player.LoadBytesAsync("carsData");
        byte[] filePowerups = await CloudSaveService.Instance.Files.Player.LoadBytesAsync("powerupsData");
        var moneyData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "moneyData" });

        string jsonCarsData = System.Text.Encoding.UTF8.GetString(fileCars);
        string jsonPowerupsData = System.Text.Encoding.UTF8.GetString(filePowerups);

        Debug.Log("Json Cars is: " + jsonCarsData);

        LoadCloudCars(jsonCarsData);
        LoadCloudPowerups(jsonPowerupsData);
        if (moneyData.TryGetValue("moneyData", out var moneyValue))
        {
            MoneyManager.Instance.currentMoney = moneyValue.Value.GetAs<int>();
        }
    }

    private void LoadCloudCars(string json)
    {
        CarUnlockedDataWrapper wrapper = JsonUtility.FromJson<CarUnlockedDataWrapper>(json);

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
                else
                {
                    car.unlocked = false;
                }
            }
        }
    }

    private void LoadCloudPowerups(string json)
    {
        PowerupUnlockedDataWrapper wrapper = JsonUtility.FromJson<PowerupUnlockedDataWrapper>(json);

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
                else
                {
                    powerup.unlocked = false;
                }
            }
        }
    }

    public void SaveToCloud()
    {
        SaveCloudCars();
        SaveCloudPowerups();
        SaveCloudMoney();
    }

    private async void SaveCloudCars()
    {
        List<CarUnlockedData> carDataList = new List<CarUnlockedData>();

        // Populate carDataList with unlocked state of each car
        foreach (var car in cars)
        {
            CarUnlockedData carData = new CarUnlockedData();
            carData.carName = car.GetName();
            carData.unlocked = car.IsUnlocked();
            carDataList.Add(carData);
        }

        CarUnlockedDataWrapper wrapper = new CarUnlockedDataWrapper(carDataList.ToArray());
        string jsonDataString = JsonUtility.ToJson(wrapper, true);
        byte[] jsonDataBytes = System.Text.Encoding.UTF8.GetBytes(jsonDataString); // Convert the JSON string to bytes
        await CloudSaveService.Instance.Files.Player.SaveAsync("carsData", jsonDataBytes);
    }

    private async void SaveCloudPowerups()
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
        string jsonDataString = JsonUtility.ToJson(wrapper, true);
        byte[] jsonDataBytes = System.Text.Encoding.UTF8.GetBytes(jsonDataString); // Convert the JSON string to bytes
        await CloudSaveService.Instance.Files.Player.SaveAsync("powerupsData", jsonDataBytes);

    }

    private async void SaveCloudMoney()
    {
        int money = MoneyManager.Instance.currentMoney;
        var data = new Dictionary<string, object> { { "moneyData", money} };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }
    #endregion
}
