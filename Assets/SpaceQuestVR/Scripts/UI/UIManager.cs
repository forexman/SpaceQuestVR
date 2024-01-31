using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameManagerListener
{
    [SerializeField] private GameObject startMenuCanvas;
    [SerializeField] private GameObject gameOverDefeatCanvas;
    [SerializeField] private GameObject gameOverVictoryCanvas;
    [SerializeField] Image shieldElement;
    [SerializeField] Image enemiesElement;
    [SerializeField] TextMeshProUGUI shieldText;
    [SerializeField] TextMeshProUGUI stageText;

    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameStateManager.RegisterListener(this);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        GameStateManager.UnregisterListener(this);
    }

    public void OnGameStart()
    {
        UpdateUI(GameStateManager.GameState.Playing);
    }

    public void OnGameOverDefeat()
    {
        UpdateUI(GameStateManager.GameState.GameOverDefeat);
    }

    public void OnGameOverVictory()
    {
        UpdateUI(GameStateManager.GameState.GameOverVictory);
    }

    public void UpdateUI(GameStateManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameStateManager.GameState.Waiting:
                startMenuCanvas.SetActive(true);
                gameOverDefeatCanvas.SetActive(false);
                gameOverVictoryCanvas.SetActive(false);
                break;
            case GameStateManager.GameState.Playing:
                startMenuCanvas.SetActive(false);
                gameOverDefeatCanvas.SetActive(false);
                gameOverVictoryCanvas.SetActive(false);
                break;
            case GameStateManager.GameState.GameOverDefeat:
                startMenuCanvas.SetActive(false);
                gameOverDefeatCanvas.SetActive(true);
                gameOverVictoryCanvas.SetActive(false);
                break;
            case GameStateManager.GameState.GameOverVictory:
                startMenuCanvas.SetActive(false);
                gameOverDefeatCanvas.SetActive(false);
                gameOverVictoryCanvas.SetActive(true);
                break;
        }
    }

    public void SetShieldLife(float value)
    {
        shieldText.text = (value * 100).ToString();
        shieldElement.fillAmount = value;
    }

    public void SetStageTitle(string value)
    {
        stageText.text = value;
    }

    public void SetEnemyElement(float value)
    {
        enemiesElement.fillAmount = value;
    }

}
