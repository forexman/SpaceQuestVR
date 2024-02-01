using System.Collections;
using TMPro;
using UnityEngine;

public class PracticeTarget : MonoBehaviour, IEnemyBehavior
{
    private float damageThisInterval = 0f;
    private const float updateInterval = 1f;
    [SerializeField] private TextMeshProUGUI dpsText;

    void Start()
    {
        StartCoroutine(DPSUpdateRoutine());
    }

    public void TakeDamage(int amount, Vector3 collisionPoint)
    {
        damageThisInterval += amount;
    }

    IEnumerator DPSUpdateRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            UpdateDPSDisplay();
            ResetIntervalDamage();
        }
    }

    private void UpdateDPSDisplay()
    {
        float dps = damageThisInterval / updateInterval;
        dpsText.text = dps.ToString();
    }

    private void ResetIntervalDamage()
    {
        damageThisInterval = 0f;
    }

    public void Move()
    {
        return;
    }

    public void Attack(GameObject target)
    {
        return;
    }

    public void OnPlayerInteraction()
    {
        return;
    }
}
