using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentMoneyText;

    private void Start()
    {
        currentMoneyText.text = MoneyManager.Instance.currentMoney.ToString();
    }

    private void OnEnable()
    {
        currentMoneyText.text = MoneyManager.Instance.currentMoney.ToString();
    }

    public void UpdateMoney()
    {
        currentMoneyText.text = MoneyManager.Instance.currentMoney.ToString();
    }
}
