using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject chooseControls;
    [SerializeField] private string shopScene;
    [SerializeField] private string playScene;
    [SerializeField] private string settingsScene;

    private const string FirstTimeKey = "IsFirstTime";

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(FirstTimeKey, 1) == 1)
        {
            chooseControls.SetActive(true);
            menu.SetActive(false);
            PlayerPrefs.SetInt(FirstTimeKey, 0);
            PlayerPrefs.Save();
        }
        else
        {
            chooseControls.SetActive(false);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(playScene);
    }

    public void Shop()
    {
        SceneManager.LoadScene(shopScene);
    }

    public void Settings()
    {
        SceneManager.LoadScene(settingsScene);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void Tilt()
    {
        PlayerPrefs.SetInt("Tilt", 1);
        chooseControls.SetActive(false);
        menu.SetActive(true);
    }

    public void Touch()
    {
        PlayerPrefs.SetInt("Tilt", 0);
        chooseControls.SetActive(false);
        menu.SetActive(true);
    }
}
