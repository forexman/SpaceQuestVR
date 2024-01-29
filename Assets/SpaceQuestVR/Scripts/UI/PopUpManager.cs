using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] float duration = 1f;

    public void Initiliaze(string text)
    {
        scoreText.text = text;
        Destroy(gameObject, duration);
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
