using UnityEngine;

public class ButtonsControl : MonoBehaviour
{
    public void BackMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    public void Replay()
    {
        GameManager.Instance.ReloadScene();
    }

    public void Exit()
    {
        GameManager.Instance.QuitGame();
    }
}
