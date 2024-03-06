    using System;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Core.Environments;
using UnityEngine.UI;

public class CloudSavingManager : MonoBehaviour
{
    public static CloudSavingManager instance;

    [SerializeField] private Button loadButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private GameObject saveSuccessfulPanel;
    [SerializeField] private GameObject loadSuccessfulPanel;

    private bool isInitialized = false;
    private string environment = "production";

    async void Awake()
    {
        if(instance == null)
            instance = this;

        try
        {
            await InitializeUnityServices();
        }
        catch (Exception exception)
        {
            // An error occurred during services initialization.
            Debug.LogError("Initialization failed: " + exception);
        }

        if (PlayerPrefs.HasKey("Save"))
        {
            saveButton.interactable = false;
        }
    }

        private async Task InitializeUnityServices()
    {
        var options = new InitializationOptions().SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        // Add your callback logic here
        OnInitializationSuccess();
    }

    private void OnInitializationSuccess()
    {
        Debug.Log("Initialization successful!");
        // Add your logic to execute after successful initialization

        // Initialization succeeded, set the flag to true
        isInitialized = true;
    }

    public async void CheckForData()
    {
        var moneyData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "moneyData" });

        if (!moneyData.ContainsKey("moneyData"))
        {
            loadButton.interactable = false;
        }
        else
        {
            Debug.Log("Cloud Not Found!");
        }
    }

    public void SaveToCloud()
    {
        loadButton.interactable = true;
        saveButton.interactable = false;
        PlayerPrefs.SetInt("Save", 1);
        GameDataHandler.instance.SaveToCloud();
        saveSuccessfulPanel.SetActive(true);
    }

    public void LoadFromCloud()
    {
        GameDataHandler.instance.LoadDataCloud();
        loadSuccessfulPanel.SetActive(true);
    }
}
