using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    public enum GameState { Waiting, Playing, GameOver }
    public static GameState currentGameState;
    public static event Action<GameState> OnGameStateChanged;
    [SerializeField] float roundTime;
    private float sliderFill;

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

        currentGameState = GameState.Waiting;
    }

    void Update()
    {
        if (currentGameState == GameState.Playing) SetTimer();
    }

    void SetTimer()
    {
        sliderFill = Math.Max(sliderFill - (Time.deltaTime / roundTime), 0f);
        UIManager.Instance.SetSlider(sliderFill);
        if(sliderFill == 0f){
            SetGameOver();
        }
    }

    public void ChangeGameState(GameState newState)
    {
        currentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public void SetGameOver()
{
    ChangeGameState(GameState.GameOver);
    // Additional game over logic, if needed
}
}
