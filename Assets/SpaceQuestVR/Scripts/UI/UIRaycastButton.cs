using UnityEngine;

public class UIRaycastButton : MonoBehaviour, IRaycast
{
    public enum ButtonAction
    {
        StartGame
    }

    [SerializeField] private ButtonAction action;

    public void HitByRaycast()
    {
        switch (action)
        {
            case ButtonAction.StartGame:
                GameStateManager.Instance.ChangeGameState(GameStateManager.GameState.Playing);
                break;
        }
    }
}