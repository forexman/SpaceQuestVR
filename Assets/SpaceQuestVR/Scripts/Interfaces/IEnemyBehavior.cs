using UnityEngine;

public interface IEnemyBehavior
{
    /// <summary>
    /// Handles the movement of the enemy.
    /// </summary>
    void Move();

    /// <summary>
    /// Executes the attack behavior of the enemy.
    /// </summary>
    /// <param name="target">The target of the attack.</param>
    void Attack(GameObject target);

    /// <summary>
    /// Defines the behavior when the enemy interacts with the player.
    /// </summary>
    void OnPlayerInteraction();

    /// <summary>
    /// Handles receiving damage by the enemy.
    /// </summary>
    /// <param name="amount">The amount of damage taken.</param>
    /// <param name="collisionPoint">The point of collision with enemy object.</param>
    void TakeDamage(int amount, Vector3 collisionPoint);
}
