using UnityEngine;

public class EnemyEndZone : MonoBehaviour
{
    void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.CompareTag("EnemyShip"))
        {
            EnemyAttack attackComponent = otherObject.GetComponent<EnemyAttack>();
            if (attackComponent != null)
            {
                attackComponent.AttackShield();
            }
        }
    }
}
