using UnityEngine;
using System;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    public enum GameState { Waiting, Playing, GameOverDefeat, GameOverVictory }
    public static GameState currentGameState;
    public static event Action<GameState> OnGameStateChanged;
    private static List<IGameManagerListener> listeners = new List<IGameManagerListener>();
    public EnemyShip[] enemyShips;
    public Weapon[] weapons;

    public static GameStateManager Instance { get; private set; }

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

    void Start()
    {
        InitializeGameComponents();
        AudioManager.Instance.PlayAudioClip(0, true);
        currentGameState = GameState.Waiting;
    }

    private void InitializeGameComponents()
    {
        foreach (var enemyShip in enemyShips)
        {
            PoolManager.Instance.CreatePool(enemyShip.name, enemyShip.prefab, enemyShip.poolSize);
        }
    }

    public static void RegisterListener(IGameManagerListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public static void UnregisterListener(IGameManagerListener listener)
    {
        listeners.Remove(listener);
    }

    public void ChangeGameState(GameState newState)
    {
        currentGameState = newState;
        if (newState == GameState.Playing) StartNewGame();
        NotifyGameStateChange();
    }

    private void NotifyGameStateChange()
    {
        OnGameStateChanged?.Invoke(currentGameState);

        // Notifying listeners that implement IGameManagerListener
        foreach (var listener in listeners)
        {
            switch (currentGameState)
            {
                case GameState.Playing:
                    listener.OnGameStart();
                    break;
                case GameState.GameOverDefeat:
                    listener.OnGameOverDefeat();
                    break;
                case GameState.GameOverVictory:
                    listener.OnGameOverVictory();
                    break;
            }
        }
    }

    public void StartNewGame()
    {
        ShieldManager.Instance.Initialize();
        ScoreManager.Instance.SetScore(0);
        LevelManager.Instance.StartNewGame();
    }

    public void SetGameOverDefeat()
    {
        ChangeGameState(GameState.GameOverDefeat);
        LevelManager.Instance.SetLevel(-1);
        PoolManager.Instance.ReturnAllActiveItemsToPool();
        AudioManager.Instance.PlayAudioClip(1, false);
    }

    public void SetGameOverVictory()
    {
        ChangeGameState(GameState.GameOverVictory);
        PoolManager.Instance.ReturnAllActiveItemsToPool();
        AudioManager.Instance.PlayAudioClip(3, false);
    }
}
