using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int scoreMultiplier = 1;

    public bool doubleMultiplier = false;

    [SerializeField] TMP_Text scoreText;

    [SerializeField] CarMovement player;

    private int score = 0;

    private void Start()
    {
        if(Instance == null)
            Instance = this;

        // Start invoking the UpdateScore function every 1 second (1.0f seconds) and repeat every 1 second (1.0f seconds)
        InvokeRepeating("UpdateScore", 0f, 0.25f);
    }

    private void UpdateScore()
    {
        float playerSpeed = player.GetSpeed();

        if (playerSpeed >= 75) // Only update the score when speed is within the relevant range
        {
            if (playerSpeed > 75 && playerSpeed < 90)
            {
                scoreMultiplier = 1;
            }
            else if (playerSpeed >= 90 && playerSpeed < 135)
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

            UpdateScoreText();
        }
        else
            scoreMultiplier = 0;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "SCORE: " + score.ToString();
    }

    public int GetFinalScore()
    {
        return score;
    }

    public void AddToScore(int value)
    {
        score += value;
    }
}
