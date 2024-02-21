using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    public InitializeAds initializeAds;
    public AdmobAds bannerAds;
    public RewardedAds rewardedAds;
    public InterstitialAds interstitialAds;

    public static AdsManager instance { get; private set; }

    public bool isInitialized = false;
    public bool adsRemoved = false;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadAds()
    {
        if (!adsRemoved)
        {
            bannerAds.LoadBanner();
            interstitialAds.LoadInterstitialAds();
        }
        rewardedAds.LoadRewardedAds();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MonsterTruckGameMode" || scene.name == "SampleScene" || scene.name == "TimeGameMode" || scene.name == "TwoWaysGameMode")
            bannerAds.DestroyBanner();
        else
        {
            if (!adsRemoved)
            {
                bannerAds.DestroyBanner();
                bannerAds.LoadBanner();
            }
        }
    }
}
