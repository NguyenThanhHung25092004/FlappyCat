using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private void OnEnable()
    {
        GameManager.OnGameOver += ToggleGameOver;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= ToggleGameOver;  
    }
    public void ToggleGameOver()
    {
        bool isGameOver = (GameManager.Instance.CurrentState == GameManager.GameState.GameOver);
        gameOverPanel.SetActive(isGameOver);
    }
}
