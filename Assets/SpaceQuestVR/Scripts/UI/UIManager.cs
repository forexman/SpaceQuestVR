using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] Image timerElement;

    public static UIManager Instance { get; private set; }

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

    void OnEnable()
    {
        GameStateManager.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        GameStateManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameStateManager.GameState state)
    {
        if (state == GameStateManager.GameState.GameOver)
        {
            ShowGameOverScreen();
        }
        // Handle other states as needed
    }

    private void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        // Additional logic for the game over screen
    }

    public void SetSlider(float value){
        timerElement.fillAmount = value;
    }

}
