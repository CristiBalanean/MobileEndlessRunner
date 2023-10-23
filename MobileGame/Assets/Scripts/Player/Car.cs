using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Car", menuName = "Car")]
public class Car : ScriptableObject
{
    [Header("Name")]
    [SerializeField] private string carName;

    [Header("Stats")]
    [SerializeField] private float topSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float brakingPower;
    [SerializeField] private float lowSpeedHandling;
    [SerializeField] private float highSpeedHandling;
    [SerializeField] private float length;
    [SerializeField] private float height;

    [Header("Sprite")]
    [SerializeField] private Sprite carSprite;

    [Header("Price")]
    [SerializeField] private int carPrice;

    [Header("Unlocked")]
    [SerializeField] private bool unlocked;

    [Header("Collider")]
    [SerializeField] private GameObject colliderPrefab;

    public string GetName() { return carName; }

    public float GetTopSpeed() {  return topSpeed; }

    public float GetAcceleration() {  return acceleration; }

    public float GetBrakingPower() {  return brakingPower; }

    public float GetLowSpeedHandling() {  return lowSpeedHandling; }

    public float GetHighSpeedHandling() { return highSpeedHandling; }

    public float GetLength() { return length; }

    public float GetHeight() { return height; }

    public Sprite GetSprite() { return carSprite; }

    public int GetCarPrice() { return carPrice; }

    public bool IsUnlocked() { return unlocked; }

    public void Unlock() { unlocked = true; }

    public GameObject GetColliderPrefab() { return colliderPrefab; }
}

[System.Serializable]
public class CarUnlockedData
{
    public string carName;
    public bool unlocked;

    public CarUnlockedData()
    {

    }
}

[System.Serializable]
public class CarUnlockedDataWrapper
{
    public CarUnlockedData[] carDataList;

    public CarUnlockedDataWrapper(CarUnlockedData[] carDataList)
    {
        this.carDataList = carDataList;
    }
}
