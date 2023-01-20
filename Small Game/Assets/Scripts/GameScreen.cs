using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreen : MonoBehaviour
{
    AudioSource SystemAudioSource;
    public static AudioClip ClickSound;
    int PlayerChoice;
    public static bool AllowBloom = true;
    public Toggle BloomSettingTick;
    private void Awake() {
        PlayerChoice = PlayerPrefs.GetInt("PlayerChoice");
        if(PlayerChoice == 0)
        {
            AllowBloom = false;
        }
        if(PlayerChoice == 1)
        {
            AllowBloom = true;
        }
    }
    private void Start() {
        SystemAudioSource = GameObject.FindGameObjectWithTag("SystemSound").GetComponent<AudioSource>();
        ClickSound = Resources.Load<AudioClip>("Click");
    }
    public void GotoMenu()
    {
        SystemAudioSource = GameObject.FindGameObjectWithTag("SystemSound").GetComponent<AudioSource>();
        SystemAudioSource.PlayOneShot(ClickSound);
        SceneManager.LoadScene("Menu");
        Time.timeScale = 0;
    }
    public void GameOver()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void Restart()
    {
        SystemAudioSource.PlayOneShot(ClickSound);
        SceneManager.LoadScene("MainGame");
        Time.timeScale = 1;
    }
    public void StartGame()
    {
        SystemAudioSource.PlayOneShot(ClickSound);
        SceneManager.LoadScene("MainGame");
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        SystemAudioSource.PlayOneShot(ClickSound);
        Application.Quit();
    }
    public void LoadSettingScreen()
    {
        gameObject.SetActive(true);
        if(!AllowBloom)
        {
            BloomSettingTick.isOn = false;
        }
        if(AllowBloom)
        {
            BloomSettingTick.isOn = true;
        }
    }
    public void BloomSetting(bool BloomBool)
    {
        AllowBloom = BloomBool;
        if(BloomBool == false)
        {
            PlayerPrefs.SetInt("PlayerChoice", 0);
        }
        if(BloomBool == true)
        {
            PlayerPrefs.SetInt("PlayerChoice", 1);
        }
    }
    public void ResumeMainMenu()
    {
        gameObject.SetActive(false);
    }
}
