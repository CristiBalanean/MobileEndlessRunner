using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Selectable;

public class PlaymodeManager : MonoBehaviour
{
    [SerializeField] private string menuScene;
    [SerializeField] private string normalModeScene;
    [SerializeField] private string twoWaysModeScene;
    [SerializeField] private string chaosModeScene;
    [SerializeField] private string mapSelectScene;
    [SerializeField] private TMP_Text playmodeNameText;
    [SerializeField] private TMP_Text playmodeHighscoreText;

    [SerializeField] private Sprite[] playmodeSprites;
    [SerializeField] private Image lockImage;

    [SerializeField] private Button playmodeButton;

    [SerializeField] private Car monsterTruck;

    [SerializeField] private Animator transition;

    int index = 0;

    private void Start()
    {
        index = 0;
        PlayModeImageSelect();
        PlayModeNameSelect();
    }

    public void Next()
    {
        if (index >= playmodeSprites.Length - 1)
        {
            index = 0;
        }
        else
            index++;

        PlayModeImageSelect();
        PlayModeNameSelect();
    }

    public void Previous()
    {
        if (index <= 0)
        {
            index = playmodeSprites.Length - 1;
        }
        else
            index--;

        PlayModeImageSelect();
        PlayModeNameSelect();
    }

    private void PlayModeImageSelect()
    {
        playmodeButton.image.sprite = playmodeSprites[index];
    }

    private void PlayModeNameSelect()
    {
        switch (index)
        {
            case 0:
                playmodeNameText.text = "NORMAL MODE";
                lockImage.gameObject.SetActive(false);
                playmodeButton.interactable = true;
                playmodeHighscoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("HighScore").ToString();
                break;

            case 1:
                playmodeNameText.text = "TWO WAYS MODE";
                lockImage.gameObject.SetActive(false);
                playmodeButton.interactable = true;
                playmodeHighscoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("TwoWaysHighscore").ToString();
                break;

            case 2:
                playmodeNameText.text = "CHAOS MODE";
                if (!monsterTruck.IsUnlocked())
                {
                    playmodeButton.interactable = false;
                    lockImage.gameObject.SetActive(true);
                    playmodeHighscoreText.text = "UNLOCK MONSTER TRUCK TO PLAY THIS MODE!";
                }
                else
                {
                    playmodeButton.interactable = true;
                    lockImage.gameObject.SetActive(false);
                    playmodeHighscoreText.text = "HIGHSCORE: " + PlayerPrefs.GetString("MonsterTruckHighScore").ToString();
                }
                break;
        }
    }

    public void PlayModeSelectButton()
    {
        switch(index)
        {
            case 0:
                GameModeData.instance.gameMode = normalModeScene;
                break;

            case 1:
                GameModeData.instance.gameMode = twoWaysModeScene;
                break;

            case 2:
                GameModeData.instance.gameMode = chaosModeScene;
                break;
        }

        StartCoroutine(LoadLevel(mapSelectScene));
    }

    public void BackButton()
    {
        StartCoroutine(LoadLevel(menuScene));
    }

    IEnumerator LoadLevel(string scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
