using UnityEngine;

public class MenuButtonsControl : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.LoadNextScene();
    }

    public void Exit()
    {
        GameManager.Instance.QuitGame();
    }
}
