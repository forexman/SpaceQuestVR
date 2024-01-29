using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject destroyShip;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject scorePopUpPrefab;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void EnemyDestroyed()
    {
        // at the moment calculate score based on distance, player is at center
        float shotDistance = Vector3.Distance(transform.position, Vector3.zero);
        int score = 10 * (int)shotDistance;
        gameManager.AddPlayerScore(score);

        GameObject scorePopUp = Instantiate(scorePopUpPrefab, transform.position, Quaternion.identity);
        PopUpManager scorePopUpManager = scorePopUp.GetComponent<PopUpManager>();
        scorePopUpManager.Initiliaze(score.ToString());
        // scorePopUp.transform.localScale = new Vector3(transform.localScale.x * shotDistance, transform.localScale.y * shotDistance, transform.localScale.z * shotDistance);

        Instantiate(destroyShip, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
