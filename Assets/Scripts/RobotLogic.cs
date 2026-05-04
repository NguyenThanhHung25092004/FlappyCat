using UnityEngine;

public class RobotLogic : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (player.transform.position.x - transform.position.x > 7f)
        {
            gameObject.SetActive(false);
        }
    }
}
