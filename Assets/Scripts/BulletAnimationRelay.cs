using UnityEngine;

public class BulletAnimationRelay : MonoBehaviour
{
    [SerializeField] private BulletSpawner spawner;

    public void CallSpawnBullet()
    {
        if (spawner != null)
        {
            spawner.SpawnBullet();
        }
    }
}
