using System;
using UnityEngine;

public class RedShotLogic : MonoBehaviour
{
    public float bulletSpeed = 1.0f;
    [SerializeField] private float lifeTime = 10f;
    private float timer;

    public RedShotPool myPool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        timer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x - bulletSpeed * Time.deltaTime, transform.position.y);
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            myPool.returnBullet(gameObject);
        }
    }
}
