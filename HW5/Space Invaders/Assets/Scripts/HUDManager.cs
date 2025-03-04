using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreTableText;

    public delegate void GameStart();
    public static event GameStart onGameStart;

    private int currentScore = 0;
    private int highScore = 0;
    void Start()
    {
        Enemy.OnEnemyDied += EnemyOnOnEnemyDied;
        Player.playerDied += PlayerOnplayerDied;
        Invoke("StartGame", 3f);
    }

    private void PlayerOnplayerDied()
    {
        currentScore = 0;
        currentScoreText.text = "SCORE\n" + currentScore.ToString("D4");
    }

    private void EnemyOnOnEnemyDied(int score)
    {
        this.currentScore+=score;
        currentScoreText.text = "SCORE\n" + currentScore.ToString("D4");
        if (currentScore >= highScore)
        {
            highScore = currentScore;
            highScoreText.text = "HI-SCORE\n" + highScore.ToString("D4");
        }
    }

    void Update()
    {
        
    }
    private void StartGame()
    {
        onGameStart?.Invoke();
        scoreTableText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDied -= EnemyOnOnEnemyDied;
        Player.playerDied -= PlayerOnplayerDied;
    }
}
