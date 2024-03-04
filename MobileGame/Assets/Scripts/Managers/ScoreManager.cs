using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private ObjectPool overtakeTextPool;

    public int scoreMultiplier = 1;

    public bool doubleMultiplier = false;

    [SerializeField] CarMovement player;

    public int score = 0;

    private float distanceTraveled;

    [SerializeField] private int overtakeCounter = 0;
    private float overtakeComboTime = 5f;
    private float currentOvertakeComboTime;

    private void Start()
    {
        if(Instance == null)
            Instance = this;

        // Start invoking the UpdateScore function every 1 second (1.0f seconds) and repeat every 1 second (1.0f seconds)
        InvokeRepeating("UpdateScore", 0f, 0.25f);

        currentOvertakeComboTime = overtakeComboTime;
    }

    private void Update()
    {
        distanceTraveled = Vector2.Distance(player.transform.position, Vector2.zero);

        if(overtakeCounter > 0)
        {
            currentOvertakeComboTime -= Time.deltaTime;
        }
        if (currentOvertakeComboTime < 0)
            overtakeCounter = 0;
    }

    private void UpdateScore()
    {
        float playerSpeed = player.GetSpeed();

        if (playerSpeed >= 60) // Only update the score when speed is within the relevant range
        {
            if (playerSpeed > 60 && playerSpeed < 80)
            {
                scoreMultiplier = 1;
            }
            else if (playerSpeed >= 80 && playerSpeed < 135)
            {
                scoreMultiplier = 10;
            }
            else if (playerSpeed >= 135 && playerSpeed < 170)
            {
                scoreMultiplier = 50;
            }
            else
            {
                scoreMultiplier = 100;
            }

            if (doubleMultiplier)
                scoreMultiplier *= 2;

            score += 1 * scoreMultiplier;
        }
        else
            scoreMultiplier = 0;
    }

    public int GetFinalScore()
    {
        return score;
    }

    public float GetDistanceTraveled()
    {
        return distanceTraveled;
    }

    public void AddToScore(int value)
    {
        score += value;
    }

    public void SetHighscore()
    {
        CheckForAchievements();

        switch(SceneManager.GetActiveScene().name)
        {
            case "SampleScene":
                if(GetFinalScore() > PlayerPrefs.GetInt("HighScore"))
                {
                    Social.ReportScore(GetFinalScore(), GPGSIds.leaderboard_normal_mode, LeaderboardUpdate);
                    PlayerPrefs.SetInt("HighScore", GetFinalScore());
                }
                break;

            case "TwoWaysGameMode":
                if (GetFinalScore() > PlayerPrefs.GetInt("TwoWaysHighscore"))
                {
                    Social.ReportScore(GetFinalScore(), GPGSIds.leaderboard_two_ways_mode, LeaderboardUpdate);
                    PlayerPrefs.SetInt("TwoWaysHighscore", GetFinalScore());
                }
                break;

            case "TimeGameMode":
                if (GetFinalScore() > PlayerPrefs.GetInt("TimeHighscore"))
                {
                    Social.ReportScore(GetFinalScore(), GPGSIds.leaderboard_time_mode, LeaderboardUpdate);
                    PlayerPrefs.SetInt("TimeHighscore", GetFinalScore());
                }
                break;

            case "MonsterTruckGameMode":
                if (GetFinalScore() > PlayerPrefs.GetInt("MonsterTruckHighScore"))
                {
                    Social.ReportScore(GetFinalScore(), GPGSIds.leaderboard_chaos_mode, LeaderboardUpdate);
                    PlayerPrefs.SetInt("MonsterTruckHighScore", GetFinalScore());
                }
                break;
        }
    }

    private void CheckForAchievements()
    {
        if(GetFinalScore() > 10000)
        {
            PlayGamesPlatform.Instance.UnlockAchievement("CgkIvZqi8NgeEAIQBw", (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked successfully!");
                    // Do any additional actions you want upon achievement unlock
                }
                else
                {
                    Debug.LogWarning("Failed to unlock achievement.");
                    // Handle the case where unlocking the achievement failed
                }
            });
        }
        if(GetFinalScore() > 100000)
        {
            PlayGamesPlatform.Instance.UnlockAchievement("CgkIvZqi8NgeEAIQCA", (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked successfully!");
                    // Do any additional actions you want upon achievement unlock
                }
                else
                {
                    Debug.LogWarning("Failed to unlock achievement.");
                    // Handle the case where unlocking the achievement failed
                }
            });
        }
        if (GetFinalScore() > 500000)
        {
            PlayGamesPlatform.Instance.UnlockAchievement("CgkIvZqi8NgeEAIQCQ", (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked successfully!");
                    // Do any additional actions you want upon achievement unlock
                }
                else
                {
                    Debug.LogWarning("Failed to unlock achievement.");
                    // Handle the case where unlocking the achievement failed
                }
            });
        }
        if (GetFinalScore() > 1000000)
        {
            PlayGamesPlatform.Instance.UnlockAchievement("CgkIvZqi8NgeEAIQCg", (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked successfully!");
                    // Do any additional actions you want upon achievement unlock
                }
                else
                {
                    Debug.LogWarning("Failed to unlock achievement.");
                    // Handle the case where unlocking the achievement failed
                }
            });
        }
    }

    public void IncrementOvertakeCounter()
    {
        overtakeCounter++;

        if(overtakeCounter == 50)
        {
            PlayGamesPlatform.Instance.UnlockAchievement("CgkIvZqi8NgeEAIQDg", (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked successfully!");
                    // Do any additional actions you want upon achievement unlock
                }
                else
                {
                    Debug.LogWarning("Failed to unlock achievement.");
                    // Handle the case where unlocking the achievement failed
                }
            });
        }

        AddToScore(overtakeCounter * 100);
        currentOvertakeComboTime = overtakeComboTime;

        GameObject overtakeText = overtakeTextPool.GetPooledObject();
        overtakeText.transform.position = player.transform.position;
        overtakeText.GetComponentInChildren<TMP_Text>().text = "+" + (overtakeCounter * 100).ToString();
        overtakeText.SetActive(true);
    }

    private static void LeaderboardUpdate(bool success)
    {
        if (success) Debug.Log("Success");
        else Debug.Log("Failed");
    }

    public int ComputeFinalMoney()
    {
        int moneyToGive = GetFinalScore() / 15 + (int)GetDistanceTraveled() / 10;
        return moneyToGive;
    }

    public void SetFinalMoney(int money)
    {
        MoneyManager.Instance.currentMoney += money;
    }
}
