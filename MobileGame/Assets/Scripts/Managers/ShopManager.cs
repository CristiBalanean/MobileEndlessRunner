using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using TMPro;

[Serializable]
public class ConsumableItems
{
    public string name;
    public string id;
    public string description;
    public float price;
}


public class ShopManager : MonoBehaviour, IStoreListener
{
    IStoreController storeController;

    [SerializeField] private TMP_Text currentMoneyText;
    [SerializeField] private Button carsButton;
    [SerializeField] private Button powerUpsButton;

    [SerializeField] private string menuScene;

    [SerializeField] private GameObject carsUI;
    [SerializeField] private GameObject powerupsUI;

    [SerializeField] private Animator transition;

    [SerializeField] private ConsumableItems[] consumableItems;


    private void Start()
    {
        carsButton.interactable = false;
        SetupBuilder();
    }

    private void SetupBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (ConsumableItems cons in consumableItems)
        {
            builder.AddProduct(cons.id, ProductType.Consumable);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("Success");
        storeController = controller;
    }

    public void BackButton()
    {
        StartCoroutine(LoadLevel(menuScene));
    }

    public void ChangeToCars()
    {
        carsButton.interactable = false;
        powerUpsButton.interactable = true;
        carsUI.SetActive(true);
        powerupsUI.SetActive(false);
    }

    public void ChangeToPowerups()
    {
        carsButton.interactable = true;
        powerUpsButton.interactable = false;
        carsUI.SetActive(false);
        powerupsUI.SetActive(true);
    }

    IEnumerator LoadLevel(string scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }

    public void AddMoney10000()
    {
        storeController.InitiatePurchase(consumableItems[0].id);
    }

    public void AddMoney100000()
    {
        storeController.InitiatePurchase(consumableItems[1].id);
    }

    public void AddMoney1000000()
    {
        storeController.InitiatePurchase(consumableItems[2].id);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        print("Purchase Complete " + product.definition.id);

        if(product.definition.id == consumableItems[0].id)
        {
            MoneyManager.Instance.currentMoney += 10000;
            UpdateMoney();
        }
        else if (product.definition.id == consumableItems[1].id)
        {
            MoneyManager.Instance.currentMoney += 100000;
            UpdateMoney();
        }
        else if (product.definition.id == consumableItems[2].id)
        {
            MoneyManager.Instance.currentMoney += 1000000;
            UpdateMoney();
        }

        return PurchaseProcessingResult.Complete;
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

    public void UpdateMoney()
    {
        currentMoneyText.text = "<sprite=0>" + MoneyManager.Instance.currentMoney.ToString();
    }
}
