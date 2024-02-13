using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeMode : MonoBehaviour
{
    public UnityEvent deathTrigger;

    [SerializeField] private Image timer;
    [SerializeField] private float startingTime;
    private float currentTime;

    private bool timeRanOut;

    private void Start()
    {
        currentTime = startingTime;
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            if(!timeRanOut)
            {
                timeRanOut = true;
                TriggerDeath();
            }
        }

        timer.fillAmount = currentTime / startingTime;
    }

    public void TriggerDeath()
    {
        CarMovement.Instance.hasDied = true;
        deathTrigger.Invoke();
    }

    public void ReplenishTime()
    {
        currentTime += 20;
        if(currentTime > startingTime)
        {
            currentTime = startingTime;
        }
        timer.fillAmount = currentTime / startingTime;
    }

}
