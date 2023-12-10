using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject chooseControls;
    [SerializeField] private string shopScene;
    [SerializeField] private string playScene;
    [SerializeField] private string settingsScene;
    [SerializeField] private Animator transition;
    [SerializeField] private AudioMixer audioMixer;

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

        //music and sound
        if (PlayerPrefs.HasKey("Music"))
            audioMixer.SetFloat("MusicParam", Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20);
        else
            audioMixer.SetFloat("MusicParam", 0);

        if (PlayerPrefs.HasKey("Sound"))
            audioMixer.SetFloat("SoundParam", Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20);
        else
            audioMixer.SetFloat("SoundParam", 0);
    }

    public void Play()
    {
        StartCoroutine(LoadLevel(playScene));
    }

    public void Shop()
    {
        StartCoroutine(LoadLevel(shopScene));
    }

    public void Settings()
    {
        StartCoroutine(LoadLevel(settingsScene));
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

    IEnumerator LoadLevel(string scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
