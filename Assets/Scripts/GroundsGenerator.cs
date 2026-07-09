using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class GroundsGenerator : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private int poolSize = 10;

    [Header("Generation Settings")]
    [SerializeField] private float minGap = 3f;
    [SerializeField] private float maxGap = 9f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;

    // Khoảng cách ngoài màn hình để sinh platform 
    [SerializeField] private float spawnBuffer = 5f;

    private Queue<GameObject> platformPool;
    private Camera cam;
    private float lastPlatformRightEdge;
    private float lastPlatformY;
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private float safetyFactor = 0.85f;

    private void Awake()
    {
        cam = Camera.main;
        platformPool = new Queue<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            GameObject platform = Instantiate(platformPrefab, transform);
            platform.SetActive(false);
            platformPool.Enqueue(platform);
        }

        obstacleSpawner = GameObject.FindAnyObjectByType<ObstacleSpawner>();
    }

    private void Start()
    {
        lastPlatformRightEdge = 0;
        SpawnPlatform(new Vector2 (0, 0));
    }

    private (float gap, float deltaY) GetSafeJump()
    {
        Rigidbody2D rb = player.Rb;
        float velocityRatio = player.currentRunSpeed / player.MaxSpeed;
        float dynamicHoldTime = player.BaseMaxHoldTime * velocityRatio;

        var samples = JumpPhysicsSimulator.SimulateMaxJump(
            player.JumpForce, player.SustainedJumpForce, dynamicHoldTime,
            player.FallMultiplier, rb.mass, rb.gravityScale, Time.fixedDeltaTime);

        float rawMaxHeight = JumpPhysicsSimulator.GetMaxHeight(samples);

        float clampedMinY = Mathf.Max(minY - lastPlatformY, -rawMaxHeight);
        float clampedMaxY = Mathf.Min(maxY - lastPlatformY, rawMaxHeight);

        float deltaY = Random.Range(clampedMinY, clampedMaxY);

        float airTime = JumpPhysicsSimulator.GetAirTimeToHeight(samples, deltaY);
        if (airTime < 0f) airTime = 0.1f;

        float maxDistance = player.currentRunSpeed * airTime * safetyFactor;
        float gap = Random.Range(minGap, Mathf.Max(minGap, Mathf.Min(maxGap, maxDistance)));

        Debug.Log("MinY: " + minY + " rawMaxHeight: " + rawMaxHeight + " MaxY: " + maxY);
        Debug.Log("Max Gap: " + maxGap + " Max Distance: " + maxDistance);

        return (gap, deltaY);
    }

    private void Update()
    {
        float camRightEdge = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float camLeftEdge = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        if (camRightEdge + spawnBuffer > lastPlatformRightEdge)
        {
            var (randomGap, deltaY) = GetSafeJump();
            float randomY = Mathf.Clamp(lastPlatformY + deltaY, minY, maxY);
            Vector2 newPlatform = new Vector2(lastPlatformRightEdge + randomGap, randomY);
            SpawnPlatform(newPlatform);

        }
        GameObject oldestPlatform = platformPool.Peek();
        if (oldestPlatform.activeInHierarchy)
        {
            float oldestRightEdge = oldestPlatform.GetComponent<BoxCollider2D>().bounds.max.x;

            if(oldestRightEdge < camLeftEdge - spawnBuffer)
            {
                RecyclePlatform();
            }
        }


        
    }
    private void SpawnPlatform(Vector2 position)
    {
        GameObject platform = platformPool.Dequeue();

        platform.transform.position = position;
        platform.SetActive(true);

        obstacleSpawner.SpawnObstacle(platform);

        // Update the right edge variable of the last platform
        // because a new platform has been spawned
        BoxCollider2D col = platform.GetComponent<BoxCollider2D>();
        lastPlatformRightEdge = col.bounds.max.x;
        lastPlatformY = position.y;

        platformPool.Enqueue(platform);
    }

    private void RecyclePlatform()
    {
        GameObject platform = platformPool.Peek();
        platform.SetActive(false);
    }
}
