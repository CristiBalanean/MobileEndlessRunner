using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GoogleSignIn : MonoBehaviour
{
    private void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success) 
        {
            Debug.Log("Success");
        }
        else
        {
            Debug.Log("Failure");
        }    
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public static void SetHighscoreLeaderboard(int score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_highway_dash, LeaderboardUpdate);
    }

    private static void LeaderboardUpdate(bool success)
    {
        if (success) Debug.Log("Success");
        else Debug.Log("Failed");
    }
}
