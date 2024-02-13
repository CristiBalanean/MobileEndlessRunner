using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] private string playModeScene;
    [SerializeField] private string normalModeScene;
    [SerializeField] private string chaosModeScene;

    [SerializeField] private Sprite[] mapSprites;

    [SerializeField] private Animator transition;

    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;

    [SerializeField] int mapsNumber = 3;
    int index = 0;

    public void Next()
    {
        if (index >= mapsNumber - 1)
        {
            index = 0;
        }
        else
            index++;

        ChangeButtonText();
    }

    public void Previous()
    {
        if (index <= 0)
        {
            index = mapsNumber - 1;
        }
        else
            index--;

        ChangeButtonText();
    }

    public void MapSelectButton()
    {
        GameModeData.instance.map = index;
        StartCoroutine(LoadLevel(GameModeData.instance.gameMode));
    }

    private void ChangeButtonText()
    {
        button.GetComponent<Image>().sprite = mapSprites[index];

        switch (index)
        {
            case 0:
                buttonText.text = "FOREST";
                break;

            case 1:
                buttonText.text = "DESERT";
                break;

            case 2:
                buttonText.text = "SNOW";
                break;
        }
    }

    public void BackButton()
    {
        StartCoroutine(LoadLevel(playModeScene));
    }

    IEnumerator LoadLevel(string scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
