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
        bannerAds.LoadBanner();
        rewardedAds.LoadRewardedAds();
        interstitialAds.LoadInterstitialAds();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MonsterTruckGameMode" || scene.name == "SampleScene" || scene.name == "TimeGameMode" || scene.name == "TwoWaysGameMode")
            bannerAds.DestroyBanner();
        else
        {
            bannerAds.DestroyBanner();
            bannerAds.LoadBanner();
        }
    }
}
