using UnityEngine;

public class EnemyScore : MonoBehaviour, IScoreable
{
    [SerializeField] private int unitBaseScore;
    [SerializeField] private GameObject scorePopUpPrefab;

    public int UnitBaseScore => unitBaseScore;

    public void Initialize(int score)
    {
        unitBaseScore = score;
    }

    public void CalculateAndReportScore()
    {
        int score = CalculateScore();
        ScoreManager.Instance.AddScore(score);
        DisplayScorePopup(score);
    }

    private int CalculateScore()
    {
        float shotDistance = Vector3.Distance(transform.position, Vector3.zero);
        return unitBaseScore + (int)shotDistance / 10;
    }

    private void DisplayScorePopup(int score)
    {
        if (scorePopUpPrefab != null)
        {
            var scorePopUpInstance = Instantiate(scorePopUpPrefab, transform.position, Quaternion.identity);
            scorePopUpInstance.GetComponent<PopUp>().Initialize(score.ToString());
        }
    }
}
