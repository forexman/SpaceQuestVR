using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData[] levels; // Array of LevelData ScriptableObjects
    private int currentLevelIndex;
    private int shipsDestroyed;
    private float spawnTimer;
    private LevelData currentLevel;

    public static LevelManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNewGame()
    {
        currentLevelIndex = 0;
        StartLevel(currentLevelIndex);
    }

    public void SetLevel(int index)
    {
        currentLevelIndex = index;
        StartLevel(currentLevelIndex);
    }

    private void EndLevel()
    {
        currentLevel = null;
        AudioManager.Instance.PlayAudioClip(2, false);
        PoolManager.Instance.ReturnAllActiveItemsToPool();
        StartCoroutine(TransitionToNextLevel());
    }

    private IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(5); // 5 seconds delay

        currentLevelIndex++;
        if (currentLevelIndex < levels.Length)
        {
            StartLevel(currentLevelIndex);
        }
        else
        {
            GameStateManager.Instance.SetGameOverVictory();
        }
    }

    public void StartLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            currentLevel = null;
            return;
        }

        currentLevelIndex = levelIndex;
        currentLevel = levels[levelIndex];
        shipsDestroyed = 0;
        spawnTimer = 0f;

        SetEnemyRemaining(1f);
        AudioManager.Instance.PlayAudioClip(currentLevel.audioClip, true);
        UIManager.Instance.SetStageTitle("Stage " + (levelIndex + 1) + "<br>" + currentLevel.levelTitle);
    }

    void Update()
    {
        if (currentLevel && currentLevelIndex >= 0 && currentLevelIndex < levels.Length)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= currentLevel.spawnRate)
            {
                SpawnEnemy(currentLevel);
                spawnTimer = 0f;
            }
        }
    }

    private void SpawnEnemy(LevelData level)
    {
        EnemyShip selectedShip = level.enemyShips[Random.Range(0, level.enemyShips.Length)];
        EnemySpawnZone.Instance.SpawnEnemy(selectedShip);
    }

    public void ShipDestroyed()
    {
        shipsDestroyed++;
        float percentRemaining = ((float)Mathf.Max(currentLevel.shipsToDestroy - shipsDestroyed, 0)) / currentLevel.shipsToDestroy;
        SetEnemyRemaining(percentRemaining);
        if (currentLevel.shipsToDestroy <= shipsDestroyed)
        {
            EndLevel();
        }
    }

    private void SetEnemyRemaining(float value)
    {
        UIManager.Instance.SetEnemyElement(value);
    }
}
