using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentMoneyText;
    [SerializeField] private GameObject shadowPanel;
    [SerializeField] private GameObject iapPanel;

    private void Start()
    {
        currentMoneyText.text = "<sprite=0>" + MoneyManager.Instance.currentMoney.ToString();
    }

    private void OnEnable()
    {
        currentMoneyText.text = "<sprite=0>" + MoneyManager.Instance.currentMoney.ToString();
    }

    public void ShowIAPWindow()
    {
        shadowPanel.gameObject.SetActive(true);
        iapPanel.gameObject.SetActive(true);
    }

    public void HideIAPWindow() 
    {
        shadowPanel.gameObject.SetActive(false);
        iapPanel.gameObject.SetActive(false);
    }
}
