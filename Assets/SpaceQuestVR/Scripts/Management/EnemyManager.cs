using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HandleEnemyDestruction(GameObject enemy, bool displayScore)
    {
        if (displayScore)
        {
            enemy.GetComponent<EnemyScore>().CalculateAndReportScore();
            LevelManager.Instance.ShipDestroyed();
        }
        // Additional logic for returning the enemy to the pool or other cleanup
        PoolManager.Instance.ReturnToPool(enemy.name, enemy);
    }
}
