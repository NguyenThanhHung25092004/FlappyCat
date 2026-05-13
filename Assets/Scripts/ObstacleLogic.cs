using UnityEngine;

public class ObstacleLogic : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (player.transform.position.x - transform.position.x > 8f)
        {
            gameObject.SetActive(false);
        }
    }
}
