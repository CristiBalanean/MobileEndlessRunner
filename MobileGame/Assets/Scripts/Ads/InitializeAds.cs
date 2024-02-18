using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string iosGameId;
    [SerializeField] private string androidGameId;
    [SerializeField] private bool isTesting;

    private string gameId;

    private void Awake()
    {
        #if UNITY_ANDROID
                gameId = androidGameId;
        #elif UNITY_EDITOR
                gameId = androidGameId;
        #endif

        if(!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTesting, this);
        }
    }

    public void OnInitializationComplete()
    {
        AdsManager.instance.isInitialized = true;
        AdsManager.instance.LoadAds();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Ads Initialization Failed");
    }
}
