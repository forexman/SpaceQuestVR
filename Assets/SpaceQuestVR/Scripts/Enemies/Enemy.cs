using UnityEngine;

public class Enemy : MonoBehaviour, IEnemyBehavior, IScoreable, IPoolable
{
    private int unitHealth;
    private float unitSpeed;
    private int unitBaseScore;
    private int unitDamageToShield;
    private Rigidbody rb;
    [SerializeField] private Vector3 direction;
    [SerializeField] private GameObject collisionPrefab;
    [SerializeField] private GameObject destroyShipPrefab;
    [SerializeField] private GameObject scorePopUpPrefab;
    private AudioClip unitExplosionSFX;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(EnemyShip data)
    {
        unitHealth = data.unitHealth;
        unitBaseScore = data.baseScore;
        unitDamageToShield = data.damageToShield;
        unitExplosionSFX = data.explosionSFX;
        unitSpeed = Random.Range(data.unitSpeedMin, data.unitSpeedMax);
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            rb.velocity = direction.normalized * unitSpeed;
        }
    }

    public void Move()
    {
        transform.Translate(direction * Time.deltaTime * unitSpeed, Space.World);
    }

    public void Attack(GameObject target)
    {
        // Implement attack behavior
    }

    public void OnPlayerInteraction()
    {
        // Implement interaction behavior
    }

    public int GetDamageToShield()
    {
        return unitDamageToShield;
    }

    public void TakeDamage(int amount, Vector3 collisionPoint)
    {
        unitHealth -= amount;
        if (unitHealth <= 0)
        {
            EnemyDestroyed();
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

    public void OnObjectSpawn()
    {
        // Reset or initialize the enemy when spawned from the pool
    }

    public void ReturnToPool()
    {
        // Cleanup before returning to the pool
        PoolManager.Instance.ReturnToPool(name, gameObject);
    }

    public void EnemyDestroyed()
    {
        CalculateAndReportScore();
        PlayDestroyEffect();
        LevelManager.Instance.ShipDestroyed();
        ReturnToPool();
    }

    private void CalculateAndReportScore()
    {
        int score = CalculateScore();
        ScoreManager.Instance.AddScore(score);
        ShowScorePopup(score);
    }

    public int CalculateScore()
    {
        float shotDistance = Vector3.Distance(transform.position, Vector3.zero);
        return unitBaseScore + 10 * (int)shotDistance;
    }

    private void ShowScorePopup(int score)
    {
        GameObject scorePopUp = Instantiate(scorePopUpPrefab, transform.position, Quaternion.identity);
        PopUp popUp = scorePopUp.GetComponent<PopUp>();
        popUp.Initialize(score.ToString());
    }

    private void PlayDestroyEffect()
    {
        GameObject explosionGO = Instantiate(destroyShipPrefab, transform.position, transform.rotation);
        AudioSource exAudioSource = explosionGO.GetComponent<AudioSource>();
        if(exAudioSource) exAudioSource.PlayOneShot(unitExplosionSFX);
    }
}
