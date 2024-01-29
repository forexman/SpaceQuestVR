using UnityEngine;

public class Enemy : MonoBehaviour, IEnemyBehavior, IScoreable, IPoolable
{
    public float minSpeed = 0.1f;
    public float maxSpeed = 10f;
    private float unitSpeed;
    private Vector3 direction;

    void Start()
    {
        unitSpeed = Random.Range(minSpeed, maxSpeed);
        // Set a random direction or other initialization
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.Translate(direction * Time.deltaTime * unitSpeed, Space.World);
    }

    public void Attack()
    {
        // Implement attack behavior
    }

    public void OnPlayerInteraction()
    {
        // Implement interaction behavior
    }

    public int CalculateScore()
    {
        // Calculate score based on enemy type, distance, etc.
        return (int)(unitSpeed * 10);
    }

    public void OnObjectSpawn()
    {
        // Reset or initialize the enemy when spawned from the pool
    }

    public void ReturnToPool()
    {
        // Cleanup before returning to the pool
        PoolManager.Instance.ReturnToPool("Enemy", gameObject);
    }

    // Additional enemy-specific methods...
}
