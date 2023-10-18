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
        }

        // Combine the path with the file name
        path = Path.Combine(path, "data.json");
    }

    public void SaveJson(CarUnlockedDataWrapper wrapper)
    {
        string jsonDataString = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, jsonDataString);
    }

    public CarUnlockedDataWrapper LoadJson()
    {
        string loadedJsonDataString = File.ReadAllText(path);
        CarUnlockedDataWrapper wrapper = JsonUtility.FromJson<CarUnlockedDataWrapper>(loadedJsonDataString);
        return wrapper;
    }
}
