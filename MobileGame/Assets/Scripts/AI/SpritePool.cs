using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpritePool : MonoBehaviour
{
    public static SpritePool Instance;

    [SerializeField] private CarGraphics[] carGraphics;

    private List<CarGraphics> carList = new List<CarGraphics>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Add all cars from carGraphics array to carList
        carList.AddRange(carGraphics);

        // Shuffle the carList
        ShuffleList(carList);
    }

    public CarGraphics ChooseSprite()
    {
        // If the list is empty, refill and shuffle it
        if (carList.Count == 0)
        {
            // Refill the list with cars from carGraphics array
            carList.AddRange(carGraphics);

            // Shuffle the carList
            ShuffleList(carList);
        }

        // Get and return the first car in the list
        CarGraphics chosenCar = carList[0];
        carList.RemoveAt(0);
        return chosenCar;
    }

    // Shuffle a list using Fisher-Yates algorithm
    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

[System.Serializable]
public class CarGraphics
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private GameObject collider;

    public Sprite GetSprite() { return sprite; }

    public GameObject GetCollider() { return collider;}
}

