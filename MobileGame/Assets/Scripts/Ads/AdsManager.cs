using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public InitializeAds initializeAds;
    public BannerAds bannerAds;
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

    public void LoadAds()
    {
        bannerAds.LoadBanner();
        rewardedAds.LoadRewardedAds();
        interstitialAds.LoadInterstitialAds();
    }
}
