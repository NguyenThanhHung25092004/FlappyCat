using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [Header("Pause Settings")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private KeyCode pauseKeyCode = KeyCode.Escape;
    void Update()
    {
        if (Input.GetKeyDown(pauseKeyCode))
        {
            TogglePause();
        }
    }
    
    public void TogglePause()
    {
        GameManager.Instance.TogglePause();

        bool isPaused = (GameManager.Instance.CurrentState == GameManager.GameState.Paused);
        pausePanel.SetActive(isPaused);
        resumeButton.SetActive(isPaused);
        pauseButton.SetActive(!isPaused);
    }
}
