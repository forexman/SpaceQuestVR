using UnityEngine;

public class EnemyEndZone : MonoBehaviour
{
    void OnTriggerEnter(Collider otherObject)
    {
        Enemy enemy = otherObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            int damage = enemy.GetDamageToShield();
            ShieldManager.Instance.ReduceShieldEnergy(damage);
            PoolManager.Instance.ReturnToPool(enemy.gameObject.name, enemy.gameObject);
        }
    }
}
