using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeData : MonoBehaviour
{
    public static GameModeData instance;

    public string gameMode;
    public int map;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
