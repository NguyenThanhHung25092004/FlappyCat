using System.Collections.Generic;
using UnityEngine;

public class InfinitePlatforms : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private int poolSize = 10;

    [SerializeField] private float spawnBuffer = 5f;

    private Queue<GameObject> platformPool;
    private Camera cam;
    private float lastPlatformRightEdge;
    private float lastPlatformAboveEdge;

    private void Awake()
    {
        cam = Camera.main;
        platformPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject platform = Instantiate(platformPrefab, transform);
            platform.SetActive(false);
            platformPool.Enqueue(platform);
        }
    }

    private void Start()
    {
        lastPlatformRightEdge = 0;
        SpawnPlatform(new Vector2(0, 0));
    }

    private void Update()
    {
        float camRightEdge = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float camLeftEdge = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        if (camRightEdge + spawnBuffer > lastPlatformRightEdge)
        {
            Vector2 newPlatform = new Vector2(lastPlatformRightEdge, lastPlatformAboveEdge);
            SpawnPlatform(newPlatform);

        }
        GameObject oldestPlatform = platformPool.Peek();
        if (oldestPlatform.activeInHierarchy)
        {
            float oldestRightEdge = oldestPlatform.GetComponent<BoxCollider2D>().bounds.max.x;

            if (oldestRightEdge < camLeftEdge - spawnBuffer)
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

        // Update the right edge variable of the last platform
        // because a new platform has been spawned
        BoxCollider2D col = platform.GetComponent<BoxCollider2D>();
        lastPlatformRightEdge = col.bounds.max.x;
        lastPlatformAboveEdge = col.bounds.max.y;


        platformPool.Enqueue(platform);
    }

    private void RecyclePlatform()
    {
        GameObject platform = platformPool.Peek();
        platform.SetActive(false);
    }
}
