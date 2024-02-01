using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    private int damage;
    private float lifetime = 1f;
    private float timeSinceLaunch;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction, float speed)
    {
        rb.velocity = direction * speed;
        timeSinceLaunch = 0;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    void Update()
    {
        if (gameObject.activeSelf && timeSinceLaunch < lifetime)
        {
            timeSinceLaunch += Time.deltaTime;
        }
        else
        {
            PoolManager.Instance.ReturnToPool(gameObject.name, gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        IEnemyBehavior enemy = target.GetComponent<IEnemyBehavior>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, transform.position);
            PoolManager.Instance.ReturnToPool(gameObject.name, gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            PoolManager.Instance.ReturnToPool(gameObject.name, gameObject);
            return;
        }
        Vector3 collisionPoint = collision.contacts[0].point;
        IEnemyBehavior enemy = collision.collider.gameObject.GetComponent<IEnemyBehavior>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, collisionPoint);
            PoolManager.Instance.ReturnToPool(gameObject.name, gameObject);
        }
    }
}
