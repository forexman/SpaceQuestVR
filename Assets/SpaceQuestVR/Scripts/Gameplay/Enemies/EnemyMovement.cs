using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float unitSpeed;
    private Rigidbody rb;
    private Vector3 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    public void Initialize(float speed, Vector3 moveDirection)
    {
        unitSpeed = speed;
        direction = moveDirection;
    }

    private void Move()
    {
        rb.velocity = direction.normalized * unitSpeed;
    }
}
