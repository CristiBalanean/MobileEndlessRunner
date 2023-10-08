using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TMP_Text carName;
    [SerializeField] private TMP_Text carPrice;
    [SerializeField] private Image carSprite;

    [SerializeField] private Button select;
    [SerializeField] private Button unlock;

    [SerializeField] private Car[] cars;

    private int i = 0;

    private void Awake()
    {
        LoadFile();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("CarDataIndex"))
        {
            carName.text = cars[PlayerPrefs.GetInt("CarDataIndex")].GetName();
            carSprite.sprite = cars[PlayerPrefs.GetInt("CarDataIndex")].GetSprite();
            CarData.Instance.currentCar = cars[PlayerPrefs.GetInt("CarDataIndex")];
            i = PlayerPrefs.GetInt("CarDataIndex");
            carName.text = cars[i].GetName();
            carSprite.sprite = cars[i].GetSprite();

            if (!cars[i].IsLocked())
            {
                select.gameObject.SetActive(false);
                unlock.gameObject.SetActive(true);
            }
            else
            {
                select.gameObject.SetActive(true);
                unlock.gameObject.SetActive(false);
            }
        }
        else
        {
            CarData.Instance.currentCar = cars[i];
            carName.text = cars[i].GetName();
            carSprite.sprite = cars[i].GetSprite();

            if (!cars[i].IsLocked())
            {
                select.gameObject.SetActive(false);
                unlock.gameObject.SetActive(true);
            }
            else
            {
                select.gameObject.SetActive(true);
                unlock.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (!cars[i].IsLocked())
        {
            select.gameObject.SetActive(false);
            unlock.gameObject.SetActive(true);
        }
        else
        {
            select.gameObject.SetActive(true);
            unlock.gameObject.SetActive(false);
        }

        if (CarData.Instance.currentCar == cars[i])
            select.interactable = false;
        else
            select.interactable = true;
    }

    public void Next()
    {
        if(i < cars.Length - 1) { i++; }
        else { i = 0; }

        carName.text = cars[i].GetName();
        carPrice.text = cars[i].GetCarPrice().ToString();
        carSprite.sprite = cars[i].GetSprite();
    }

    public void Previous()
    {
        if(i > 0) { i--; }
        else { i = cars.Length - 1; }

        carName.text = cars[i].GetName();
        carPrice.text = cars[i].GetCarPrice().ToString();
        carSprite.sprite = cars[i].GetSprite();
    }

    public void Select()
    {
        CarData.Instance.currentCar = cars[i];
        PlayerPrefs.SetInt("CarDataIndex", i);
    }

    public void Unlock()
    {
        if (MoneyManager.Instance.currentMoney >= cars[i].GetCarPrice())
        {
            select.gameObject.SetActive(true);
            unlock.gameObject.SetActive(false);
            cars[i].Unlock();
            MoneyManager.Instance.currentMoney -= cars[i].GetCarPrice();
            PlayerPrefs.SetInt("Money", MoneyManager.Instance.currentMoney);
            

            SaveFile();
            LoadFile();
        }
        else
            Debug.LogError("Not Enough Money!");
    }

    private void SaveFile()
    {
        CarUnlockedData[] carDataList = new CarUnlockedData[cars.Length];

        // Populate carDataList with unlocked state of each car
        for (int i = 0; i < cars.Length; i++)
        {
            carDataList[i] = new CarUnlockedData();
            carDataList[i].carName = cars[i].GetName();
            carDataList[i].unlocked = cars[i].IsLocked();
        }
        CarUnlockedDataWrapper wrapper = new CarUnlockedDataWrapper(carDataList);

        // Serialize carDataList to JSON
        string json = JsonUtility.ToJson(wrapper);
        Debug.Log(json);

        // Save the JSON data to PlayerPrefs or a file on disk
        PlayerPrefs.SetString("CarUnlockedData", json);
        Debug.Log("Car data saved to PlayerPrefs.");
    }

    private void LoadFile()
    {
        // Retrieve the JSON data from PlayerPrefs
        string json = PlayerPrefs.GetString("CarUnlockedData");

        // Deserialize the JSON data into a CarUnlockedDataWrapper instance
        CarUnlockedDataWrapper wrapper = JsonUtility.FromJson<CarUnlockedDataWrapper>(json);

        // Check if the wrapper is not null and contains valid data
        if (wrapper != null && wrapper.carDataList != null && wrapper.carDataList.Length > 0)
        {
            // Iterate through the deserialized data and update your Car objects
            for (int i = 0; i < wrapper.carDataList.Length; i++)
            {
                string carName = wrapper.carDataList[i].carName;
                bool isUnlocked = wrapper.carDataList[i].unlocked;

                // Find the corresponding Car object in your cars array
                Car car = System.Array.Find(cars, c => c.GetName() == carName);

                // Update the unlocked state of the Car object
                if (car != null)
                {
                    if(isUnlocked)
                        car.Unlock(); // Unlock the car if necessary
                }
            }
            Debug.Log("Car data loaded successfully.");
        }
        else
        {
            SaveFile();
        }
    }
}
