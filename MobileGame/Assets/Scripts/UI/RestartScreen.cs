using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScore;

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SetFinalScore()
    {
        finalScore.text = "FINAL SCORE: " + ScoreManager.Instance.GetFinalScore();
    }
}
