using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class CarHealth : MonoBehaviour
{
    public static CarHealth Instance;

    [SerializeField] private UnityEvent deathTrigger;
    [SerializeField] private int health;
    [SerializeField] private AudioMixer audioMixer;

    private bool hasDied = false;
    public bool shield = false;

    private void Awake()
    {
        Instance = this;
    }

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
        audioMixer.SetFloat("SoundParam", -80);
    }

    public void TakeDamage(int amount)
    {
        if (shield)
            return;

        health -= amount;
        if (health <= 0 && !hasDied)
        {
            TriggerDeath();
            hasDied = true;
        }
    }
}
