using Unity.VisualScripting;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject robotPrefab;
    [SerializeField] private GameObject slidePrefab;
    private GameObject activeObstacle;

    [Header("Positioning Settings")]
    [SerializeField] private float offsetRobot = -0.1f;
    [SerializeField] private float offsetSlide = 1.5f;
    [SerializeField] private float robotEdgeBuffer = 2f;

    public void SpawnObstacle(GameObject platform)
    {
        if (Random.value > 0.3f) return;

        if (activeObstacle != null)
        {
            activeObstacle.SetActive(false);
        }

        BoxCollider2D col = platform.GetComponent<BoxCollider2D>();
        float platformCenterX = col.bounds.center.x;
        float halfWidth = col.bounds.extents.x;

        float spawnX;
        float spawnY;
        GameObject targetPrefab;

        if (Random.value < 0.5)
        {
            spawnX = platformCenterX + Random.Range(halfWidth * 0.5f, halfWidth - robotEdgeBuffer);
            spawnY = platform.transform.position.y + offsetRobot;
            targetPrefab = robotPrefab;
        } else
        {
            spawnX = platformCenterX + Random.Range(-halfWidth * 0.5f, halfWidth * 0.5f);
            spawnY = platform.transform.position.y + offsetSlide;
            targetPrefab = slidePrefab;
        }

        UpdateActiveObstacle(targetPrefab, spawnX, spawnY);
    }

    private void UpdateActiveObstacle(GameObject prefab, float x, float y)
    {
        if (activeObstacle == null || activeObstacle.name != prefab.name + "(Clone)")
        {
            activeObstacle = Instantiate(prefab);
        }

        activeObstacle.transform.position = new Vector2(x, y);
        activeObstacle.SetActive(true);
    }
}
