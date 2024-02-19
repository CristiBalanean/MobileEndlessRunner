using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MonsterTruckGas : MonoBehaviour
{
    [SerializeField] private UnityEvent deathTrigger;

    [SerializeField] private float startingGas;
    [SerializeField] private float gasBurnValue;
    [SerializeField] private Image gasBar;
    private float currentGas;
    private bool hasRanOutOfGas = false;

    private void Start()
    {
        currentGas = startingGas;
        gasBar.fillAmount = currentGas / startingGas;

        MoneyManager moneyManager = FindObjectOfType<MoneyManager>();
        //deathTrigger.AddListener(moneyManager.ComputeFinalMoney);
    }

    private void Update()
    {
        float speedRatio = CarMovement.Instance.GetSpeed() / CarMovement.Instance.GetTopSpeed();
        currentGas -= Time.deltaTime * speedRatio * gasBurnValue;
        gasBar.fillAmount = currentGas / startingGas;

        if (currentGas <= 0 && !hasRanOutOfGas)
        {
            TriggerDeath();
            hasRanOutOfGas = true;
        }
    }

    public void TriggerDeath()
    {
        CarMovement.Instance.hasDied = true;
        deathTrigger.Invoke();
    }

    public void ReplenishGas()
    {
        currentGas = startingGas;
        gasBar.fillAmount = currentGas / startingGas;
    }
}
