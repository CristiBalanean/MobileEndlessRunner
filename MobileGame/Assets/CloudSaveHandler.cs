using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSaveHandler : MonoBehaviour
{
    public static CloudSaveHandler instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationFocus(bool focus)
    {
        
    }
}
