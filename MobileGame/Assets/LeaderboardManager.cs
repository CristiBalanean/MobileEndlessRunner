using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;

    public bool connectedToGooglePlay;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        LogIntoGooglePlay();
    }

    public void LogIntoGooglePlay()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    private void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            connectedToGooglePlay = true;
        }

        else
            connectedToGooglePlay = false;
    }

    public void SendToLeaderboard(int score)
    {
        if (connectedToGooglePlay) 
        {
            Social.ReportScore(score, GPGSIds.leaderboard_highwaydashleaderboard, LeaderboardUpdate);
        }
    }

    private void LeaderboardUpdate(bool success)
    {
        if (success) Debug.Log("Updated Leaderboard");
        else Debug.Log("Updating Leaderboard Failed!");
    }
}
