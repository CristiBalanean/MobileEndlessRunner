using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int currentMoney;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        currentMoney = PlayerPrefs.GetInt("Money");
    }

    public void ComputeFinalMoney()
    {
        int moneyToGive = ScoreManager.Instance.GetFinalScore() / 25 + (int)ScoreManager.Instance.GetDistanceTraveled() / 20;
        currentMoney += moneyToGive;
        PlayerPrefs.SetInt("Money", currentMoney);
        Debug.Log("Money given: " + moneyToGive);
    }
}
