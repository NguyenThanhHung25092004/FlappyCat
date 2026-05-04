using UnityEngine;

public class PlayerColision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("KillZone"))
        {
            // Game over logic
            gameObject.SetActive(false);
        }
    }
}
