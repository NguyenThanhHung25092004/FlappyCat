using System.Collections.Generic;
using UnityEngine;

public class RedShotPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> redshotPool = new Queue<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab); 
            obj.SetActive(false);
            redshotPool.Enqueue(obj);
        }
    }

    public GameObject getBullet()
    {
        GameObject bullet;
        if (redshotPool.Count > 0)
        {
            bullet = redshotPool.Dequeue();
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(bulletPrefab);
        }
        bullet.GetComponent<RedShotLogic>().myPool = this;
        return bullet;
    }

    public void returnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        redshotPool.Enqueue(bullet);
    }
}
