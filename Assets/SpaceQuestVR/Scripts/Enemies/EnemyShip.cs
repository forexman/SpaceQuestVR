using UnityEngine;

[CreateAssetMenu(fileName = "EnemyShip", menuName = "ScriptableObjects/EnemyShip", order = 1)]
public class EnemyShip : ScriptableObject
{
    public GameObject prefab;
    public int poolSize;
    public int unitHealth;
    public float unitSpeedMin;
    public float unitSpeedMax;
    public int baseScore;
    public int damageToShield;
    public AudioClip explosionSFX;
}