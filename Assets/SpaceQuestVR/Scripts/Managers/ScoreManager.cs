using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int playerScore = 0;

    public void AddScore(int amount)
    {
        playerScore += amount;
        scoreText.text = playerScore.ToString();
    }

    public void ResetScore()
    {
        SetScore(0);
    }

    private void SetScore(int amount)
    {
        playerScore = amount;
        scoreText.text = playerScore.ToString();
    }
}
