using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentMoneyText;

    private void OnEnable()
    {
        currentMoneyText.text = "CURRENT MONEY: " + MoneyManager.Instance.currentMoney.ToString();
    }
}
