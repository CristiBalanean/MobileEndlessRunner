using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class AdmobAds : MonoBehaviour
{
    [SerializeField] private string appId = "ca-app-pub-3940256099942544~3347511713";

#if UNITY_ANDROID
    string bannerId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
    string bannerId = "ca-app-pub-3940256099942544/2934735716";
#endif

    BannerView bannerView;

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => { Debug.Log("Ads Initialized!"); });
    }

    public void LoadBanner()
    {
        CreateBannerView();
        ListenToBannerEvents();
        if(bannerView == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading Banner Ad");
        bannerView.LoadAd(adRequest);
    }

    private void CreateBannerView()
    {
        if (bannerView != null)
        {
            DestroyBanner();
        }
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);
    }

    private void ListenToBannerEvents()
    {
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    public void DestroyBanner()
    {
        if(bannerView != null)
        {
            print("Destroying The Banner");
            bannerView.Destroy();
            bannerView = null;
        }
    }
}
