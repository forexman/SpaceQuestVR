using UnityEngine;

public class EnemyEndZone : MonoBehaviour
{
    void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.CompareTag("EnemyShip"))
            if (otherObject.TryGetComponent<EnemyAttack>(out var attackComponent)) attackComponent.AttackShield();
    }
}
