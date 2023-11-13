using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarHealth : MonoBehaviour
{
    [SerializeField] private UnityEvent deathTrigger;
    [SerializeField] private int health;

    private bool hasDied = false;

    private void Start()
    {
        MoneyManager moneyManager = FindObjectOfType<MoneyManager>();
        deathTrigger.AddListener(moneyManager.ComputeFinalMoney);
    }

    public void TriggerDeath()
    {
        //ParticleManager.instance.InstantiateParticle(transform);
        CarMovement.Instance.hasDied = true;
        deathTrigger.Invoke();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0 && !hasDied)
        {
            TriggerDeath();
            hasDied = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            deathTrigger.Invoke();
        }
    }
}
