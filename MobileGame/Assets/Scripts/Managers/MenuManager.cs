using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class NonConsumableItems
{
    public string name;
    public string id;
    public string description;
    public float price;
}

public class MenuManager : MonoBehaviour, IStoreListener
{
    IStoreController storeController;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject chooseControls;
    [SerializeField] private string shopScene;
    [SerializeField] private string playScene;
    [SerializeField] private string settingsScene;
    [SerializeField] private Animator transition;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private NonConsumableItems nonConsumable;
    [SerializeField] private Button removeAdsButton;

    private const string FirstTimeKey = "IsFirstTime";

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(FirstTimeKey, 1) == 1)
        {
            chooseControls.SetActive(true);
            menu.SetActive(false);
            PlayerPrefs.SetInt(FirstTimeKey, 0);
            PlayerPrefs.Save();
        }
        else
        {
            chooseControls.SetActive(false);
        }

        //music and sound
        if (PlayerPrefs.HasKey("Music"))
            audioMixer.SetFloat("MusicParam", Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20);
        else
            audioMixer.SetFloat("MusicParam", 0);

        if (PlayerPrefs.HasKey("Sound"))
            audioMixer.SetFloat("SoundParam", Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20);
        else
            audioMixer.SetFloat("SoundParam", 0);

        SetupBuilder();
        CheckNonConsumables(nonConsumable.id);
    }

    private void SetupBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(nonConsumable.id, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("Success");
        storeController = controller;
    }

    public void Play()
    {
        StartCoroutine(LoadLevel(playScene));
    }

    public void Shop()
    {
        StartCoroutine(LoadLevel(shopScene));
    }

    public void Settings()
    {
        StartCoroutine(LoadLevel(settingsScene));
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void Tilt()
    {
        PlayerPrefs.SetInt("Tilt", 1);
        chooseControls.SetActive(false);
        menu.SetActive(true);
    }

    public void Touch()
    {
        PlayerPrefs.SetInt("Tilt", 0);
        chooseControls.SetActive(false);
        menu.SetActive(true);
    }

    IEnumerator LoadLevel(string scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }

    public void RemoveAds()
    {
        storeController.InitiatePurchase(nonConsumable.id);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        print("Purchase Complete " + product.definition.id);

        AdsManager.instance.adsRemoved = true;
        AdsManager.instance.bannerAds.DestroyBanner();
        removeAdsButton.interactable = false;

        return PurchaseProcessingResult.Complete;
    }

    private void CheckNonConsumables(string id)
    {
        if(storeController != null)
        {
            var product =storeController.products.WithID(id);
            if (product != null)
            {
                if (product.hasReceipt)
                {
                    AdsManager.instance.adsRemoved = true;
                    AdsManager.instance.bannerAds.DestroyBanner();
                    removeAdsButton.interactable = false;
                }
            }
        }
    }

    #region callbacks
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print("Initialization failed " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print("Initialization failed " + error + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print("Purchase failed");
    }
    #endregion
}
