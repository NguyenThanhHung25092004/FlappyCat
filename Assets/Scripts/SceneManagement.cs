using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;

    private bool gameOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void loadNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("HighScore", 0);
        PlayerPrefs.Save();
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene("Menu");
        PlayerPrefs.SetFloat("HighScore", 0);
        TimerManager.instance.ResetTimer();
        PlayerPrefs.Save();
    }

    public void SetGameOver(bool state)
    {
        gameOver = state;
    }

    private void Update()
    {
        if (gameOver)
        {
            TimerManager.instance.StopTimer();
        }
    }
}