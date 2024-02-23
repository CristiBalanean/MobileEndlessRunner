using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class CarHealth : MonoBehaviour
{
    public static CarHealth Instance;

    [SerializeField] private UnityEvent deathTrigger;
    [SerializeField] private int health;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider healthBar;

    public bool hasDied = false;
    public bool shield = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MoneyManager moneyManager = FindObjectOfType<MoneyManager>();
        //deathTrigger.AddListener(moneyManager.ComputeFinalMoney);
    }

    public void TriggerDeath()
    {
        //ParticleManager.instance.InstantiateParticle(transform);
        CarMovement.Instance.hasDied = true;
        if (GamesPlayedManager.instance.gamesPlayed % 5 == 0 && !AdsManager.instance.adsRemoved)
            AdsManager.instance.interstitialAds.ShowInterstitialAds();
        GamesPlayedManager.instance.gamesPlayed++;
        deathTrigger.Invoke();
        audioMixer.SetFloat("SoundParam", -80);
    }

    public void TakeDamage(int amount)
    {
        if (shield)
            return;

        health -= amount;
        healthBar.value = (float)health / 15;
        Debug.Log(health / 15);
        if (health <= 0 && !hasDied)
        {
            TriggerDeath();
            hasDied = true;
        }
    }

    public void ContinueGame()
    {
        CarMovement.Instance.hasDied = false;
        hasDied = false;
        health = 15;

        if (PlayerPrefs.HasKey("Sound"))
            audioMixer.SetFloat("SoundParam", Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20);
        else
            audioMixer.SetFloat("SoundParam", 0);
    }
}
