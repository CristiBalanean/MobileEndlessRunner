using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Button carsButton;
    [SerializeField] private Button powerUpsButton;

    [SerializeField] private string menuScene;

    [SerializeField] private GameObject carsUI;
    [SerializeField] private GameObject powerupsUI;

    [SerializeField] private Animator transition;

    private void Start()
    {
        carsButton.interactable = false;
    }

    public void BackButton()
    {
        StartCoroutine(LoadLevel(menuScene));
    }

    public void ChangeToCars()
    {
        carsButton.interactable = false;
        powerUpsButton.interactable = true;
        carsUI.SetActive(true);
        powerupsUI.SetActive(false);
    }

    public void ChangeToPowerups()
    {
        carsButton.interactable = true;
        powerUpsButton.interactable = false;
        carsUI.SetActive(false);
        powerupsUI.SetActive(true);
    }

    IEnumerator LoadLevel(string scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }

    public void AddMoney()
    {
        MoneyManager.Instance.currentMoney += 100000;
        PlayerPrefs.SetInt("Money", MoneyManager.Instance.currentMoney);
        carsUI.GetComponent<CarsShopManager>().updateMoney?.Invoke();
    }
}
