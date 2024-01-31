using UnityEngine;
using TMPro;

public class PopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float duration = 1f;
    private float timer;

    void Start()
    {
        timer = duration;
    }

    public void Initialize(string text)
    {
        scoreText.text = text;
    }

    void Update()
    {
        // Update the timer
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);

        // Ensure the popup always faces the camera
        if (Camera.main != null)
        {
            transform.LookAt(2 * transform.position - Camera.main.transform.position);
        }
    }
}
