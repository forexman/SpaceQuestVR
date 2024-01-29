using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Image timerElement;
    [SerializeField] float roundTime;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;

    float sliderFill = 1f;
    int playerScore = 0;

    public enum GameState
    {
        Waiting,
        Playing,
        GameOver
    }
    public static GameState currentGameState;

    void Awake()
    {
        currentGameState = GameState.Waiting;
        PlayAudioClip(audioClips[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGameState == GameState.Playing) SetTimer();
    }

    public void StartGame(){
        PlayAudioClip(audioClips[1]);
        currentGameState = GameState.Playing;
        sliderFill = 1f;
        timerElement.fillAmount = 1f;
        SetPlayerScore(0);
    }

    public void GameOver(){
        currentGameState = GameState.GameOver;
        gameOverScreen.SetActive(true);
        PlayAudioClip(audioClips[2]);
    }

    void SetTimer()
    {
        timerElement.fillAmount = Math.Max(sliderFill - (Time.deltaTime / roundTime), 0f);
        sliderFill = timerElement.fillAmount;
        if(sliderFill == 0f){
            GameOver();
        }
    }

    public void AddPlayerScore(int amount)
    {
        playerScore += amount;
        scoreText.text = playerScore.ToString();
    }

    public void SetPlayerScore(int amount)
    {
        playerScore = amount;
        scoreText.text = playerScore.ToString();
    }

    void PlayAudioClip(AudioClip clip, bool loop = true){
        audioSource.clip = clip;
        audioSource.Play();
        audioSource.loop = loop;
    }
}
