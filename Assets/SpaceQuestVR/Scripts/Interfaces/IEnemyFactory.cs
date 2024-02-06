using UnityEngine;

public interface IEnemyFactory
{
    GameObject CreateEnemy(EnemyShip data, Vector3 moveDirection);
}
