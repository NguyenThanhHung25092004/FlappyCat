using UnityEngine;

public class PlayerColision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("KillZone")
            || collision.CompareTag("Kill"))
        {
            // Game over logic
            GameManager.Instance.SetGameOver();
            gameObject.SetActive(false);
        }
    }
}
