using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    public int maxShieldEnergy = 100;
    private int currentShieldEnergy;

    public static ShieldManager Instance { get; private set; }

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

    public void Initialize(){
        currentShieldEnergy = maxShieldEnergy;
        UIManager.Instance.SetShieldLife(currentShieldEnergy / 100f);
    }

    public void ReduceShieldEnergy(int amount)
    {
        currentShieldEnergy -= amount;
        currentShieldEnergy = Mathf.Max(currentShieldEnergy, 0);
        UIManager.Instance.SetShieldLife(currentShieldEnergy / 100f);

        if (currentShieldEnergy <= 0)
        {
            GameStateManager.Instance.SetGameOverDefeat();
        }
    }

    // Additional methods (e.g., for restoring shield energy)
}
