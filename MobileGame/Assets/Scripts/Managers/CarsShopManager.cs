using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CarsShopManager : MonoBehaviour
{
    public UnityEvent updateMoney;

    [SerializeField] private TMP_Text carName;
    [SerializeField] private TMP_Text carPrice;
    [SerializeField] private Image carSprite;
    [SerializeField] private Image lockedSprite;

    [SerializeField] private Button select;
    [SerializeField] private Button unlock;

    [SerializeField] private Car[] cars;

    [SerializeField] private int maxSpeed;
    [SerializeField] private int maxAcceleration;
    [SerializeField] private int maxBrake;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider accelerationSlider;
    [SerializeField] private Slider brakeSlider;

    [SerializeField] private Material colorSwapMaterial;
    [SerializeField] private Color[] colors;
    [SerializeField] private Image colorsButtonHighlight;
    [SerializeField] private Button[] colorsButtons;

    private int i = 0;

    private void Start()
    {
        select.interactable = false;

        if (PlayerPrefs.HasKey("CarDataIndex"))
        {
            carName.text = cars[PlayerPrefs.GetInt("CarDataIndex")].GetName();
            carSprite.sprite = cars[PlayerPrefs.GetInt("CarDataIndex")].GetSprite();
            i = PlayerPrefs.GetInt("CarDataIndex");
            carName.text = cars[i].GetName();
            carSprite.sprite = cars[i].GetSprite();

            select.gameObject.SetActive(true);
            unlock.gameObject.SetActive(false);
            lockedSprite.gameObject.SetActive(false);
            carPrice.text = "OWNED";
            SetStats(cars[i]);
        }
        else
        {
            CarData.Instance.currentCar = cars[i];
            carName.text = cars[i].GetName();
            carSprite.sprite = cars[i].GetSprite();

            select.gameObject.SetActive(true);
            unlock.gameObject.SetActive(false);
            lockedSprite.gameObject.SetActive(false);
            carPrice.text = "OWNED";
            SetStats(cars[i]);
        }

        if (PlayerPrefs.HasKey("Color"))
        {
            colorSwapMaterial.SetColor("_BaseColor", colors[PlayerPrefs.GetInt("Color") - 1]);
            colorsButtonHighlight.transform.position = colorsButtons[PlayerPrefs.GetInt("Color") - 1].transform.position;
        }
        else
            colorSwapMaterial.SetColor("_BaseColor", colors[0]);
    }

    public void Next()
    {
        if (i < cars.Length - 1) { i++; }
        else { i = 0; }

        carName.text = cars[i].GetName();
        if (cars[i].IsUnlocked())
            carPrice.text = "OWNED";
        else
            carPrice.text = "<sprite=0> " + cars[i].GetCarPrice().ToString();
        carSprite.sprite = cars[i].GetSprite();

        if (!cars[i].IsUnlocked())
        {
            select.gameObject.SetActive(false);
            unlock.gameObject.SetActive(true);
            lockedSprite.gameObject.SetActive(true);
        }
        else
        {
            select.gameObject.SetActive(true);
            unlock.gameObject.SetActive(false);
            lockedSprite.gameObject.SetActive(false);
        }

        if (CarData.Instance.currentCar == cars[i] || (carName.text == "Monster Truck" && cars[i].IsUnlocked()))
            select.interactable = false;
        else
            select.interactable = true;

        SetStats(cars[i]);
    }

    public void Previous()
    {
        if (i > 0) { i--; }
        else { i = cars.Length - 1; }

        carName.text = cars[i].GetName();
        if (cars[i].IsUnlocked())
            carPrice.text = "OWNED";
        else
            carPrice.text = "<sprite=0> " + cars[i].GetCarPrice().ToString();
        carSprite.sprite = cars[i].GetSprite();

        if (!cars[i].IsUnlocked())
        {
            select.gameObject.SetActive(false);
            unlock.gameObject.SetActive(true);
            lockedSprite.gameObject.SetActive(true);
        }
        else
        {
            select.gameObject.SetActive(true);
            unlock.gameObject.SetActive(false);
            lockedSprite.gameObject.SetActive(false);
        }

        if (CarData.Instance.currentCar == cars[i] || (carName.text == "Monster Truck" && cars[i].IsUnlocked()))
            select.interactable = false;
        else
            select.interactable = true;

        SetStats(cars[i]);
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
            lockedSprite.gameObject.SetActive(false);
            carPrice.text = "OWNED";
            cars[i].Unlock();
            MoneyManager.Instance.currentMoney -= cars[i].GetCarPrice();
            PlayerPrefs.SetInt("Money", MoneyManager.Instance.currentMoney);

            if((carName.text == "Monster Truck" && cars[i].IsUnlocked()))
                select.interactable = false;

            SaveFile();
            LoadFile();
        }
        else
            Debug.LogError("Not Enough Money!");

        updateMoney.Invoke();
    }

    public void ChangeColor(int colorIndex)
    {
        PlayerPrefs.SetInt("Color", colorIndex);

        carSprite.material = colorSwapMaterial;
        colorSwapMaterial.SetColor("_BaseColor", colors[colorIndex - 1]);

        colorsButtonHighlight.transform.position = colorsButtons[colorIndex - 1].transform.position;
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

    private void SetStats(Car car)
    {
        speedSlider.value = car.GetTopSpeed() / maxSpeed;
        accelerationSlider.value = car.GetAcceleration() / maxAcceleration;
        brakeSlider.value = car.GetBrakingPower() / maxBrake;
    }
}
