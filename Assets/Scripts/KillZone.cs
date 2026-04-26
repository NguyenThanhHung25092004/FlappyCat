using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private float offsetHeight;

    private void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        offsetHeight = transform.position.y - playerTransform.position.y;
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + offsetHeight, playerTransform.position.z);
        }
    }
}
