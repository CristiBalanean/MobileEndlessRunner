using System.Collections;
using System.Collections.Generic;
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

    public void Previous()
    {
        if(i > 0) { i--; }
        else { i = cars.Length - 1; }

        carName.text = cars[i].GetName();
        carPrice.text = cars[i].GetCarPrice().ToString();
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
        }
        else
            Debug.LogError("Not Enough Money!");
    }
}
