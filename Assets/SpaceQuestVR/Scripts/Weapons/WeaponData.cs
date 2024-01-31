using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public GameObject projectilePrefab;
    public bool isAutomatic;
    public int roundsPerMinute;
    public int projectileDamage;
    public float projectileSpeed;
    public int projectilePoolSize;
}