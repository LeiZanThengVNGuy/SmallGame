using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScreen : MonoBehaviour
{
    AudioSource SystemAudioSource;
    public static AudioClip ClickSound;
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
}
