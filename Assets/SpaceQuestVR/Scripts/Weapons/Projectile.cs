using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    private float projectileSpeed;
    private int damage;
    private float lifetime = 1f;
    private float timeSinceLaunch;
    private Rigidbody rb;
    private Vector3 velocity;

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
        if (target.GetComponent<Enemy>() != null)
        {
            target.GetComponent<Enemy>().TakeDamage(damage, transform.position);
            PoolManager.Instance.ReturnToPool(gameObject.name, gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {       
        Vector3 collisionPoint = collision.contacts[0].point; // Get the collision point
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, collisionPoint);
            PoolManager.Instance.ReturnToPool(gameObject.name, gameObject);
        }
    }
}
