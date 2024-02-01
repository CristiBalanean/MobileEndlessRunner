using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private TMP_Text moneyEarned;

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(currentScene);
        SceneManager.LoadScene(currentScene.name);
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SetFinalScore()
    {
        finalScore.text = "FINAL SCORE: " + ScoreManager.Instance.GetFinalScore();
    }

    public void SetMoneyEarned()
    {
        moneyEarned.text = "MONEY EARNED: " + PlayerPrefs.GetInt("MoneyToGive").ToString();
    }
}
