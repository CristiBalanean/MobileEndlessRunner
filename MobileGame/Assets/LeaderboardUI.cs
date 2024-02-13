using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    public void ShowLeaderboard()
    {
        Debug.Log("HEHE");
        if (!LeaderboardManager.instance.connectedToGooglePlay) LeaderboardManager.instance.LogIntoGooglePlay();
        Social.ShowLeaderboardUI();
    }
}
