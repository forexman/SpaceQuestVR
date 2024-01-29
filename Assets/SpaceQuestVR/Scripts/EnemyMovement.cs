using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy speed")]
    public float minSpeed = 0.1f;
    public float maxSpeed = 10f;

    public Vector3 direction;

    float unitSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //for now, each ship has random speed - later to be changed when classes are introduced
        unitSpeed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * unitSpeed, Space.World);
    }
}
