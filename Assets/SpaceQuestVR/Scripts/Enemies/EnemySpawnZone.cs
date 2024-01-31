using UnityEngine;

public class EnemySpawnZone : MonoBehaviour
{
    [Header("Rate of enemy spawns")]
    public float spawnRate = 1f;

    [Header("Rotation of enemy spawns")]
    public Vector3 spawnRotation;

    float timeSinceLastSpawn = 0f;
    Vector3 spawnAreaSize;

    public bool spawnActive = true;
    public static EnemySpawnZone Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        spawnAreaSize = gameObject.transform.localScale;
    }

    void Update()
    {
        // if (GameStateManager.currentGameState != GameStateManager.GameState.Playing) return;
        // timeSinceLastSpawn += Time.deltaTime;
        // if (timeSinceLastSpawn > spawnRate)
        // {
        //     SpawnEnemy();
        //     timeSinceLastSpawn = 0;
        // }
    }

    public void SpawnEnemy(EnemyShip selectedShip)
    {
        GameObject enemy = PoolManager.Instance.GetFromPool(selectedShip.name);
        enemy.GetComponent<Enemy>().Initialize(selectedShip);

        if (enemy != null)
        {
            Vector3 spawnPoint = transform.position + new Vector3(Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2), Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2), Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2));
            enemy.transform.position = spawnPoint;
            enemy.transform.rotation = Quaternion.Euler(spawnRotation);
            enemy.transform.SetParent(this.transform);

            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.Initialize(selectedShip);
            }
        }
    }
}
