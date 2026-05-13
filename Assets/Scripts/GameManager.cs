using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public enum GameState { MainMenu, Playing, Paused, GameOver }

    [Header("Settings")]
    public GameState CurrentState { get; private set; }
    [SerializeField] private string mainMenuSceneName = "Menu";

    [SerializeField] private PlayerMovement player;
    public float currentDistance {  get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    #region SCENE EVENT HANDLING
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

        if (scene.name == mainMenuSceneName)
        {
            CurrentState = GameState.MainMenu;
            player = null;
        }
        else
        {
            CurrentState = GameState.Playing;
            player = FindAnyObjectByType<PlayerMovement>();
            PrepareNewGame();
        }

        Debug.Log($"<color=green>GameManager:</color> Switched to {CurrentState} state.");
    }
    #endregion

    #region GAME LOGIC
    private void PrepareNewGame()
    {
        PlayerPrefs.SetFloat("CurrentScore", 0);
        currentDistance = 0;

        if (TimerManager.Instance != null)
        {
            TimerManager.Instance.ResetTimer();
            TimerManager.Instance.StartTimer();
        }

        PlayerPrefs.Save();
    }

    public void SetGameOver()
    {
        if (CurrentState == GameState.GameOver) return;

        CurrentState = GameState.GameOver;
        Time.timeScale = 0f;

        if (TimerManager.Instance != null)
            TimerManager.Instance.StopTimer();

        Debug.Log("Game Over!");
    }

    public void TogglePause()
    {
        if (CurrentState != GameState.Playing && CurrentState != GameState.Paused) return;

        if (CurrentState == GameState.Paused)
        {
            CurrentState = GameState.Playing;
            Time.timeScale = 1f;
        }
        else
        {
            CurrentState = GameState.Paused;
            Time.timeScale = 0f;
        }
    }
    #endregion

    #region SCENE NAVIGATION
    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    #endregion

    public void QuitGame()
    {
        Application.Quit();
    }

    #region DISTANCE CALCULATION
    private void FixedUpdate()
    {
        if (CurrentState == GameState.Playing && player != null)
        {
            currentDistance += player.currentRunSpeed * Time.fixedDeltaTime;
        }
    }
    #endregion

}