using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;

    private string adUnitId;

    private void Awake()
    {
        adUnitId = androidAdUnitId;
    }

    public void LoadInterstitialAds()
    {
        Advertisement.Load(adUnitId, this);
    }

    public void ShowInterstitialAds()
    {
        Advertisement.Show(adUnitId, this);
        LoadInterstitialAds();
    }

    #region LoadCallbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial Ad Loaded Successful");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Interstitial Ad Load Failed");
    }
    #endregion

    #region ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message){}

    public void OnUnityAdsShowStart(string placementId){}

    public void OnUnityAdsShowClick(string placementId){}

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Ad Show Complete");
    }
    #endregion
}
