using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private int unitHealth;
    [SerializeField] private GameObject collisionPrefab; // For visual feedback on damage
    [SerializeField] private GameObject destroyShipPrefab; // For destruction visual
    [SerializeField] private AudioClip unitExplosionSFX; // For destruction sound
    private AudioSource audioSource; // To play the explosion sound

    private void Awake()
    {
        // Ensure there's an AudioSource component for playing sounds
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Initialize(int health)
    {
        unitHealth = health;
    }

    public void TakeDamage(int amount, Vector3 collisionPoint)
    {
        unitHealth -= amount;
        if (unitHealth <= 0)
        {
            PlayDestroyEffect();
            EnemyManager.Instance.HandleEnemyDestruction(gameObject, true); // Notify for destruction
        }
        else
        {
            PlayDamageEffect(collisionPoint);
        }
    }

    private void PlayDamageEffect(Vector3 collisionPoint)
    {
        if (collisionPrefab != null)
        {
            Instantiate(collisionPrefab, new Vector3 (collisionPoint.x, transform.position.y - 0.85f, collisionPoint.z), Quaternion.identity);
        }
    }

    private void PlayDestroyEffect()
    {
        // Visual effect
        if (destroyShipPrefab != null)
        {
            Instantiate(destroyShipPrefab, transform.position, Quaternion.identity);
        }
        // Sound effect
        if (unitExplosionSFX != null)
        {
            audioSource.PlayOneShot(unitExplosionSFX);
        }
    }
}
