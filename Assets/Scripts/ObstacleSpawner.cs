using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject robotPrefab;
    [SerializeField] private GameObject slidePrefab;

    [Header("Positioning Settings")]
    [SerializeField] private float offsetRobot = -0.1f;
    [SerializeField] private float offsetSlide = 1.5f;
    [SerializeField] private float robotEdgeBuffer = 4f;

    private List<GameObject> robotPool = new List<GameObject>();
    private List<GameObject> slidePool = new List<GameObject>();

    public void SpawnObstacle(GameObject platform)
    {
        if (Random.value > 0.4f) return;

        BoxCollider2D col = platform.GetComponent<BoxCollider2D>();
        float platformCenterX = col.bounds.center.x;
        float halfWidth = col.bounds.extents.x;

        float spawnX;
        float spawnY;

        if (Random.value < 0.5)
        {
            spawnX = platformCenterX + Random.Range(halfWidth * 0.5f, halfWidth - robotEdgeBuffer);
            spawnY = platform.transform.position.y + offsetRobot;

            GameObject robot = GetObjectFromPool(robotPool, robotPrefab);
            robot.transform.position = new Vector2(spawnX, spawnY);
            robot.SetActive(true);
        } else
        {
            spawnX = platformCenterX + Random.Range(-halfWidth * 0.5f, halfWidth * 0.5f);
            spawnY = platform.transform.position.y + offsetSlide;

            GameObject slideObstacle = GetObjectFromPool(slidePool, slidePrefab);
            slideObstacle.transform.position = new Vector2(spawnX, spawnY);
            slideObstacle.SetActive(true);
        }
    }

    private GameObject GetObjectFromPool(List<GameObject> pool, GameObject prefab)
    {
        foreach (var obj in pool)
        {
            if(!obj.activeInHierarchy)
            {
                return obj;
            }            
        }

        GameObject newObj = Instantiate(prefab);
        pool.Add(newObj);
        return newObj;
    }
}
