using UnityEngine;

public class EnemySpawnZone : MonoBehaviour
{
    [Header("Rate of enemy spawns")]
    public float spawnRate = 1f;

    [Header("Rotation of enemy spawns")]
    public Vector3 spawnRotation;

    float timeSinceLastSpawn = 0f;
    Vector3 spawnAreaSize;
    public Vector3 spawnDirection;
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

    public void SpawnEnemy(EnemyShip selectedShip)
    {
        GameObject enemy = PoolManager.Instance.GetFromPool(selectedShip.name);
        if (enemy != null)
        {
            InitializeEnemyComponents(enemy, selectedShip);
            PositionEnemy(enemy);
        }
    }

    private void InitializeEnemyComponents(GameObject enemy, EnemyShip data)
    {
        // Initialize components like EnemyHealth, EnemyMovement, etc.
        var health = enemy.GetComponent<EnemyHealth>();
        var movement = enemy.GetComponent<EnemyMovement>();
        var attack = enemy.GetComponent<EnemyAttack>();
        var score = enemy.GetComponent<EnemyScore>();

        if (health) health.Initialize(data.unitHealth);
        if (movement) movement.Initialize(Random.Range(data.unitSpeedMin, data.unitSpeedMax), spawnDirection);
        if (attack) attack.Initialize(data.damageToShield);
        if (score) score.Initialize(data.baseScore);
    }

    private void PositionEnemy(GameObject enemy)
    {
        Vector3 spawnPoint = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2), 
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2), 
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2));
        
        enemy.transform.position = spawnPoint;
        enemy.transform.rotation = Quaternion.Euler(spawnRotation);
        enemy.transform.SetParent(this.transform); // Optional, depending on whether you want to keep the hierarchy organized
    }
}
