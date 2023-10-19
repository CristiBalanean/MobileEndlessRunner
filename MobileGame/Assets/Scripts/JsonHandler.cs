using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonHandler : MonoBehaviour
{
    public static JsonHandler instance;

    private string path;

    private void Awake()
    {
        instance = this;

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
        path = Path.Combine(path, "data.json");

        Debug.Log("JsonHandler initialized with path: " + path);
    }


    public void SaveJson(CarUnlockedDataWrapper wrapper)
    {
        string jsonDataString = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, jsonDataString);
        Debug.Log("Saved JSON data to: " + path);
    }


    public CarUnlockedDataWrapper LoadJson()
    {
        if (File.Exists(path))
        {
            string loadedJsonDataString = File.ReadAllText(path);
            CarUnlockedDataWrapper wrapper = JsonUtility.FromJson<CarUnlockedDataWrapper>(loadedJsonDataString);
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
