using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private TMP_Text moneyEarned;
    [SerializeField] private Animator transition;
    [SerializeField] private Button reviveButton;
    [SerializeField] private Button doubleMoneyButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button continueButton;

    public bool isRewarded = false;
    public RewardType rewardType;

    private void OnEnable()
    {
        restartButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
    }

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

    public void ShowAdRevive()
    {
        rewardType = RewardType.Revive;
        AdsManager.instance.rewardedAds.ShowRewardedAds();
        reviveButton.interactable = false;
        doubleMoneyButton.interactable = false;
    }

    public void ShowAdDoubleMoney()
    {
        rewardType = RewardType.DoubleMoney;
        AdsManager.instance.rewardedAds.ShowRewardedAds();
        reviveButton.interactable = false;
        doubleMoneyButton.interactable = false;
    }

    public void ContinueButton()
    {
        CarHealth.Instance.ContinueGame();
        gameObject.SetActive(false);
    }

    IEnumerator LoadLevel(string scene)
    {
        Time.timeScale = 1;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }

    public enum RewardType
    {
        DoubleMoney,
        Revive
    }

    public void HandleRewardedAdComplete(RewardType rewardType)
    {
        // Handle the completion of the rewarded ad based on the reward type
        switch (rewardType)
        {
            case RewardType.DoubleMoney:
                MoneyManager.Instance.ComputeFinalMoney();
                break;

            case RewardType.Revive:
                restartButton.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(true);
                break;

            default:
                break;
        }
    }
}
