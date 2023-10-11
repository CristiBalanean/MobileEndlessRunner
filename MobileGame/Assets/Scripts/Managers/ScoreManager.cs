using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

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
                scoreMultiplier = 5;
            }
            else if (playerSpeed >= 135 && playerSpeed < 170)
            {
                scoreMultiplier = 10;
            }
            else
            {
                scoreMultiplier = 20;
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
        if (GetFinalScore() > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", GetFinalScore());
    }

    public void IncrementOvertakeCounter()
    {
        overtakeCounter++;
        AddToScore(overtakeCounter * 10);
        currentOvertakeComboTime = overtakeComboTime;
    }
}
