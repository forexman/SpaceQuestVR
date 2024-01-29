using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class EnemySpawnZone : MonoBehaviour
{
    [Header("Rate of enemy spawns")]
    public float spawnRate = 1f;

    [Header("Rotation of enemy spawns")]
    public Vector3 spawnRotation;

    [SerializeField] GameObject[] enemyPrefabs;

    float timeSinceLastSpawn = 0f;
    Vector3 spawnAreaSize;

    public bool spawnActive = true;

    void Start(){
        spawnAreaSize = gameObject.transform.localScale;
    }

    void Update()
    {
        if(currentGameState != GameState.Playing) return;
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > spawnRate)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPoint = transform.position + new Vector3(Random.Range(-spawnAreaSize.x/2, spawnAreaSize.x/2), Random.Range(-spawnAreaSize.y/2, spawnAreaSize.y/2), Random.Range(-spawnAreaSize.z/2, spawnAreaSize.z/2));
        GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint,  Quaternion.Euler(spawnRotation));
        enemy.transform.SetParent(this.transform);
        enemy.gameObject.tag = "EnemyShip";
    }
}
