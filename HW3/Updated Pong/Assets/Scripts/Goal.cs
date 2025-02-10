using System;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;


public class Goal : MonoBehaviour
{
    public bool scoredOnLeft;
    public GameObject ballSpawnLocation;
    public GameObject ballPrefab;
    public TextMeshProUGUI scoreText;
    public PowerupLogic powerupLogic;
    
    private static int player1Score;
    private static int player2Score;
    private float timer;
    private bool resetting;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 5f;
        resetting = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            //Updates score value, updates direction for next spawn, spawns powerups
            Vector3 direction;
            if (scoredOnLeft)
            {
                player2Score++;
                Debug.Log("Right player scored  " + player1Score.ToString() + " - " + player2Score.ToString());
                direction = new Vector3(0f, 0f, 1f);
                if (!(player1Score >= 11 || player2Score >= 11))
                {
                    powerupLogic.SpawnPowerup(1, (player2Score - player1Score) / 10f);
                }
                
            }
            else
            {
                player1Score++;
                Debug.Log("Left player scored  " + player1Score.ToString() + " - " + player2Score.ToString());
                direction = new Vector3(0f, 0f, -1f);
                if (!(player1Score >= 11 || player2Score >= 11))
                {
                    powerupLogic.SpawnPowerup(-1, (player1Score - player2Score) / 10f);
                }
            }

            if (player1Score >= 11 || player2Score >= 11)
            {
                ChangeTextColor();
                if (player1Score >= 11)
                {
                    scoreText.text = player1Score.ToString() + " - " + player2Score.ToString() + '\n' + "Game Over" +
                                     '\n' + "Left Player Wins";
                }
                else
                {
                    scoreText.text = player1Score.ToString() + " - " + player2Score.ToString() + '\n' + "Game Over" +
                                     '\n' + "Right Player Wins";
                }

                player1Score = 0;
                player2Score = 0;
                timer = 0f;
                resetting = true;
                DestroyBalls();
            }

            //Spawns balls, changes texr
            if (resetting == false)
            {
                GameObject newball = Instantiate(ballPrefab, ballSpawnLocation.transform.position, Quaternion.identity);
                Rigidbody rb = newball.GetComponent<Rigidbody>();
                rb.AddForce(direction * 300, ForceMode.Acceleration);
                SetScoreText();
                ChangeTextColor();
            }
            Destroy(other.gameObject);
        }

        //Destroys powerups if they get past the goal and replaces it
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            if (scoredOnLeft)
            {
                powerupLogic.SpawnPowerup(1f, 1f);    
            }
            else
            {
                powerupLogic.SpawnPowerup(-1f, 1f);
            }
            
        }
    } 
    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (!resetting || !(timer >= 5f)) return;
        Vector3 direction = Random.Range(0, 2) == 1 ? new Vector3(0f,0f,-1f) : new Vector3(0f,0f, 1f);
        GameObject newball = Instantiate(ballPrefab, ballSpawnLocation.transform.position, Quaternion.identity);
        Rigidbody rb = newball.GetComponent<Rigidbody>();
        rb.AddForce(direction * 300, ForceMode.Acceleration);
        SetScoreText();
        resetting = false;
    }
    
    private void SetScoreText()
    {
        scoreText.text = player1Score.ToString() + " - " + player2Score.ToString();
    }

    private void ChangeTextColor()
    {
        scoreText.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }

    private void DestroyBalls()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        //Allows all possible ball duplicates to also jump
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }
    }
}
