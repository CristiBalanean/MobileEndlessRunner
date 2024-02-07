using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int currentMoney;

    public int moneyToGive;

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
        moneyToGive = ScoreManager.Instance.GetFinalScore() / 15 + (int)ScoreManager.Instance.GetDistanceTraveled() / 10;
        currentMoney += moneyToGive;
        PlayerPrefs.SetInt("Money", currentMoney);
    }
}
