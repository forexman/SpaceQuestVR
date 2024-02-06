using UnityEngine;

public class EnemyFactory : MonoBehaviour, IEnemyFactory
{
    public GameObject CreateEnemy(EnemyShip data, Vector3 moveDirection)
    {
        GameObject enemy = PoolManager.Instance.GetFromPool(data.name);
        if (enemy != null)
        {
            InitializeEnemyComponents(enemy, data, moveDirection);
            if (enemy.TryGetComponent<DissolvingController>(out var dC)) dC.Appear();
        }
        return enemy;
    }

    private void InitializeEnemyComponents(GameObject enemy, EnemyShip data, Vector3 moveDirection)
    {
        if (enemy.TryGetComponent<EnemyHealth>(out var health)) health.Initialize(data.unitHealth);
        if (enemy.TryGetComponent<EnemyMovement>(out var movement)) movement.Initialize(Random.Range(data.unitSpeedMin, data.unitSpeedMax), moveDirection);
        if (enemy.TryGetComponent<EnemyAttack>(out var attack)) attack.Initialize(data.damageToShield);
        if (enemy.TryGetComponent<EnemyScore>(out var score)) score.Initialize(data.baseScore);
    }
}
