using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private TMP_Text moneyEarned;
    [SerializeField] private Animator transition;

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(currentScene);
        StartCoroutine(LoadLevel(currentScene.name));
    }

    public void Exit()
    {
        StartCoroutine(LoadLevel("Menu"));
    }

    public void SetFinalScore()
    {
        finalScore.text = "FINAL SCORE: " + ScoreManager.Instance.GetFinalScore();
    }

    public void SetMoneyEarned()
    {
        moneyEarned.text = "MONEY EARNED: " + MoneyManager.Instance.moneyToGive.ToString();
    }

    IEnumerator LoadLevel(string scene)
    {
        Time.timeScale = 1;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
