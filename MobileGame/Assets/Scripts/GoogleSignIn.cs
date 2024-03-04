using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoogleSignIn : MonoBehaviour
{
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private Button savedGamesButton;
    [SerializeField] private Button leaderboardsButton;
    [SerializeField] private Button achievementsButton;
    [SerializeField] private GameObject loadingPanel;

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
        PlayGamesPlatform.Instance.Authenticate(ProcessGooglePlayGamesAuthentication);
    }

    internal void ProcessGooglePlayGamesAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Google Play Games Authentication Success");

            PlayGamesPlatform.Instance.RequestServerSideAccess(true, async code =>
            {
                Debug.Log("Authorization code: " + code);
                string token = code;

                // Now, call the asynchronous sign-in method
                await SignInWithGooglePlayGamesAsync(token);
            });
        }
        else
        {
            Debug.Log("Google Play Games Authentication Failure");
            if (!PlayerPrefs.HasKey("Warning"))
            {
                warningPanel.SetActive(true);
                PlayerPrefs.SetInt("Warning", 1);
            }
            savedGamesButton.interactable = false;
            leaderboardsButton.interactable = false;
            achievementsButton.interactable = false;
            loadingPanel.SetActive(false);
        }
    }

    async Task SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            Debug.Log("SignIn is successful.");
            loadingPanel.SetActive(false);
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            loadingPanel.SetActive(false);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            loadingPanel.SetActive(false);
        }
        CloudSavingManager.instance.CheckForData();
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }
}
