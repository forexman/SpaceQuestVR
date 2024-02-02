using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private int unitDamageToShield;

    public void Initialize(int damage)
    {
        unitDamageToShield = damage;
    }

    public void AttackShield()
    {
        ShieldManager.Instance.ReduceShieldEnergy(unitDamageToShield);
        EnemyManager.Instance.HandleEnemyDestruction(gameObject, false); // false indicates no score display
    }
}
