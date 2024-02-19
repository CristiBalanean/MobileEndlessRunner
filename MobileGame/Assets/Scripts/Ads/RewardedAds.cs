using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;

    private string adUnitId;

    private void Awake()
    {
        adUnitId = androidAdUnitId;
    }

    public void LoadRewardedAds()
    {
        Advertisement.Load(adUnitId, this);
    }

    public void ShowRewardedAds()
    {
        Advertisement.Show(adUnitId, this);
        LoadRewardedAds();
    }

    #region LoadCallbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Rewarded Ad Loaded Successful");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Rewarded Ad Load Failed");
    }
    #endregion

    #region ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { Debug.Log("Rewarded Ad Failed"); }

    public void OnUnityAdsShowStart(string placementId) { Debug.Log("Rewarded Ad Started"); }

    public void OnUnityAdsShowClick(string placementId) { Debug.Log("Rewarded Ad Clicked"); }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log(showCompletionState.ToString());
        Debug.Log("Ad Show Rewarded Complete");
        FindObjectOfType<RestartScreen>().isRewarded = true;
        FindObjectOfType<RestartScreen>().HandleRewardedAdComplete(FindObjectOfType<RestartScreen>().rewardType);
    }
    #endregion
}
