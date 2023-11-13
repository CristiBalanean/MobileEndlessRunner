using System;
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

    private void Start()
    {
        select.interactable = false;

        if (PlayerPrefs.HasKey("CarDataIndex"))
        {
            carName.text = cars[PlayerPrefs.GetInt("CarDataIndex")].GetName();
            carSprite.sprite = cars[PlayerPrefs.GetInt("CarDataIndex")].GetSprite();
            CarData.Instance.currentCar = cars[PlayerPrefs.GetInt("CarDataIndex")];
            i = PlayerPrefs.GetInt("CarDataIndex");
            carName.text = cars[i].GetName();
            carSprite.sprite = cars[i].GetSprite();

            if (!cars[i].IsUnlocked())
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

            if (!cars[i].IsUnlocked())
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
        LoadFile();
    }

    public void Next()
    {
        if(i < cars.Length - 1) { i++; }
        else { i = 0; }

        carName.text = cars[i].GetName();
        if (cars[i].IsUnlocked())
            carPrice.text = "OWNED";
        else
            carPrice.text = cars[i].GetCarPrice().ToString();
        carSprite.sprite = cars[i].GetSprite();

        if (!cars[i].IsUnlocked())
        {
            select.gameObject.SetActive(false);
            unlock.gameObject.SetActive(true);
        }
        else
        {
            select.gameObject.SetActive(true);
            unlock.gameObject.SetActive(false);
        }

        if (CarData.Instance.currentCar == cars[i] || (carName.text == "Monster Truck" && cars[i].IsUnlocked()))
            select.interactable = false;
        else
            select.interactable = true;
    }

    public void Previous()
    {
        if(i > 0) { i--; }
        else { i = cars.Length - 1; }

        carName.text = cars[i].GetName();
        if (cars[i].IsUnlocked())
            carPrice.text = "OWNED";
        else
            carPrice.text = cars[i].GetCarPrice().ToString();
        carSprite.sprite = cars[i].GetSprite();

        if (!cars[i].IsUnlocked())
        {
            select.gameObject.SetActive(false);
            unlock.gameObject.SetActive(true);
        }
        else
        {
            select.gameObject.SetActive(true);
            unlock.gameObject.SetActive(false);
        }

        if (CarData.Instance.currentCar == cars[i] || (carName.text == "Monster Truck" && cars[i].IsUnlocked()))
            select.interactable = false;
        else
            select.interactable = true;
    }

    public void Select()
    {
        CarData.Instance.currentCar = cars[i];
        PlayerPrefs.SetInt("CarDataIndex", i);
        select.interactable = false;
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

    private void LoadFile()
    {
        CarUnlockedDataWrapper wrapper = JsonHandler.instance.LoadJson();

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
