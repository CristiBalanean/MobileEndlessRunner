using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Car monsterTruck;
    [SerializeField] private Button chaosModeButton;

    private void OnEnable()
    {
        if (monsterTruck.IsUnlocked())
            chaosModeButton.interactable = true;
        else
            chaosModeButton.interactable = false;
    }

    public void NormalMode()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ChaosMode()
    {
        SceneManager.LoadScene("MonsterTruckGameMode");
    }

    public void Back()
    {
        gameObject.SetActive(false);
        menu.SetActive(true);
    }
}
