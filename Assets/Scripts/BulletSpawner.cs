using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float targetDistance = 10f;
    [SerializeField] private Transform spawnPos;

    [SerializeField] private PlayerMovement player;
    [SerializeField] private RedShotPool pool;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Animator ani;
    private float timer = 0;

    private void OnEnable()
    {
        spawnPos = GetComponent<Transform>();
        player = GameObject.FindAnyObjectByType<PlayerMovement>();
        pool = GameObject.FindAnyObjectByType<RedShotPool>();
        ani = transform.parent.GetComponent<Animator>();
    }

    private void Update()
    {
        float currentGameSpeed = player.currentRunSpeed;
        float bulletSpeed = bulletPrefab.GetComponent<RedShotLogic>().bulletSpeed;
        float dynamicSpawnRate = targetDistance / (currentGameSpeed + bulletSpeed);

        dynamicSpawnRate = Mathf.Max(dynamicSpawnRate, 2.5f);

        timer += Time.deltaTime;
        if (timer >= dynamicSpawnRate)
        {
            if (ani != null)
            {
                ani.SetTrigger("Shoot");
            }
            timer = 0;
        }
    }

    public void SpawnBullet()
    {
        GameObject bullet = pool.getBullet();
        bullet.transform.position = spawnPos.position;
    }
}
