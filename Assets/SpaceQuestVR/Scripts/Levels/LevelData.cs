using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public string levelTitle;
    public EnemyShip[] enemyShips;
    public int shipsToDestroy;
    public float spawnRate;
    public AudioClip audioClip;
}
