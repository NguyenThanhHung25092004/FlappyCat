using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI highScoreText;

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
        float highScore = PlayerPrefs.GetFloat("High Score");
        highScoreText.text = "High Score: " + Mathf.FloorToInt(highScore).ToString() + "m";
    }
}
