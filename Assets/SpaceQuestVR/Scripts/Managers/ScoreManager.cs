using UnityEngine;
using TMPro;
using static GameStateManager;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int playerScore = 0;

    public static ScoreManager Instance { get; private set; }

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

    public void AddScore(int amount)
    {
        if (currentGameState != GameState.Playing) return;
        playerScore += amount;
        scoreText.text = playerScore.ToString();
    }

    public void ResetScore()
    {
        SetScore(0);
    }

    public void SetScore(int amount)
    {
        playerScore = amount;
        scoreText.text = playerScore.ToString();
    }
}
