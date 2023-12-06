using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonHandlerPowerups : MonoBehaviour
{
    public static JsonHandlerPowerups instance;

    private string path;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Combine the path to the saved files directory
        path = Path.Combine(Application.persistentDataPath, "saved files");

        // Check if the directory exists
        if (!Directory.Exists(path))
        {
            // If the directory doesn't exist, create it
            Directory.CreateDirectory(path);
            Debug.Log("Directory created: " + path);
        }
        else
        {
            Debug.Log("Directory already exists: " + path);
        }

        // Combine the path with the file name
        path = Path.Combine(path, "dataPowerups.json");

        // Check if the JSON file exists, create an empty one if not
        if (!File.Exists(path))
        {
            CreateEmptyJsonFile();
            Debug.Log("Empty JSON file created at: " + path);
        }

        Debug.Log("JsonHandler initialized with path: " + path);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void CreateEmptyJsonFile()
    {
        // Create an empty JSON file at the specified path
        File.WriteAllText(path, "");
    }


    public void SaveJson(PowerupUnlockedDataWrapper wrapper)
    {
        string jsonDataString = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, jsonDataString);
        Debug.Log("Saved JSON data to: " + path);
    }


    public PowerupUnlockedDataWrapper LoadJson()
    {
        if (File.Exists(path))
        {
            string loadedJsonDataString = File.ReadAllText(path);
            PowerupUnlockedDataWrapper wrapper = JsonUtility.FromJson<PowerupUnlockedDataWrapper>(loadedJsonDataString);
            Debug.Log("Loaded JSON data from: " + path);
            return wrapper;
        }
        else
        {
            Debug.LogWarning("Json file not found at path: " + path);
            return null; // or handle the absence of the file as per your logic
        }
    }
}
