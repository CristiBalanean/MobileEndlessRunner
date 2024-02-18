using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamesPlayedManager : MonoBehaviour
{
    public static GamesPlayedManager instance;

    public int gamesPlayed = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
