using UnityEngine;

public enum FiringMode { Manual, Burst, Automatic }

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public GameObject projectilePrefab;
    public FiringMode firingMode;
    public AudioClip firingSound;
    public AudioClip emptySound;
    public int roundsPerMinute;
    public int projectileDamage;
    public float projectileSpeed;
    public int projectilePoolSize;
    public int magazineCapacity;
    public float reloadTime;
}