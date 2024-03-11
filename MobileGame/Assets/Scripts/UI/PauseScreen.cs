using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private TMP_Text countDownText;
    [SerializeField] private int countDownTime;
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private GameObject exitWarningPanel;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private CameraShake cameraShake;
    private int currentCountDownTime;

    private void OnEnable()
    {
        Time.timeScale = 0;
        cameraShake.enabled = false;
        audioMixer.SetFloat("SoundParam", -80);
    }

    public void ReturnButton()
    {
        StartCoroutine(Countdown());
    }

    public void RestartButton()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(currentScene);
        StartCoroutine(LoadLevel(currentScene.name));
    }

    public void ExitWarning()
    {
        exitWarningPanel.SetActive(true);
    }

    public void ExitButton()
    {
        StartCoroutine(LoadLevel("Menu"));
    }

    IEnumerator LoadLevel(string scene)
    {
        Time.timeScale = 1;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }

    IEnumerator Countdown()
    {
        currentCountDownTime = countDownTime;

        countDownText.gameObject.SetActive(true);
        countdownPanel.SetActive(true);

        gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;

        while(currentCountDownTime > 0)
        {
            countDownText.text = currentCountDownTime.ToString();

            yield return new WaitForSecondsRealtime(1f);
            currentCountDownTime--;
        }

        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

        gameObject.SetActive(false);

        if(PlayerPrefs.HasKey("Sound"))
            audioMixer.SetFloat("SoundParam", Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20);
        else
            audioMixer.SetFloat("SoundParam", 0);

        Time.timeScale = 1f;
        cameraShake.enabled = true;

        countdownPanel.SetActive(false);
        countDownText.gameObject.SetActive(false);
    }
}
