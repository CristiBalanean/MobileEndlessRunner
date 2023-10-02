using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TMP_Text carName;
    [SerializeField] private Image carSprite;

    [SerializeField] private Button select;

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
        }
        else
        {
            CarData.Instance.currentCar = cars[i];
            carName.text = cars[i].GetName();
            carSprite.sprite = cars[i].GetSprite();
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
        carSprite.sprite = cars[i].GetSprite();
    }

    public void Previous()
    {
        if(i > 0) { i--; }
        else { i = cars.Length - 1; }

        carName.text = cars[i].GetName();
        carSprite.sprite = cars[i].GetSprite();
    }

    public void Select()
    {
        CarData.Instance.currentCar = cars[i];
        PlayerPrefs.SetInt("CarDataIndex", i);
    }
}
