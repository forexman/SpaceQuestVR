using UnityEngine;

public class EnemySpawnZone : MonoBehaviour
{
    [Header("Rate of enemy spawns")]
    public float spawnRate = 1f;

    [Header("Rotation of enemy spawns")]
    public Vector3 spawnRotation;
    Vector3 spawnAreaSize;
    public Vector3 spawnDirection;
    public bool spawnActive = true;
    [SerializeField] EnemyFactory enemyFactory;
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
        GameObject enemy = enemyFactory.CreateEnemy(selectedShip, spawnDirection);
        if (enemy != null) PositionEnemy(enemy);
    }

    private void PositionEnemy(GameObject enemy)
    {
        Vector3 spawnPoint = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2));

        enemy.transform.position = spawnPoint;
        enemy.transform.rotation = Quaternion.Euler(spawnRotation);
        enemy.transform.SetParent(transform);
    }
}
