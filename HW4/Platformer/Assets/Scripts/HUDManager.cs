using System;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;
    public int timerCapacity;
    public int worldNumber;
    public int levelNumber;

    private int coins = 0;
    private int score = 0;
    private int timeLeft = 0;
    private bool gameOver = false;
    
    void Start()
    {
        UpdateLevelText(worldNumber,levelNumber);
        UpdateScoreText(score);
        UpdateCoinText(coins);
    }
    
    void Update()
    {
        timeLeft = timerCapacity - (int)Time.time;
        if (!gameOver)
        {
            UpdateTimeText(timeLeft);
        }

        if (timeLeft <= 0 && !gameOver)
        {
            Debug.Log("Game Over");
            gameOver = true;
        }
    }

    public void GiveCoins(int amount)
    {
        coins += amount;
        if (coins >= 100)
        {
            coins -= 100;
            //TODO give 1up
        }
        UpdateCoinText(coins);
        GiveScore(amount * 100);
    }

    public void GiveScore(int amount)
    {
        score += amount;
        UpdateScoreText(score);
    }
    
    private void UpdateScoreText(int score)
    {
        scoreText.text = "MARIO" + '\n' + score.ToString("D6");
    }
    
    private void UpdateCoinText(int coins)
    {
        //Todo: Add coin image to text
        coinText.text = "COINS" + '\n' + coins.ToString("D2");
    }
    
    private void UpdateLevelText(int worldNum, int levelNum)
    {
        levelText.text = "WORLD" + '\n' + worldNum.ToString() + "-" + levelNum.ToString();
    }
    
    private void UpdateTimeText(int time)
    {
       timeText.text = "TIME" + '\n' + time.ToString();
    }
}
